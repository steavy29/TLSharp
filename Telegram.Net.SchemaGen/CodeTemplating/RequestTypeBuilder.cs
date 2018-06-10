using System.Collections.Generic;
using System.Linq;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class RequestTypeBuilder
    {
        private readonly string RequestNameCursor = "RequestNameCursor";
        private readonly string CodeCursor = "CodeCursor";
        private readonly string ParamFieldsCursor = "ParamFieldsCursor";
        private readonly string ResultsCursor = "ResultsCursor";
        private readonly string ConstructorParamsCursor = "ConstructorParamsCursor";
        private readonly string ParamFieldsInitCursor = "ParamFieldsInitCursor";
        private readonly string OnSendBodyCursor = "OnSendBodyCursor";
        private readonly string OnResponseCursor = "OnResponseCursor";

        private readonly string CodeFieldName = "Code";
        private readonly string ResultFieldName = "Result";

        private readonly CodeTemplate codeTemplate;
        private readonly ApiSchema.MethodInfo methodInfo;

        public string NamespaceName { get; private set; }
        public string RequestName { get; private set; }
        public string ResultType { get; private set; }
        public bool HasReturnType => ApiSchema.IsSkipItemType(ResultType);

        public RequestTypeBuilder(CodeTemplate codeTemplate, ApiSchema.MethodInfo methodInfo)
        {
            this.codeTemplate = codeTemplate;
            this.methodInfo = methodInfo;
        }

        public void Build()
        {
            (string, string) methodFieldSplitted = Extensions.SplitToTuple(methodInfo.Method, '.', false);

            NamespaceName = methodFieldSplitted.Item1;
            RequestName = CodeSnippets.Naming.CamelCase(methodFieldSplitted.Item2, true);

            codeTemplate.Replace(RequestNameCursor, RequestName);

            BuildCode();

            var classFields = new List<CodeSnippets.ClassField>(methodInfo.Params.Count);
            foreach (var paramField in methodInfo.Params)
            {
                if (ApiSchema.IsSkipItemType(paramField.Type))
                {
                    continue;
                }

                var isCustomType = ApiSchema.BasicTypes.Contains(paramField.Type);
                var fieldType = CodeSnippets.Naming.CamelCase(paramField.Type, isCustomType);
                var fieldName = CodeSnippets.Naming.CamelCase(paramField.Name, false);

                var classField = new CodeSnippets.ClassField
                {
                    Access = CodeSnippets.Access.Private,
                    IsReadonly = true,
                    Type = fieldType,
                    Name = fieldName
                };

                classFields.Add(classField);
            }

            codeTemplate.Replace(ParamFieldsCursor, classFields.Select(f => f.ToString()).ToList());

            BuildResult();
            BuildConstructor(classFields);
            BuildOnSend(classFields);
            BuildOnResponse();
        }

        private void BuildCode()
        {
            codeTemplate.Replace(CodeCursor, methodInfo.Id);
        }

        private void BuildResult()
        {
            (string, string) typeSplitted = Extensions.SplitToTuple(methodInfo.Type, '.', false);
            ResultType = typeSplitted.Item2;

            if (HasReturnType)
            {
                codeTemplate.RemoveCursorFullLine(ResultsCursor, true);
                return;
            }

            var resultField = new CodeSnippets.ClassField
            {
                Access = CodeSnippets.Access.Public,
                Type = ResultType,
                Name = ResultFieldName
            };

            codeTemplate.Replace(ResultsCursor, resultField.ToString());
        }

        private void BuildConstructor(List<CodeSnippets.ClassField> classFields)
        {
            var constructorParams = new List<CodeSnippets.MethodParam>(classFields.Count);
            var fieldsAssignments = new List<CodeSnippets.Assignment>(classFields.Count);
            foreach (var classField in classFields)
            {
                var methodParam = new CodeSnippets.MethodParam
                {
                    Type = classField.Type,
                    Name = classField.Name
                };

                constructorParams.Add(methodParam);

                var assignment = new CodeSnippets.Assignment
                {
                    Field = $"this.{classField.Name}",
                    Value = classField.Name
                };

                fieldsAssignments.Add(assignment);
            }

            var constructorParamsCodeStr = CodeSnippets.ConcatMethodParams(constructorParams);

            codeTemplate.Replace(ConstructorParamsCursor, constructorParamsCodeStr);
            codeTemplate.Replace(ParamFieldsInitCursor, fieldsAssignments.StringifyEnumerable().ToList());
        }

        private void BuildOnSend(List<CodeSnippets.ClassField> classFields)
        {
            var onSendSection = new List<string>
            {
                $"writer.Write({CodeFieldName});",
                string.Empty
            };

            foreach (var field in classFields)
            {
                onSendSection.Add($"writer.Write({field.Name});");
            }

            codeTemplate.Replace(OnSendBodyCursor, onSendSection);
        }

        private void BuildOnResponse()
        {
            if (!HasReturnType)
            {
                codeTemplate.RemoveCursorFullLine(OnResponseCursor);
                return;
            }
            
            var readStatement = ApiSchema.BasicTypes.Contains(methodInfo.Type) ?
                    $"reader.Read({ResultFieldName});" :
                    $"{ResultFieldName} = TL.Parse<{ResultType}>(reader);";

            codeTemplate.Replace(OnResponseCursor, readStatement);
        }
    }
}
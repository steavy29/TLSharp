using System.Collections.Generic;
using System.Linq;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class RequestTypeBuilder : CodeBuilder
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

        private readonly ApiSchema.MethodInfo methodInfo;

        public string NamespaceName { get; private set; }
        public string RequestName { get; private set; }
        public string ResultType { get; private set; }
        public bool HasReturnType => !ApiSchema.IsSkipItemType(ResultType);

        public RequestTypeBuilder(CodeTemplateInstance codeTemplate, ApiSchema.MethodInfo methodInfo) : base(codeTemplate)
        {
            this.methodInfo = methodInfo;
        }

        public override void Build()
        {
            (string, string) methodFieldSplitted = Extensions.SplitToTuple(methodInfo.Method, '.', false);

            NamespaceName = methodFieldSplitted.Item1;
            RequestName = CodeSnippets.Naming.CamelCase(methodFieldSplitted.Item2, true);

            CodeTemplate.Replace(RequestNameCursor, RequestName);

            BuildCode();

            var classFields = new List<CodeSnippets.ClassField>(methodInfo.Params.Count);
            foreach (var paramField in methodInfo.Params)
            {
                if (ApiSchema.IsSkipItemType(paramField.Type))
                {
                    continue;
                }

                var isCustomType = ApiSchema.ApiTypes.IsCustomType(paramField.Type);
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

            BuildParamFields(classFields);

            BuildResult();
            BuildConstructor(classFields);
            BuildOnSend(classFields);
            BuildOnResponse();
        }

        private void BuildCode()
        {
            CodeTemplate.Replace(CodeCursor, methodInfo.Id);
        }

        private void BuildParamFields(List<CodeSnippets.ClassField> classFields)
        {
            CodeTemplate.Replace(ParamFieldsCursor, classFields.Select(f => f.ToString()).ToList(), EmptyMultilineReplace.RemoveCursorLine);
        }

        private void BuildResult()
        {
            (string, string) typeSplitted = Extensions.SplitToTuple(methodInfo.Type, '.', false);
            ResultType = typeSplitted.Item2;

            if (!HasReturnType)
            {
                CodeTemplate.RemoveCursorFullLine(ResultsCursor, true);
                return;
            }

            var resultProperty = new CodeSnippets.ClassProperty
            {
                Access = CodeSnippets.Access.Public,
                Type = ResultType,
                Name = ResultFieldName,
                HasGet = true,
                GetAccess = CodeSnippets.Access.Public,
                HasSet = true,
                SetAccess = CodeSnippets.Access.Private
            };

            CodeTemplate.Replace(ResultsCursor, resultProperty.ToString());
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

            CodeTemplate.Replace(ConstructorParamsCursor, constructorParamsCodeStr);

            CodeTemplate.Replace(ParamFieldsInitCursor, fieldsAssignments.StringifyEnumerable().ToList(), EmptyMultilineReplace.RemoveCursorLine);
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

            CodeTemplate.Replace(OnSendBodyCursor, onSendSection);
        }

        private void BuildOnResponse()
        {
            if (!HasReturnType)
            {
                CodeTemplate.RemoveCursorFullLine(OnResponseCursor);
                return;
            }

            var readStatement = ApiSchema.ApiTypes.IsBasicType(methodInfo.Type) ?
                    $"reader.Read({ResultFieldName});" :
                    $"{ResultFieldName} = TL.Parse<{ResultType}>(reader);";

            CodeTemplate.Replace(OnResponseCursor, readStatement);
        }
    }
}
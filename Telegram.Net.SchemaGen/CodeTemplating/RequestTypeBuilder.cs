using System.Collections.Generic;
using System.Linq;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public static class CodeSnippets
    {
        public enum Access
        {
            Private,
            Public
        }

        public class ClassField
        {
            public string Type;
            public string Name;
            public string DefaultValue;
            public Access Access;
            public bool IsReadonly;
            public bool IsStatic;

            public override string ToString()
            {
                var result = $"{Access.ToString().ToLower()} {(IsStatic ? "static" : "")} {(IsReadonly ? "readonly" : "")} {Type} {Name}".Replace("  ", " ");
                if (DefaultValue != null)
                {
                    result += $" {DefaultValue}";
                }
                return result + ";";
            }
        }

        public class MethodParam
        {
            public string Type;
            public string Name;

            public override string ToString()
            {
                return $"{Type} {Name}";
            }
        }

        public class Assignment
        {
            public string Field;
            public string Value;

            public override string ToString()
            {
                return $"{Field} = {Value};";
            }
        }

        public static string ConcatMethodParams(List<MethodParam> methodParams) => string.Join(", ", methodParams.Select(p => p.ToString()));
    }

    public class RequestTypeBuilder
    {
        private readonly CodeTemplate codeTemplate;
        private readonly ApiSchema.MethodInfo methodInfo;

        public string NamespaceName { get; private set; }
        public string RequestName { get; private set; }

        public RequestTypeBuilder(CodeTemplate codeTemplate, ApiSchema.MethodInfo methodInfo)
        {
            this.codeTemplate = codeTemplate;
            this.methodInfo = methodInfo;
        }

        public void Build()
        {
            (string, string) methodFieldSplitted = Extensions.SplitToTuple(methodInfo.Method, '.', false);

            NamespaceName = methodFieldSplitted.Item1;
            RequestName = methodFieldSplitted.Item2;

            codeTemplate.Replace("RequestNameCursor", RequestName);

            var classFields = new List<CodeSnippets.ClassField>(methodInfo.Params.Count);
            foreach (var paramField in methodInfo.Params)
            {
                if (ApiSchema.IsSkipItemType(paramField.Type))
                {
                    continue;
                }

                var classField = new CodeSnippets.ClassField
                {
                    Access = CodeSnippets.Access.Private,
                    IsReadonly = true,
                    Type = paramField.Type,
                    Name = paramField.Name
                };

                classFields.Add(classField);
            }

            codeTemplate.Replace("ParamFieldsCursor", classFields.Select(f => f.ToString()).ToList());

            BuildResult();
            BuildConstructor(classFields);
        }

        private void BuildResult()
        {
            (string, string) typeSplitted = Extensions.SplitToTuple(methodInfo.Type, '.', false);
            var resultType = typeSplitted.Item2;

            if (ApiSchema.IsSkipItemType(resultType))
            {
                // TODO: FIX NULL
                codeTemplate.Replace("ResultsCursor", (string)null);
                return;
            }

            var resultField = new CodeSnippets.ClassField
            {
                Access = CodeSnippets.Access.Public,
                Type = resultType,
                Name = resultType
            };

            codeTemplate.Replace("ResultsCursor", resultField.ToString());
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
            codeTemplate.Replace("ConstructorParamsCursor", constructorParamsCodeStr);
            codeTemplate.Replace("ParamFieldsInitCursor", fieldsAssignments.StringifyEnumerable().ToList());
        }
    }
}
using System.Collections.Generic;
using System.Text;

using Telegram.Net.SchemaGen.Parser;

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
    }

    public class RequestTypeBuilder
    {
        private readonly CodeTemplate codeTemplate;
        private readonly RawSchema.MethodInfo methodInfo;

        public string NamespaceName { get; private set; }
        public string RequestName { get; private set; }

        public RequestTypeBuilder(CodeTemplate codeTemplate, RawSchema.MethodInfo methodInfo)
        {
            this.codeTemplate = codeTemplate;
            this.methodInfo = methodInfo;
        }

        public void Build()
        {
            var methodFieldSplitted = methodInfo.method.Split('.');

            NamespaceName = methodFieldSplitted[0];
            RequestName = methodFieldSplitted[1];

            var typeSplitted = methodInfo.type.Split('.');
            var resultNamespace = typeSplitted[0];
            var resultType = typeSplitted[1];

            codeTemplate.Replace("RequestNameCursor", RequestName);

            var paramFieldLines = new List<string>(methodInfo.@params.Count);
            foreach (var paramField in methodInfo.@params)
            {
                var classField = new CodeSnippets.ClassField
                {
                    Access = CodeSnippets.Access.Private,
                    IsReadonly = true,
                    Type = paramField.type,
                    Name = paramField.name
                };

                paramFieldLines.Add(classField.ToString());
            }

            var resultField = new CodeSnippets.ClassField
            {
                Access = CodeSnippets.Access.Public,
                Type = resultType,
                Name = resultType
            };

            codeTemplate.Replace("ParamFieldsCursor", paramFieldLines);
        }
    }
}
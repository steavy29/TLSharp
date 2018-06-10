using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                var result = $"{Access.ToString().ToLower()} {(IsStatic ? "static" : "")} {(IsReadonly ? "readonly" : "")} {Type} {Name}";

                result = Extensions.CompressSpaces(result);

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

        public static class Naming
        {
            public static string CamelCase(string value, bool capitalize, char wordSplitter = '_')
            {
                var result = new StringBuilder(value.Length);

                var words = value.Split(wordSplitter);
                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];
                    if (i > 0 || capitalize)
                    {
                        word = words[i].Capitalize();
                    }

                    result.Append(word);
                }

                return result.ToString();
            }
        }
    }
}
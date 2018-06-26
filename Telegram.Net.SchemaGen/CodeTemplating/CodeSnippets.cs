using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public static class CodeSnippets
    {
        public class StringValue
        {
            public readonly string Value;

            public StringValue(string value)
            {
                Value = value;
            }

            public override string ToString()
            {
                return Value;
            }

            protected bool Equals(StringValue other)
            {
                return string.Equals(Value, other.Value);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;

                return Equals((StringValue)obj);
            }

            public override int GetHashCode()
            {
                return Value != null ? Value.GetHashCode() : 0;
            }
        }

        public class Access : StringValue
        {
            public static Access Private = new Access("private");

            public static Access Public = new Access("public");

            public Access(string value)
                : base(value)
            {
            }

            public static string ToStringIfNot(Access value, Access ifNot)
            {
                if (value == null)
                {
                    return string.Empty;
                }

                if (value.Equals(ifNot))
                {
                    return string.Empty;
                }

                return value.ToString();
            }
        }

        public class ClassProperty
        {
            public string Type;
            public string Name;
            public string DefaultValue;
            public Access Access;

            public bool HasGet;
            public Access GetAccess;
            public bool HasSet;
            public Access SetAccess;

            public bool IsStatic;

            public override string ToString()
            {
                var result = $"{Access.ToString().ToLower()} {(IsStatic ? "static" : "")} {Type} {Name} " +
                             $"{{ {(HasGet ? Access.ToStringIfNot(GetAccess, Access.Public) + " get; " : "")}" +
                             $"{(HasSet ? Access.ToStringIfNot(SetAccess, Access.Public) + " set; " : "")} }}";

                result = Extensions.CompressSpaces(result);

                if (DefaultValue != null)
                {
                    result += $" {DefaultValue}";
                }
                return result + ";";
            }
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
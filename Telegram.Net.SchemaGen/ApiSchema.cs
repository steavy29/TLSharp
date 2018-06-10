using System.Collections.Generic;
using System.Linq;
using Telegram.Net.SchemaGen.CodeTemplating;

namespace Telegram.Net.SchemaGen
{
    public static class ApiSchema
    {
        public static class ApiTypes
        {
            public static bool IsBasicType(string type) => BasicTypes.Contains(type);

            public static bool IsCustomType(string type) => !IsBasicType(type);

            public static HashSet<string> BasicTypes = new HashSet<string>
            {
                "int",
                "long",
                "bool",
                "string"
            };

            private static readonly Dictionary<string, string> typeMappings = new Dictionary<string, string>
            {

            };

            public static string GetCSharpType(string apiType)
            {
                var lowerCase = apiType.ToLower();

                var mappedType = ApplyTypeMapping(lowerCase);
                var isCustomType = IsCustomType(lowerCase);

                var camelCase = CodeSnippets.Naming.CamelCase(mappedType, isCustomType);

                return camelCase;
            }

            private static string ApplyTypeMapping(string value)
            {
                return typeMappings.ContainsKey(value) ? typeMappings[value] : value;
            }
        }

        public static bool IsSkipItemType(string value) => value == "X" || value == "!X";

        public class File
        {
            public List<ConstructorInfo> Constructors;
            public List<MethodInfo> Methods;
        }

        public class ConstructorInfo
        {
            public static HashSet<string> SystemTypes = new HashSet<string>
            {
                "Bool",
                "True",
                "Vector t"
            };

            public string Id;
            public string Predicate;
            public List<ParameterInfo> Params;
            public string Type;

            public bool IsSystemType => SystemTypes.Contains(Type);
        }

        public class MethodInfo
        {
            public string Id;
            public string Method;
            public List<ParameterInfo> Params;
            public string Type;

            public override string ToString()
            {
                var paramsStr = string.Join(", ", Params.Select(p => p.ToString()));
                return $"{Type} {Method}({paramsStr})";
            }
        }

        public class ParameterInfo
        {
            public string Name;
            public string Type;

            public override string ToString()
            {
                return $"{Type} {Name}";
            }
        }
    }
}

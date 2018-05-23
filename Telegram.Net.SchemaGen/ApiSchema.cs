using System.Collections.Generic;

namespace Telegram.Net.SchemaGen
{
    public static class ApiSchema
    {
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
        }

        public class ParameterInfo
        {
            public string Name;
            public string Type;
        }
    }
}

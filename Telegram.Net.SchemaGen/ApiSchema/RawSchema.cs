using System.Collections.Generic;

namespace Telegram.Net.SchemaGen.Parser
{
    public static class RawSchema
    {
        public class File
        {
            public List<ConstructorInfo> constructors;
            public List<MethodInfo> methods;
        }

        public class ConstructorInfo
        {
            public static HashSet<string> SystemTypes = new HashSet<string>
            {
                "Bool",
                "True",
                "Vector t"
            };

            public string id;
            public string predicate;
            public List<ParameterInfo> @params;
            public string type;

            public bool IsSystemType => SystemTypes.Contains(type);
        }

        public class MethodInfo
        {
            public string id;
            public string method;
            public List<ParameterInfo> @params;
            public string type;
        }

        public class ParameterInfo
        {
            public string name;
            public string type;
        }
    }
}

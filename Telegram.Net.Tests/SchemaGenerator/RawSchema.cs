using System.Collections.Generic;

namespace Telegram.Net.Tests.SchemaGenerator
{
    static class RawSchema
    {
        public class File
        {
            public List<ConstructorInfo> constructors;
            public List<MethodInfo> methods;
        }

        public class ConstructorInfo
        {
            public string id;
            public string predicate;
            public List<ParameterInfo> @params;
            public string type;
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
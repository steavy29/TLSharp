using System.Collections.Generic;
using System.Linq;

using Telegram.Net.SchemaGen.Parser;

namespace Telegram.Net.Tests.SchemaGenerator
{
    class TLType
    {
        private readonly RawSchema.ConstructorInfo constructorInfo;

        private static readonly HashSet<string> builtInTypes = new HashSet<string> { "booltrue", "boolfalse", "true", "vector", "null" };
        public bool mapsToBuiltInType => builtInTypes.Contains(typeName.ToLowerInvariant());

        public string id;
        public string baseTypeName;
        public string typeName;
        public string namespaceName;

        public TLType(RawSchema.ConstructorInfo constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            id = constructorInfo.id;
            baseTypeName = constructorInfo.type;
            typeName = constructorInfo.predicate.Split('.').Last().Capitalize();

            var splitted = constructorInfo.type.Split('.');
            namespaceName = splitted.Length == 2 ? splitted.First() : null;
        }

        public CodeGen.Class GetCodeClass()
        {
            var constructorClass = new CodeGen.Class(typeName, false, constructorInfo.type);

            foreach (var parameter in constructorInfo.@params)
            {
                var field = new CodeGen.Field(CodeGen.AccessType.Public(), true, parameter.type, parameter.name);
                constructorClass.fields.Add(field);
            }

            // Deserialize
            var readMethod = new CodeGen.Method(CodeGen.AccessType.Public(), null, "Deserialize");
            readMethod.parameters.Add(new CodeGen.Parameter("BinaryReader", "reader"));
            readMethod.isOverride = true;

            constructorClass.methods.Add(readMethod);

            // Serialize
            var writeMethod = new CodeGen.Method(CodeGen.AccessType.Public(), null, "Serialize");
            writeMethod.parameters.Add(new CodeGen.Parameter("BinaryWriter", "writer"));
            writeMethod.isOverride = true;

            constructorClass.methods.Add(writeMethod);

            return constructorClass;
        }

        public CodeGen.Field AsIdField()
        {
            return new CodeGen.Field(CodeGen.AccessType.Public(), true, "int", typeName, id);
        }
    }

    class TLRequest
    {
        private readonly RawSchema.MethodInfo methodInfo;

        public TLRequest(RawSchema.MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        public string namespaceName => methodInfo.method.Split('.')[0];
        public string requestName => methodInfo.method.Split('.')[1];

        public CodeGen.Class GetClass()
        {
            return null;
        }
    }
}

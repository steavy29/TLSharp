using System.Collections.Generic;
using System.Linq;
using Telegram.Net.SchemaGen;

namespace Telegram.Net.Tests.SchemaGenerator
{
    class TLType
    {
        private readonly ApiSchema.ConstructorInfo constructorInfo;

        private static readonly HashSet<string> builtInTypes = new HashSet<string> { "booltrue", "boolfalse", "true", "vector", "null" };
        public bool mapsToBuiltInType => builtInTypes.Contains(typeName.ToLowerInvariant());

        public string id;
        public string baseTypeName;
        public string typeName;
        public string namespaceName;

        public TLType(ApiSchema.ConstructorInfo constructorInfo)
        {
            this.constructorInfo = constructorInfo;
            id = constructorInfo.Id;
            baseTypeName = constructorInfo.Type;
            typeName = constructorInfo.Predicate.Split('.').Last().Capitalize();

            var splitted = constructorInfo.Type.Split('.');
            namespaceName = splitted.Length == 2 ? splitted.First() : null;
        }

        public CodeGen.Class GetCodeClass()
        {
            var constructorClass = new CodeGen.Class(typeName, false, constructorInfo.Type);

            foreach (var parameter in constructorInfo.Params)
            {
                var field = new CodeGen.Field(CodeGen.AccessType.Public(), true, parameter.Type, parameter.Name);
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
        private readonly ApiSchema.MethodInfo methodInfo;

        public TLRequest(ApiSchema.MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        public string namespaceName => methodInfo.Method.Split('.')[0];
        public string requestName => methodInfo.Method.Split('.')[1];

        public CodeGen.Class GetClass()
        {
            return null;
        }
    }
}

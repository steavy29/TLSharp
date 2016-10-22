using System.Collections.Generic;
using System.Linq;

namespace Telegram.Net.Tests.SchemaGenerator
{
    public class Schema
    {
        public List<ConstructorClass> constructors;
        public List<MethodClass> methods;
    }

    public class ConstructorClass
    {
        public string id;
        public string predicate;
        public List<Parameter> @params;
        public string type;

        private string ClassName => predicate.Split('.').Last();

        private string NamespaceName
        {
            get
            {
                var splitted = type.Split('.');
                return splitted.Length == 2 ? splitted.First() : null;
            }
        }

        public CodeGen.Class Convert()
        {
            var constructorClass = new CodeGen.Class(ClassName, type);

            foreach (var parameter in @params)
            {
                var field = new CodeGen.Field(CodeGen.AccessType.Public(), true, parameter.type, parameter.name);
                constructorClass.fields.Add(field);
            }

            // Read
            var readMethod = new CodeGen.Method(CodeGen.AccessType.Public(), null, "Read");
            readMethod.parameters.Add(new CodeGen.Parameter("BinaryReader", "reader"));
            readMethod.isOverride = true;

            constructorClass.methods.Add(readMethod);

            // Write
            var writeMethod = new CodeGen.Method(CodeGen.AccessType.Public(), null, "Write");
            writeMethod.parameters.Add(new CodeGen.Parameter("BinaryWriter", "writer"));
            writeMethod.isOverride = true;
            
            constructorClass.methods.Add(writeMethod);

            return constructorClass;
        }
    }

    public class MethodClass
    {
        public string id;
        public string method;
        public List<Parameter> @params;
        public string type;

        public string NamespaceName => method.Split('.')[0];
        public string MethodName => method.Split('.')[1];
    }

    public class Parameter
    {
        public string name;
        public string type;
    }
}
using System.Linq;

using Telegram.Net.SchemaGen.Parser;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class ConstructorTypeBuilder
    {
        private readonly CodeTemplate codeTemplate;
        private readonly RawSchema.ConstructorInfo constructorInfo;

        public string Id { get; private set; }

        public string BaseTypeName { get; private set; }
        public string TypeName { get; private set; }
        public string NamespaceName { get; private set; }
        public string ClassName { get; private set; }

        public ConstructorTypeBuilder(CodeTemplate codeTemplate, RawSchema.ConstructorInfo constructorInfo)
        {
            this.codeTemplate = codeTemplate;
            this.constructorInfo = constructorInfo;
        }

        public void Build()
        {
            Id = constructorInfo.id;
            BaseTypeName = constructorInfo.type;
            TypeName = constructorInfo.predicate.Split('.').Last().Capitalize();

            var typeFieldSplitted = constructorInfo.type.Split('.');
            NamespaceName = typeFieldSplitted.Length == 2 ? typeFieldSplitted.First() : null;

            ClassName = constructorInfo.predicate;

            codeTemplate.Replace("ParamFieldsCursor", ClassName);
        }
    }
}
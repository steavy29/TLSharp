using System.Linq;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class ConstructorTypeBuilder : CodeBuilder
    {
        private readonly ApiSchema.ConstructorInfo constructorInfo;

        public string Id { get; private set; }

        public string BaseTypeName { get; private set; }
        public string TypeName { get; private set; }
        public string NamespaceName { get; private set; }
        public string ClassName { get; private set; }

        public ConstructorTypeBuilder(CodeTemplateInstance codeTemplate, ApiSchema.ConstructorInfo constructorInfo)
            : base(codeTemplate)
        {
            this.constructorInfo = constructorInfo;
        }

        public override void Build()
        {
            Id = constructorInfo.Id;
            BaseTypeName = constructorInfo.Type;
            TypeName = constructorInfo.Predicate.Split('.').Last().Capitalize();

            var typeFieldSplitted = constructorInfo.Type.Split('.');
            NamespaceName = typeFieldSplitted.Length == 2 ? typeFieldSplitted.First() : null;

            ClassName = constructorInfo.Predicate;

            CodeTemplate.Replace("ParamFieldsCursor", ClassName);
        }
    }
}
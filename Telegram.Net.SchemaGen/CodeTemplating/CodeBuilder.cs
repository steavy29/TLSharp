namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public abstract class CodeBuilder
    {
        protected readonly CodeTemplateInstance CodeTemplate;

        protected CodeBuilder(CodeTemplateInstance codeTemplate)
        {
            CodeTemplate = codeTemplate;
        }

        public abstract void Build();
    }
}
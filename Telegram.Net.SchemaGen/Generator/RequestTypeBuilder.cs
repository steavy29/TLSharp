namespace Telegram.Net.SchemaGen.Generator
{
    public class RequestTypeBuilder
    {
        private TextTemplate textTemplate;

        public string RequestName { get; set; }

        public RequestTypeBuilder(TextTemplate textTemplate)
        {
            this.textTemplate = textTemplate;
        }

        public void Build()
        {
            textTemplate.Apply("ParamFieldsCursor", RequestName);
        }
    }
}
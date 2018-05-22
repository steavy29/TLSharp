namespace Telegram.Net.SchemaGen.Generator
{
    public class InlineCursor
    {
        public string Name { get; }

        public string Value { get; set; }

        public InlineCursor(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}

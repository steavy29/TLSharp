using System.IO;
using System.Linq;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class CodeTemplateSchema
    {
        public readonly string[] Lines;

        public CodeTemplateSchema(string[] lines)
        {
            Lines = lines;
        }

        public static CodeTemplateSchema ReadFromFile(string path)
        {
            var templateLines = File.ReadAllLines(path);
            return new CodeTemplateSchema(templateLines);
        }

        public CodeTemplateInstance CreateInstance()
        {
            return new CodeTemplateInstance(Lines.ToList());
        }
    }
}
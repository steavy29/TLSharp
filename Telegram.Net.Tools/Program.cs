using Telegram.Net.SchemaGen;

namespace Telegram.Net.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateApiSchema();
        }

        public static void GenerateApiSchema()
        {
            ApiCodeGenerator.Run();
        }
    }
}

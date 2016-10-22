using System.IO;
using Newtonsoft.Json;

namespace Telegram.Net.Tests.SchemaGenerator
{
    public class Generator
    {
        public static void GenerateSchema()
        {
            var content = File.ReadAllLines("schema.json");
            var schemaLine = content[0];

            var schema = JsonConvert.DeserializeObject<Schema>(schemaLine);
            
            Directory.Delete("src", true);

            Directory.CreateDirectory("src");
            Directory.CreateDirectory("src/Constructors");
            foreach (var constructorSchema in schema.constructors)
            {
                using (var srcFileStream = File.CreateText($"src/Constructors/{constructorSchema.type}.cs"))
                {
                    var srcFile = new CodeGen.SourceFile();
                    srcFile.classes.Add(constructorSchema.Convert());

                    srcFile.Serialize(srcFileStream);
                }
            }

            Directory.CreateDirectory("src/Methods");
            foreach (var method in schema.methods)
            {
                using (var srcFile = File.CreateText($"src/Methods/{method.method}.cs"))
                {

                }
            }
        }
    }
}

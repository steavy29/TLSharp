using System.IO;

using Newtonsoft.Json;

using Telegram.Net.SchemaGen.Parser;

namespace Telegram.Net.SchemaGen
{
    class ApiCodeGenerator
    {
        public static void Run()
        {
            var content = File.ReadAllLines("schema.json");
            var schemaLine = content[0];

            var schema = JsonConvert.DeserializeObject<RawSchema.File>(schemaLine);

            if (Directory.Exists("src"))
                Directory.Delete("src", true);

            Directory.CreateDirectory("src");
            Directory.CreateDirectory("src/Constructors");

            foreach (var constructorInfo in schema.constructors)
            {
                var tlType = new TLType(constructorInfo);

                if (tlType.mapsToBuiltInType)
                {
                    if (tlType.typeName == "true") // redundand class
                        continue;

                    constantsClass.fields.Add(tlType.AsIdField());
                    continue;
                }

                using (var srcFileStream = File.CreateText($"src/Constructors/{tlType.typeName}.cs"))
                {
                    var srcFile = new CodeGen.SourceFile();
                    srcFile.classes.Add(tlType.GetCodeClass());

                    srcFile.Write(srcFileStream);
                }
            }

            using (var srcFileStream = File.CreateText($"src/{constantsClass.name}.cs"))
            {
                var srcFile = new CodeGen.SourceFile();
                srcFile.classes.Add(constantsClass);

                srcFile.Write(srcFileStream);
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

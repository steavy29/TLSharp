using System.IO;

using Newtonsoft.Json;

using Telegram.Net.SchemaGen.CodeTemplating;

namespace Telegram.Net.SchemaGen
{
    public class ApiCodeGenerator
    {
        private static readonly string ConstructorTemplateFileName = "ConstructorTemplate.tpt";
        private static readonly string RequestTemplateFileName = "RequestTemplate.tpt";

        public static void Run()
        {
            var content = File.ReadAllText("schema.json");

            var schema = JsonConvert.DeserializeObject<ApiSchema.File>(content);

            if (Directory.Exists("src"))
                Directory.Delete("src", true);

            Directory.CreateDirectory("src");
            Directory.CreateDirectory("src/Constructors");
            Directory.CreateDirectory("src/Methods");

            var constructorTemplate = File.ReadAllLines(ConstructorTemplateFileName);
            var requestTemplate = File.ReadAllLines(RequestTemplateFileName);

            foreach (var constructorInfo in schema.Constructors)
            {
                if (constructorInfo.IsSystemType)
                {
                    continue;
                }

                var template = new CodeTemplate(constructorTemplate);
                var constructorTypeBuilder = new ConstructorTypeBuilder(template, constructorInfo);

                constructorTypeBuilder.Build();

                var generatedCode = template.ToString();
                using (var srcFileStream = File.CreateText($"src/Constructors/{constructorTypeBuilder.ClassName}.cs"))
                {
                    srcFileStream.Write(generatedCode);
                }
            }

            /*using (var srcFileStream = File.CreateText($"src/{constantsClass.name}.cs"))
            {
                var srcFile = new CodeGen.SourceFile();
                srcFile.classes.Add(constantsClass);

                srcFile.Write(srcFileStream);
            }*/

            foreach (var methodInfo in schema.Methods)
            {
                var template = new CodeTemplate(requestTemplate);
                var requestTypeBuilder = new RequestTypeBuilder(template, methodInfo);

                requestTypeBuilder.Build();

                var generatedCode = template.ToString();
                using (var srcFile = File.CreateText($"src/Methods/{requestTypeBuilder.RequestName}.cs"))
                {
                    srcFile.Write(generatedCode);
                }
            }
        }
    }
}

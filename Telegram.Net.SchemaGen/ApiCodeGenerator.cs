using System;
using System.Diagnostics;
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

            CleanUpDirectory("src");

            Directory.CreateDirectory("src");
            Directory.CreateDirectory("src/Constructors");
            Directory.CreateDirectory("src/Methods");

            var requestTemplate = File.ReadAllLines(RequestTemplateFileName);
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

            var constructorTemplate = File.ReadAllLines(ConstructorTemplateFileName);
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
        }

        private static void CleanUpDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            var codeFilesPaths = Directory.GetFiles(directoryPath, "*.cs");
            foreach (var codeFilePath in codeFilesPaths)
            {
                try
                {
                    File.Delete(codeFilePath);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Failed to remove file: {codeFilePath}, Exception: {e}");
                }
            }
        }
    }
}

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
        private static readonly string TelegramProtocolCodesTemplateFileName = "TelegramProtocolCodesTemplate.tpt";

        public static void Run()
        {
            var content = File.ReadAllText("schema.json");

            var schema = JsonConvert.DeserializeObject<ApiSchema.File>(content);

            CleanUpDirectory("src");

            Directory.CreateDirectory("src");
            Directory.CreateDirectory("src/Constructors");
            Directory.CreateDirectory("src/Methods");

            var requestTemplateSchema = CodeTemplateSchema.ReadFromFile(RequestTemplateFileName);
            foreach (var methodInfo in schema.Methods)
            {
                var template = requestTemplateSchema.CreateInstance();
                var requestTypeBuilder = new RequestTypeBuilder(template, methodInfo);

                requestTypeBuilder.Build();

                var generatedCode = template.ToString();
                using (var srcFile = File.CreateText($"src/Methods/{requestTypeBuilder.RequestName}.cs"))
                {
                    srcFile.Write(generatedCode);
                }
            }

            var tlProtocolCodesRegistryTemplate = CodeTemplateSchema.ReadFromFile(TelegramProtocolCodesTemplateFileName).CreateInstance();
            var tlProtocolCodesRegistryBuilder = new TelegramProtocolCodesBuilder(tlProtocolCodesRegistryTemplate);

            var constructorTemplateSchema = CodeTemplateSchema.ReadFromFile(ConstructorTemplateFileName);
            foreach (var constructorInfo in schema.Constructors)
            {
                if (constructorInfo.IsSystemType)
                {
                    continue;
                }

                var template = constructorTemplateSchema.CreateInstance();
                var constructorTypeBuilder = new ConstructorTypeBuilder(template, constructorInfo);

                constructorTypeBuilder.Build();

                tlProtocolCodesRegistryBuilder.RegisterType(constructorTypeBuilder.Id, constructorTypeBuilder.TypeName);

                var generatedCode = template.ToString();
                using (var srcFileStream = File.CreateText($"src/Constructors/{constructorTypeBuilder.ClassName}.cs"))
                {
                    srcFileStream.Write(generatedCode);
                }
            }

            using (var srcFileStream = File.CreateText("src/TelegramApi.Codes.cs"))
            {
                var generatedCode = tlProtocolCodesRegistryTemplate.ToString();
                srcFileStream.Write(generatedCode);
            }
        }

        private static void CleanUpDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            var codeFilesPaths = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
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

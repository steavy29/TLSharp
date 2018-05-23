using Microsoft.VisualStudio.TestTools.UnitTesting;

using Telegram.Net.SchemaGen;

namespace Telegram.Net.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void GenerateSchema()
        {
            ApiCodeGenerator.Run();
        }
    }
}

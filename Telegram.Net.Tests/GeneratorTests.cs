using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Net.Tests.SchemaGenerator;

namespace Telegram.Net.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void GenerateSchema()
        {
            Generator.GenerateSchema();
        }
    }
}

using System;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Wave;
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

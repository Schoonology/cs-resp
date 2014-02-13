using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resp;

namespace Resp.Tests
{
    [TestClass]
    public class RespReaderTests
    {
        [TestMethod]
        public void TestReadFromString()
        {
            RespReader reader = new RespReader("+OK\r\n");
            Assert.AreEqual(
                reader.Read(),
                "OK"
            );
        }

        [TestMethod]
        public void TestReadFromStream()
        {
            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes("+OK\r\n"));
            RespReader reader = new RespReader(stream);
        }
    }
}

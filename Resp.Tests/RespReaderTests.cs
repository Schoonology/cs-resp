using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
            Assert.AreEqual(
                reader.Read(),
                "OK"
            );
        }

        [TestMethod]
        public void TestReadFromStringAsync()
        {
            RespReader reader = new RespReader("+OK\r\n");
            Task<object> task = reader.ReadAsync();
            task.Wait();
            Assert.AreEqual(
                task.Result,
                "OK"
            );
        }

        [TestMethod]
        public void TestReadFromStreamAsync()
        {
            Stream stream = new MemoryStream(Encoding.ASCII.GetBytes("+OK\r\n"));
            RespReader reader = new RespReader(stream);
            Task<object> task = reader.ReadAsync();
            task.Wait();
            Assert.AreEqual(
                task.Result,
                "OK"
            );
        }
    }
}

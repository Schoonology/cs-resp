using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resp;

namespace Resp.Tests
{
    [TestClass]
    public class RespSerializerTests
    {
        RespSerializer serializer = new RespSerializer();

        [TestMethod]
        public void TestSerializeString()
        {
            Assert.AreEqual(
                this.serializer.Serialize("foobar"),
                "$6\r\nfoobar\r\n"
            );
        }

        [TestMethod]
        public void TestSerializeInteger()
        {
            Assert.AreEqual(
                this.serializer.Serialize(42),
                ":42\r\n"
            );
        }

        [TestMethod]
        public void TestSerializeBoolean()
        {
            Assert.AreEqual(
                this.serializer.Serialize(true),
                ":1\r\n"
            );
        }

        [TestMethod]
        public void TestSerializeError()
        {
            Assert.AreEqual(
                this.serializer.Serialize(new Resp.Error("baderr", "some message")),
                "-BADERR some message\r\n"
            );
        }

        [TestMethod]
        public void TestSerializeArray()
        {
            object[] arr = { 1, 2, "foo", "bar", 5, false };

            Assert.AreEqual(
                this.serializer.Serialize(arr),
                "*6\r\n:1\r\n:2\r\n$3\r\nfoo\r\n$3\r\nbar\r\n:5\r\n:0\r\n"
            );
        }
    }
}

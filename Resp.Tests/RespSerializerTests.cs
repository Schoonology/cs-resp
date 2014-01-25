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

        [TestMethod]
        public void TestDeserializeSimpleString()
        {
            Assert.AreEqual(
                this.serializer.Deserialize("+OK"),
                "OK"
            );
        }

        [TestMethod]
        public void TestDeserializeBulkString()
        {
            Assert.AreEqual(
                this.serializer.Deserialize("$6\r\nfoobar\r\n"),
                "foobar"
            );
        }

        [TestMethod]
        public void TestDeserializeBulkStringNull()
        {
            Assert.AreEqual(
                this.serializer.Deserialize("$-1\r\n"),
                null
            );
        }

        [TestMethod]
        public void TestDeserializeError()
        {
            var obj = this.serializer.Deserialize("-BADERR some message");

            Assert.IsTrue(obj is Error);
            Error err = (Error)obj;

            Assert.AreEqual(err.type, "BADERR");
            Assert.AreEqual(err.message, "some message");
        }

        [TestMethod]
        public void TestDeserializeInteger()
        {
            Assert.AreEqual(
                this.serializer.Deserialize(":42"),
                42
            );
        }

        [TestMethod]
        public void TestDeserializeArray()
        {
            object[] expected = { 1, 2, "foo", "bar", 5, 0 };
            object obj = this.serializer.Deserialize("*6\r\n:1\r\n:2\r\n$3\r\nfoo\r\n$3\r\nbar\r\n:5\r\n:0\r\n");

            Assert.IsTrue(obj is object[]);

            object[] arr = (object[])obj;

            Assert.AreEqual(arr.Length, expected.Length);

            for (int i = 0; i < arr.Length; i++) {
                Assert.AreEqual(arr[i], expected[i]);
            }
        }

        [TestMethod]
        public void TestDeserializeArrayNull()
        {
            Assert.AreEqual(
                this.serializer.Deserialize("*-1\r\n"),
                null
            );
        }

        [TestMethod]
        public void TestDeserializeBadStart()
        {
            Assert.AreEqual(
                this.serializer.Deserialize("!!!:42\r\n"),
                42
            );
        }
    }
}


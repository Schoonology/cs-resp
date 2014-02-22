using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Resp;

namespace Resp.Tests
{
    [TestClass]
    class RespClientTests
    {
        [TestMethod]
        public void TestCreation()
        {
            RespClient client = new RespClient();
        }
    }
}

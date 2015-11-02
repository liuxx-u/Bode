using System;
using OSharp.Utility.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OSharp.Utility.Tests.Demo
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var time = DateTime.Now;
            var tesult= RetryHelper.Retry(() =>
            {
                var a = time;
                return false;
            }, 2);
        }
    }
}

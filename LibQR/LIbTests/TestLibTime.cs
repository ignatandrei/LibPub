using LibTimeCreator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LIbTests
{
    [TestClass]
    public class TestLibTime
    {
        [TestMethod]
        public void TestEncrypt()
        {
            var test = new LibTimeInfo();
            test.Info = "this is now";
            test.MaxDate = DateTime.Now.AddDays(1);
            var test1 = LibTimeInfo.FromString(test.Generate());
            Assert.AreEqual(test.MaxDate.ToString("yyyyMMddHHmmss"), test1.MaxDate.ToString("yyyyMMddHHmmss"));
            Assert.AreEqual(test.Info, test1.Info);

        }
        [TestMethod]
        public void TestValid()
        {
            var test = new LibTimeInfo(10);
            SystemTime.Now = () => DateTime.Now.ToUniversalTime().AddMinutes(1);
            Assert.IsTrue(test.IsValid());

            SystemTime.Now = () => DateTime.Now.ToUniversalTime().AddMinutes(5);
            Assert.IsTrue(test.IsValid());
            SystemTime.Now = () => DateTime.Now.ToUniversalTime().AddMinutes(15);
            Assert.IsFalse(test.IsValid());
        }

    }
}

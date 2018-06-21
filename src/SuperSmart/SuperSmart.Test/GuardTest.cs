using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using System;

namespace SuperSmart.Test
{
    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        public void GuardNotNullObjectThrowPropertyExceptionCollection()
        {
            object element = null;

            try
            {
                Guard.NotNull(element);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GuardNotNullOrEmptyStringThrowPropertyExceptionCollection()
        {
            var element = string.Empty;

            try
            {
                Guard.NotNullOrEmpty(element);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GuardNotNullOrEmptyStringNullThrowPropertyExceptionCollection()
        {
            string element = null;

            try
            {
                Guard.NotNullOrEmpty(element);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }
    }
}

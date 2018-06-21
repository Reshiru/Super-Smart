using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Extension;
using System;

namespace SuperSmart.Test
{
    [TestClass]
    public class ExtensionTest
    {
        [TestMethod]
        public void RandomStringTestLengthSucceed()
        {
            var stringLength = 10;

            try
            {
                var result = IntExtension.RandomString(stringLength);

                Assert.IsTrue(result.Length == stringLength);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void HashStringTestSucceed()
        {
            var toHashValue = "test";
            var salt = "salt";

            try
            {
                 StringExtension.GenerateHash(toHashValue, salt);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }
    }
}

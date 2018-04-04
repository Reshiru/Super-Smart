using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;

namespace SuperSmart.Test
{
    [TestClass]
    public class VerificationTest
    {
        public VerificationTest()
        {
            verificationPersistence = new VerificationPersistence();
        }

        IVerificationPersistence verificationPersistence;

        [TestMethod]
        public void LoginEmptyTest()
        {
            try
            {
                verificationPersistence.Login(string.Empty, string.Empty);
                Assert.IsTrue(false);
            }
            catch 
            {
                Assert.IsTrue(true);
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;

namespace SuperSmart.Test
{
    [TestClass]
    public class VerificationTest
    {
        IVerificationPersistence verificationPersistence = new VerificationPersistence();

        [TestMethod]
        public void RegisterNullTest()
        {
            try
            {
                verificationPersistence.Register(null);
            }
            catch (Exception ex)
            {
                if(ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void RegisterNewDataTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = "test@user.com",
                    FirstName = "Test",
                    LastName = "User",
                    Password = "TestPass"
                });
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
        [TestMethod]
        public void RegisterInvalidPasswordTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = "test@user.com",
                    FirstName = "Test",
                    LastName = "User",
                    Password = "Short"
                });
            }
            catch (Exception ex)
            {
                if (ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void RegisterExistingDataTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = "u1@user.com",
                    FirstName = "Test",
                    LastName = "User",
                    Password = "TestPass"
                });
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = "u1@user.com",
                    FirstName = "Test",
                    LastName = "User",
                    Password = "TestPass"
                });
            }
            catch (Exception ex)
            {
                if(ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void LoginNullTest()
        {
            try
            {
                verificationPersistence.Login(null);
            }
            catch (Exception ex)
            {
                if (ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void LoginInvalidDataTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            try
            {
                verificationPersistence.Login(new LoginViewModel()
                {
                    Email = "test@user.com",
                    Password = "TestPass"
                });
            }
            catch (Exception ex)
            {
                if (ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void LoginValidDataTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "test@user.com",
                FirstName = "Test",
                LastName = "User",
                Password = "TestPass"
            });
            try
            {
                verificationPersistence.Login(new LoginViewModel()
                {
                    Email = "test@user.com",
                    Password = "TestPass"
                });
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}

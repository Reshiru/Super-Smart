using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
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

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void RegisterNewDataTest()
        {
            var email = "test@user.com";
            var firstname = "Test";
            var lastname = "User";
            var password = "TestPass";

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                    Password = password,
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
            var email = "test@user.com";
            var firstname = "Test";
            var lastname = "User";
            var password = "Short";

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                    Password = password
                });

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }

        }

        [TestMethod]
        public void RegisterExistingDataTest()
        {
            var email = "u1@user.com";
            var firstname = "Test";
            var lastname = "User";

            var account = new AccountBuilder().WithEmail(email)
                                              .WithFirstname(firstname)
                                              .WithLastname(lastname)
                                              .Build();

            new DatabaseBuilder().WithAccount(account)
                                 .WithSecureDatabaseDeleted(true)
                                 .Build();
            
            try
            {
                verificationPersistence.Register(new RegisterViewModel()
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                });

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void LoginNullTest()
        {
            try
            {
                verificationPersistence.Login(null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void LoginInvalidDataTest()
        {
            var email = "test@user.com";
            var password = "TestPass";

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();
            
            try
            {
                verificationPersistence.Login(new LoginViewModel()
                {
                    Email = email,
                    Password = password
                });

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void LoginValidDataTest()
        {
            var email = "test@user.com";
            var password = "TestPass";

            var account = new AccountBuilder().WithEmail(email)
                                              .WithPassword(password)
                                              .Build();

            new DatabaseBuilder().WithAccount(account)
                                 .WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                verificationPersistence.Login(new LoginViewModel()
                {
                    Email = email,
                    Password = password
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
using System;

namespace SuperSmart.Test
{
    [TestClass]
    public class UserTests
    {
        IVerificationPersistence verificationPersistence = new VerificationPersistence();
        IUserPersistence userPersistence = new UserPersistence();

        [TestMethod]
        public void GetUserByLoginTokenValidTest()
        {
            var email = "test@test.test";
            var firstname = "User";
            var lastname = "Test";

            var account = new AccountBuilder().WithEmail(email)
                                              .WithFirstname(firstname)
                                              .WithLastname(lastname)
                                              .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .Build();

            try
            {
                var user = userPersistence.GetUserByLoginToken(account.LoginToken);

                if (user == null || user.Email != email || user.Firstname != firstname ||
                    user.Lastname != lastname)
                {
                    Assert.IsTrue(false);
                }

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetUserByLoginTokenNullTest()
        {
            try
            {
                userPersistence.GetUserByLoginToken(null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetFullNameFromValidTokenTest()
        {
            var firstname = "User";
            var lastname = "Test";
            var email = "test@test.test";

            var account = new AccountBuilder().WithEmail(email)
                                  .WithFirstname(firstname)
                                  .WithLastname(lastname)
                                  .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .Build();

            try
            {
                var user = userPersistence.GetFullNameFromUser(account.LoginToken);

                if (user == null || user.Firstname != firstname && user.Lastname != lastname)
                {
                    Assert.IsTrue(false);
                }

            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetFullNameFromTokenNullTest()
        {
            try
            {
                var user = userPersistence.GetFullNameFromUser(null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetFullNameFromEmptyTokenString()
        {
            try
            {
                var user = userPersistence.GetFullNameFromUser(string.Empty);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }
    }
}

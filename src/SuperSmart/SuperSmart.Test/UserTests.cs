using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;
using System.Linq;

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
            DatabaseHelper.SecureDeleteDatabase();

            //Register admin user
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "test@test.test",
                FirstName = "User",
                LastName = "Test",
                Password = "12345678"
            });

            var loginToken = verificationPersistence.Login(new LoginViewModel()
            {
                Email = "test@test.test",
                Password = "12345678"
            });

            try
            {
                UserViewModel user = userPersistence.GetUserByLoginToken(loginToken);

                if (user == null)
                {
                    Assert.IsTrue(false);
                    return;
                }
                if (user.Email != "test@test.test")
                {
                    Assert.IsTrue(false);
                    return;
                }
                if (user.Firstname != "User")
                {
                    Assert.IsTrue(false);
                    return;
                }
                if (user.Lastname != "Test")
                {
                    Assert.IsTrue(false);
                    return;
                }

            }
            catch (Exception ex)
            {
                if (ex is PropertyExceptionCollection)
                {
                    Assert.IsTrue(false);
                    return;
                }
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetUserByLoginTokenNullTest()
        {
            try
            {
                userPersistence.GetUserByLoginToken(null);
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

    }
}

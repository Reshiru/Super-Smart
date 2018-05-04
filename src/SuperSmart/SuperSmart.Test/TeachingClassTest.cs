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
    public class TeachingClassTest
    {
        IVerificationPersistence verificationPersistence = new VerificationPersistence();
        ITeachingClassPersistence teachingClassPersistence = new TeachingClassPersistence();
        
        [TestMethod]
        public void CreateNullStringEmptyTest()
        {
            try
            {
                teachingClassPersistence.Create(null, string.Empty);
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
        public void CreateNullNullTest()
        {
            try
            {
                teachingClassPersistence.Create(null, null);
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
        public void CreateValidTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "test@test.test",
                FirstName = "Create",
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
                teachingClassPersistence.Create(new CreateTeachingClassViewModel()
                {
                    Designation = "Test",
                    NumberOfEducationYears = 4,
                    Started = DateTime.Now
                }, loginToken);
            }
            catch 
            {
                Assert.IsTrue(false);
                return;
            }
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void CreateInvalidTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "test@test.test",
                FirstName = "Create",
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
                teachingClassPersistence.Create(new CreateTeachingClassViewModel()
                {
                    Designation = string.Empty,
                    NumberOfEducationYears = 0,
                    Started = DateTime.Now
                }, loginToken);
            }
            catch (Exception ex)
            {
                if (ex is PropertyExceptionCollection && (ex as PropertyExceptionCollection)?.Exceptions?.Count == 2)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void JoinClassEmptyTest()
        {
            try
            {
                teachingClassPersistence.Join(string.Empty, string.Empty);
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
        public void JoinClassNullStringEmptyTest()
        {
            try
            {
                teachingClassPersistence.Join(null, string.Empty);
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
        public void JoinClassStringEmptyNullTest()
        {
            try
            {
                teachingClassPersistence.Join(string.Empty, null);
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
        public void JoinClassValidTest()
        {
            DatabaseHelper.SecureDeleteDatabase();
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "test@test.test",
                FirstName = "Create",
                LastName = "Test",
                Password = "12345678"
            });
            var loginToken = verificationPersistence.Login(new LoginViewModel()
            {
                Email = "test@test.test",
                Password = "12345678"
            });
            teachingClassPersistence.Create(new CreateTeachingClassViewModel()
            {
                Designation = "Test",
                NumberOfEducationYears = 4,
                Started = DateTime.Now
            }, loginToken);
            var referral = string.Empty;
            using (var db = new SuperSmartDb())
            {
                referral = db.TeachingClasses.First().Referral;
            }
            try
            {
                teachingClassPersistence.Join(referral, loginToken);
            }
            catch
            {
                Assert.IsTrue(false);
                return;
            }
            Assert.IsTrue(true);
        }
    }
}

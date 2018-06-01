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
        IUserPersistence userPersistence = new UserPersistence();

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

        [TestMethod]
        public void RemoveUserFromClassEmptyNullTest()
        {
            try
            {
                teachingClassPersistence.RemoveUser(null, null);
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
        public void RemoveUserFromClassValidTest()
        {
            DatabaseHelper.SecureDeleteDatabase();

            //Register admin user
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "class.admin@test.test",
                FirstName = "Admin",
                LastName = "Test",
                Password = "12345678"
            });

            var loginTokenAdmin = verificationPersistence.Login(new LoginViewModel()
            {
                Email = "class.admin@test.test",
                Password = "12345678"
            });

            teachingClassPersistence.Create(new CreateTeachingClassViewModel()
            {
                Designation = "Test",
                NumberOfEducationYears = 4,
                Started = DateTime.Now
            }, loginTokenAdmin);

            //Register user to remove
            verificationPersistence.Register(new RegisterViewModel()
            {
                Email = "class.remove@test.test",
                FirstName = "Remove",
                LastName = "Test",
                Password = "12345678"
            });

            var loginTokenUserToRemove = verificationPersistence.Login(new LoginViewModel()
            {
                Email = "class.remove@test.test",
                Password = "12345678"
            });

            var referral = string.Empty;
            long classId;
            UserViewModel user;
            using (var db = new SuperSmartDb())
            {
                referral = db.TeachingClasses.First().Referral;
                classId = db.TeachingClasses.First().Id;
                user = userPersistence.GetUserByLoginToken(loginTokenUserToRemove);
            }

            teachingClassPersistence.Join(referral, loginTokenUserToRemove);

            try
            {

                teachingClassPersistence.RemoveUser(new RemoveUserFromTeachingClassViewModel()
                {
                    TeachingClassId = classId,
                    UserId = user.Id
                }, loginTokenAdmin);
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

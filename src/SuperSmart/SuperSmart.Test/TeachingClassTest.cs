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
    public class TeachingClassTest
    {
        ITeachingClassPersistence teachingClassPersistence = new TeachingClassPersistence();

        [TestMethod]
        public void CreateNullStringEmptyTest()
        {
            try
            {
                teachingClassPersistence.Create(null, string.Empty);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void CreateNullNullTest()
        {
            try
            {
                teachingClassPersistence.Create(null, null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void CreateValidTest()
        {
            var designation = "Test teaching class";
            var numberOfEducationYears = 4;
            var started = DateTime.Now;

            var account = new AccountBuilder().Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .Build();

            try
            {
                teachingClassPersistence.Create(new CreateTeachingClassViewModel()
                {
                    Designation = designation,
                    NumberOfEducationYears = numberOfEducationYears,
                    Started = started,
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void CreateInvalidTest()
        {
            var account = new AccountBuilder().Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .Build();

            var designation = string.Empty;
            var numberOfEducationYears = 4;
            var started = DateTime.Now;

            try
            {
                teachingClassPersistence.Create(new CreateTeachingClassViewModel()
                {
                    Designation = designation,
                    NumberOfEducationYears = numberOfEducationYears,
                    Started = started,
                }, account.LoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void JoinClassEmptyTest()
        {
            var referral = string.Empty;
            string loginToken = string.Empty;

            try
            {
                teachingClassPersistence.Join(referral, loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void JoinClassNullStringEmptyTest()
        {
            string referral = null;
            var loginToken = string.Empty;

            try
            {
                teachingClassPersistence.Join(referral, loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void JoinClassStringEmptyNullTest()
        {
            try
            {
                teachingClassPersistence.Join(string.Empty, null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void JoinClassValidTest()
        {
            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();

            try
            {
                teachingClassPersistence.Join(teachingClass.Referral, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void RemoveUserFromClassEmptyNullTest()
        {
            RemoveUserFromTeachingClassViewModel removeUserFromTeachingClassViewModel = null;
            string loginToken = null;

            try
            {
                teachingClassPersistence.RemoveUser(removeUserFromTeachingClassViewModel, loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void RemoveUserFromClassValidTest()
        {
            var account = new AccountBuilder().Build();
            var userAccount = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .WithAssignedAccount(userAccount)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithAccount(userAccount)
                                 .WithTeachingClass(teachingClass)
                                 .Build();

            try
            {

                teachingClassPersistence.RemoveUser(new RemoveUserFromTeachingClassViewModel()
                {
                    TeachingClassId = teachingClass.Id,
                    UserId = userAccount.Id
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }
    }
}
﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
using System;
using System.Linq;

namespace SuperSmart.Test
{
    [TestClass]
    public class SubjectTest
    {
        ISubjectPersistence subjectPersistence = new SubjectPersistence();

        [TestMethod]
        public void AddNullSubjectThrowPropertyExceptionCollection()
        {
            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                subjectPersistence.Create(null, null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void AddNullLoginTokenSubjectThrowPropertyExceptionCollection()
        {
            var designation = "Test";
            var teachingClassId = 1;

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = designation,
                    TeachingClassId = teachingClassId,
                }, null);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void CreateTeachingClassSucceed()
        {
            var designation = "Test";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();
            try
            {
                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = designation,
                    TeachingClassId = teachingClass.Id,
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void CreateTeachingClassInvalidLoginTokenThrowPropertyExcpetionCollection()
        {
            var designation = "Test";
            var invalidLoginToken = "loginToken";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();
            try
            {
                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = designation,
                    TeachingClassId = teachingClass.Id,
                }, invalidLoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void CreateTeachingClassInvalidTeachingClassIdThrowPropertyExcpetionCollection()
        {
            var designation = "Test";
            var invalidTeachingClassId = 1;

            var account = new AccountBuilder().Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .Build();

            try
            {
                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = designation,
                    TeachingClassId = invalidTeachingClassId,
                }, account.LoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetSubjectOverviewSuccessfullWithTwoSubjects()
        {
            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var secondSubject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithSubject(secondSubject)
                                 .Build();

            try
            {
                var result = subjectPersistence.GetOverview(account.LoginToken, teachingClass.Id);

                Assert.IsTrue(result.Subjects.Count == 2);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void GetSubjectOverviewInvalidLoginTokenThrowPropertyExcpetionCollection()
        {
            var invalidLoginToke = "loginToken";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();

            try
            {
                var result = subjectPersistence.GetOverview(invalidLoginToke, teachingClass.Id);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetSubjectOverviewInvalidSubjectIdReturnsEmptyList()
        {
            var invalidSubjectId = 1;

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();

            try
            {
                var result = subjectPersistence.GetOverview(account.LoginToken, invalidSubjectId);

                Assert.IsTrue(result.Subjects.Count == 0);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void ManageSubjectSuccessfull()
        {
            var designation = "Test";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .Build();
            try
            {
                subjectPersistence.Manage(new ManageSubjectViewModel()
                {
                    Designation = designation,
                    Id = subject.Id,
                }, account.LoginToken);

                using (SuperSmartDb db = new SuperSmartDb())
                {
                    Assert.IsTrue(db.Subjects.SingleOrDefault(itm => itm.Id == subject.Id)?.Designation == designation);
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void ManageSubjectInvalidSubjectIdThrowsPropertyExcpetionCollection()
        {
            var designation = "Test";
            var invalidSubjectId = 1;

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();
            try
            {
                subjectPersistence.Manage(new ManageSubjectViewModel()
                {
                    Designation = designation,
                    Id = invalidSubjectId,
                }, account.LoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void ManageSubjectInvalidLoginTokenThrowsPropertyExcpetionCollection()
        {
            var designation = "Test";
            var subjectId = 1;
            var invalidLoginToken = "loginToken";

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();
            try
            {
                subjectPersistence.Manage(new ManageSubjectViewModel()
                {
                    Designation = designation,
                    Id = subjectId,
                }, invalidLoginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }
    }
}
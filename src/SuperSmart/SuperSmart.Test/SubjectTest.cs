using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;
using System.Collections.Generic;
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
            DatabaseHelper.SecureDeleteDatabase();

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
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = "Test",
                    TeachingClassId = 1,
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
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                var token = "isvalidtoken";
                var teachingClassId = 1;
                using (var db = new SuperSmartDb())
                {
                    var acc = new Account()
                    {
                        Created = DateTime.Now,
                        Email = "test@test.test",
                        LoginToken = token
                    };

                    db.Accounts.Add(acc);

                    var teachingClass = new TeachingClass()
                    {
                        Admin = acc,
                        Started = DateTime.Now,
                        Referral = "reff",
                    };

                    acc.AssignedClasses.Add(teachingClass);
                    db.TeachingClasses.Add(teachingClass);

                    db.SaveChanges();
                }

                subjectPersistence.Create(new CreateSubjectViewModel()
                {
                    Designation = "Test",
                    TeachingClassId = teachingClassId,
                }, token);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void GetSubjectOverview()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                var token = "isvalidtoken";
                var teachingClassId = 1;
                using (var db = new SuperSmartDb())
                {
                    var acc = new Account()
                    {
                        Created = DateTime.Now,
                        Email = "test@test.test",
                        LoginToken = token
                    };

                    db.Accounts.Add(acc);

                    var teachingClass = new TeachingClass()
                    {
                        Admin = acc,
                        Started = DateTime.Now,
                        Referral = "reff",
                    };

                    acc.AssignedClasses.Add(teachingClass);
                    db.TeachingClasses.Add(teachingClass);

                    Subject subject1 = new Subject
                    {
                        Designation = "Test1",
                        TeachingClass = teachingClass,
                    };

                    db.Subjects.Add(subject1);

                    Subject subject2 = new Subject
                    {
                        Designation = "Test2",
                        TeachingClass = teachingClass,
                    };

                    db.Subjects.Add(subject2);

                    db.SaveChanges();

                    teachingClassId = (int)teachingClass.Id;
                }

                List<OverviewSubjectViewModel> result = subjectPersistence.GetSubjectsForOverviewByClassId(teachingClassId);

                if (result.Count == 2)
                    Assert.IsTrue(true);
                else
                    Assert.IsTrue(false);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void ManageSubject()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                var token = "isvalidtoken";
                var subjectId = 1;
                using (var db = new SuperSmartDb())
                {
                    var acc = new Account()
                    {
                        Created = DateTime.Now,
                        Email = "test@test.test",
                        LoginToken = token
                    };

                    db.Accounts.Add(acc);

                    var teachingClass = new TeachingClass()
                    {
                        Admin = acc,
                        Started = DateTime.Now,
                        Referral = "reff",
                    };

                    acc.AssignedClasses.Add(teachingClass);
                    db.TeachingClasses.Add(teachingClass);

                    var subject = new Subject()
                    {
                        Designation = "Test",
                        TeachingClass = teachingClass
                    };

                    db.Subjects.Add(subject);

                    db.SaveChanges();

                    subjectId = (int)subject.Id;
                }

                subjectPersistence.Manage(new ManageSubjectViewModel()
                {
                    Designation = "TestBlaBla",
                    Id = subjectId,
                }, token);

                using (SuperSmartDb db = new SuperSmartDb())
                {
                    if (db.Subjects.SingleOrDefault(itm => itm.Id == subjectId)?.Designation == "TestBlaBla")
                        Assert.IsTrue(true);
                    else
                        Assert.IsTrue(false);
                }
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;

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
    }
}
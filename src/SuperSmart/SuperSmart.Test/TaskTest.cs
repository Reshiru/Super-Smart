using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;
using System.Linq;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;

namespace SuperSmart.Test
{
    [TestClass]
    public class TaskTest
    {
        ITaskPersistence taskPersistence = new TaskPersistence();

        [TestMethod]
        public void AddNullTaskThrowPropertyExceptionCollection()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                taskPersistence.Create(null, null);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void AddNullLoginTokenTaskThrowPropertyExceptionCollection()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                taskPersistence.Create(new CreateTaskViewModel()
                {
                    Designation = "Task xy",
                    Finished = DateTime.Now.AddDays(10),
                    SubjectId = 1
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
                var token = DatabaseHelper.GenerateFakeAccount();
                var teachingClassId = DatabaseHelper.GenerateFakeTeachingClass(token);
                var subjectId = DatabaseHelper.GenerateFakeSubject(teachingClassId);

                taskPersistence.Create(new CreateTaskViewModel()
                {
                    Designation = "Test",
                    SubjectId = subjectId,
                    Finished = DateTime.Now.AddDays(2)
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

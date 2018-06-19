using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Helper;
using System;

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
        public void CreateTaskSucceed()
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

        [TestMethod]
        public void ManageTaskSucceed()
        {
            DatabaseHelper.SecureDeleteDatabase();

            try
            {
                var token = DatabaseHelper.GenerateFakeAccount();
                var teachingClassId = DatabaseHelper.GenerateFakeTeachingClass(token);
                var subjectId = DatabaseHelper.GenerateFakeSubject(teachingClassId);
                var taskId = DatabaseHelper.GenerateFakeTask(subjectId, token);

                taskPersistence.Manage(new ManageTaskViewModel()
                {
                    Designation = "Test",
                    TaskId = taskId,
                    Finished = DateTime.Now.AddDays(4)
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

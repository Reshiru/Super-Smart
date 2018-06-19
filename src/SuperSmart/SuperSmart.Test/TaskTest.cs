using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
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

        [TestMethod]
        public void SaveValidTaskStatusTest()
        {
            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder()
                .WithAdmin(account)
                .Build();

            var subject = new SubjectBuilder()
                .WithTeachingClass(teachingClass)
                .Build();

            var task = new TaskBuilder()
                .WithOwner(account)
                .WithSubject(subject)
                .Build();

            var appointment = new AppointmentBuilder()
                .WithSubject(subject)
                .Build();
            
            new DatabaseBuilder()
                .WithTask(task)
                .WithAppointment(appointment)
                .WithSubject(subject)
                .WithSecureDatabaseDeleted(true)
                .WithAccount(account)
                .WithTeachingClass(teachingClass)
                .Build();
           
            try
            {
                taskPersistence.SaveTaskStatus(new SaveTaskStatusViewModel()
                {
                    AccountId = account.Id,
                    Status = TaskStatus.Done,
                    TaskId = task.Id
                });
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void SaveNullTasktStatusTest()
        {
            try
            {
                taskPersistence.SaveTaskStatus(null);
                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }
    }
}

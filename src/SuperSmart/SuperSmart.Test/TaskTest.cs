using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
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
            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

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
            var designation = "Test task";
            var finished = DateTime.Now.AddDays(10);
            var fakeSubjectId = 1;

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .Build();

            try
            {
                taskPersistence.Create(new CreateTaskViewModel()
                {
                    Designation = designation,
                    Finished = finished,
                    SubjectId = fakeSubjectId
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
            var designation = "Test";
            var finished = DateTime.Now.AddDays(2);

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
                taskPersistence.Create(new CreateTaskViewModel()
                {
                    Designation = designation,
                    SubjectId = subject.Id,
                    Finished = finished,
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void ManageTaskSucceed()
        {
            var designation = "Test";
            var finished = DateTime.Now.AddDays(4);

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var task = new TaskBuilder().WithOwner(account)
                                           .WithSubject(subject)
                                           .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithTask(task)
                                 .Build();

            try
            {
                taskPersistence.Manage(new ManageTaskViewModel()
                {
                    Designation = designation,
                    TaskId = task.Id,
                    Finished = finished,
                }, account.LoginToken);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void SaveValidTaskStatusTest()
        {
            var state = TaskStatus.Done;

            var account = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var task = new TaskBuilder().WithOwner(account)
                                        .WithSubject(subject)
                                        .Build();

            var appointment = new AppointmentBuilder().WithSubject(subject)
                                                      .Build();
            
            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithTask(task)
                                 .WithAppointment(appointment)
                                 .WithSubject(subject)
                                 .WithAccount(account)
                                 .WithTeachingClass(teachingClass)
                                 .Build();
           
            try
            {
                taskPersistence.SaveTaskStatus(new SaveTaskStatusViewModel()
                {
                    AccountId = account.Id,
                    Status = state,
                    TaskId = task.Id,
                });

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
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

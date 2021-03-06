﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Test.Builder;
using System;
using System.Linq;

namespace SuperSmart.Test
{
    [TestClass]
    public class DashboardTest
    {
        IDashboardPersistence dashboardPersistence = new DashboardPersistence();

        [TestMethod]
        public void GetDashboardDataSuccessfullTest()
        {
            var account = new AccountBuilder().Build();
            var admin = new AccountBuilder().Build();

            var teachingClass = new TeachingClassBuilder().WithAdmin(admin)
                                                          .WithAssignedAccount(account)
                                                          .Build();

            var subject = new SubjectBuilder().WithTeachingClass(teachingClass)
                                              .Build();

            var appointment = new AppointmentBuilder().WithSubject(subject)
                                                      .Build();

            var task = new TaskBuilder().WithSubject(subject)
                                        .WithOwner(account)
                                        .Build();

            new DatabaseBuilder().WithSecureDatabaseDeleted(true)
                                 .WithAccount(account)
                                 .WithAccount(admin)
                                 .WithTeachingClass(teachingClass)
                                 .WithSubject(subject)
                                 .WithAppointment(appointment)
                                 .WithTask(task)
                                 .Build();

            try
            {
                var result = dashboardPersistence.GetDashboardData(account.LoginToken);

                Assert.IsTrue(result.Appointments.Any() && result.Tasks.Any());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex?.Message);
            }
        }

        [TestMethod]
        public void GetDashboardDataEmptyLoginTokenThrowPropertyException()
        {
            var loginToken = string.Empty;

            try
            {
                var result = dashboardPersistence.GetDashboardData(loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }

        [TestMethod]
        public void GetDashboardDataInvalidLoginTokenThrowPropertyException()
        {
            var loginToken = "loginToken";

            try
            {
                var result = dashboardPersistence.GetDashboardData(loginToken);

                Assert.IsTrue(false);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is PropertyExceptionCollection);
            }
        }
    }
}
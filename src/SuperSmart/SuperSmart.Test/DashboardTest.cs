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
    public class DashboardTest
    {
        IDashboardPersistence dashboardPersistence = new DashboardPersistence();

        [TestMethod]
        public void DashboardDataTest()
        {
            DatabaseHelper.SecureDeleteDatabase();

            long userId = 1;
            using (SuperSmartDb db = new SuperSmartDb())
            {

                var acc = new Account()
                {
                    Created = DateTime.Now,
                    Email = "test@test.test",
                    LoginToken = "token"
                };

                db.Accounts.Add(acc);

                var admin = new Account()
                {
                    Created = DateTime.Now,
                    Email = "admin@test.test",
                    LoginToken = "adminToken"
                };

                db.Accounts.Add(admin);

                var teachingClass = new TeachingClass()
                {
                    Admin = admin,
                    Started = DateTime.Now,
                    Referral = "reff",
                };

                teachingClass.AssignedAccounts.Add(acc);

                db.TeachingClasses.Add(teachingClass);

                var sub = new Subject()
                {
                    Designation = "m426",
                    TeachingClass = teachingClass
                };

                db.Subjects.Add(sub);

                var app = new Appointment()
                {
                    Classroom = "m426",
                    Day = DayOfWeek.Monday,
                    From = DateTime.Now.AddHours(3),
                    Until = DateTime.Now.AddHours(6),
                    Subject = sub
                };

                sub.Appointments.Add(app);

                db.Appointments.Add(app);

                var task = new Task()
                {
                    Designation = "Husi",
                    Finished = DateTime.Now.AddHours(4),
                    Owner = acc,
                    Subject = sub,
                };

                db.Tasks.Add(task);

                db.SaveChanges();

                userId = acc.Id;
            }

            DashboardViewModel result = dashboardPersistence.GetDashboardData("token");

            if (result.Appointments.Any() && result.Tasks.Any())
                Assert.IsTrue(true);
            else
                Assert.IsFalse(false);
        }
    }
}
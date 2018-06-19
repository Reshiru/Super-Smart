using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;

namespace SuperSmart.Test.Helper
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Deletes the test database (only in test mode)
        /// </summary>
        public static void SecureDeleteDatabase()
        {
#if TEST
            using (var db = new SuperSmartDb())
            {
                db.Database.EnsureDeleted();
            }
#endif
        }
        
        [Obsolete]
        public static string GenerateFakeAccount()
        {
            using (var db = new SuperSmartDb())
            {
                var account = new Account()
                {
                    Created = DateTime.Now,
                    Email = "test@user.test",
                    FirstName = "Test",
                    LastName = "User",
                    LoginToken = "ValidToken",
                    LastLogin = DateTime.Now,
                    Password = "NonSecurePassword",
                    Salt = "NoSalt",
                };

                db.Accounts.Add(account);

                db.SaveChanges();

                return account.LoginToken;
            }
        }

        [Obsolete]
        public static Int64 GenerateFakeTeachingClass(string loginToken)
        {
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                var teachingClass = new TeachingClass()
                {
                    AssignedAccounts = new List<Account>()
                    {
                        account
                    },
                    Admin = account,
                    Active = true,
                    Designation = "Fake teaching class",
                    NumberOfEducationYears = 4,
                    Referral = "testreferral",
                    Started = DateTime.Now
                };

                db.TeachingClasses.Add(teachingClass);
                db.SaveChanges();

                return teachingClass.Id;
            }
        }

        [Obsolete]
        public static void JoinFakeTeachingClass(long teachingClassId, string loginToken)
        {
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.Include(a => a.AssignedClasses)
                                          .SingleOrDefault(a => a.LoginToken == loginToken);

                var teachingClass = db.TeachingClasses.Include(a => a.AssignedAccounts)
                                                      .SingleOrDefault(t => t.Id == teachingClassId);

                teachingClass.AssignedAccounts.Add(account);
                account.AssignedClasses.Add(teachingClass);

                db.SaveChanges();
            }
        }

        [Obsolete]
        public static Int64 GenerateFakeSubject(long teachingClassId)
        {
            using (var db = new SuperSmartDb())
            {
                var teachingClass = db.TeachingClasses.SingleOrDefault(a => a.Id == teachingClassId);

                var subject = new Subject()
                {
                    Active = true,
                    Designation = "Test modul",
                    TeachingClass = teachingClass,

                };

                db.Subjects.Add(subject);
                db.SaveChanges();

                return subject.Id;
            }
        }

        [Obsolete]
        public static Int64 GenerateFakeTask(long subjectId, string ownerToken)
        {
            using (var db = new SuperSmartDb())
            {
                var subject = db.Subjects.SingleOrDefault(s => s.Id == subjectId);

                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == ownerToken);

                var task = new Task()
                {
                    Active = true,
                    Designation = "Test modul",
                    Subject = subject,
                    Owner = account,
                    Finished = DateTime.Now.AddDays(3)
                };

                db.Tasks.Add(task);
                db.SaveChanges();

                return task.Id;
            }
        }
    }
}
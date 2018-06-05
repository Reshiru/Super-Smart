using System;
using System.Collections.Generic;
using System.Linq;
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

        // ToDo: Implement this type of methods for testing, remove dependency on overlaying methods
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

        public static Int64 GenerateFakeSubject(Int64 teachingClassId)
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
    }
}
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SuperSmart.Core.Data.Enumeration;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class TeachingClassPersistence : ITeachingClassPersistence
    {
        public void Create(CreateTeachingClassViewModel createTeachingClassViewModel, string loginToken)
        {
            if (createTeachingClassViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(createTeachingClassViewModel), "Parameter cannot be null");
            }
            if (loginToken == null || loginToken == string.Empty)
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createTeachingClassViewModel, new ValidationContext(createTeachingClassViewModel, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);
                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }
                var referralLink = string.Empty;
                do
                {
                    referralLink = 16.RandomString();
                } while (db.TeachingClasses.SingleOrDefault(t => t.Referral == referralLink) != null);
                var teachingClass = new TeachingClass()
                {
                    Admin = account,
                    Designation = createTeachingClassViewModel.Designation,
                    NumberOfEducationYears = createTeachingClassViewModel.NumberOfEducationYears,
                    Started = createTeachingClassViewModel.Started,
                    AssignedAccounts = new List<Account>() { account },
                    Referral = referralLink
                };
                account.AssignedClasses.Add(teachingClass);
                db.TeachingClasses.Add(teachingClass);
                db.SaveChanges();
            }
        }

        public void Join(string referral, string loginToken)
        {
            if (referral == null || referral == string.Empty)
            {
                throw new PropertyExceptionCollection(nameof(referral), "Parameter cannot be null or empty");
            }
            if (loginToken == null || loginToken == string.Empty)
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null or empty");
            }
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);
                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }
                var teachingClass = db.TeachingClasses.SingleOrDefault(c => c.Referral == referral);
                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(referral), "Referral invalid");
                }
                account.AssignedClasses.Add(teachingClass);
                teachingClass.AssignedAccounts.Add(account);
                db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            if (id == 0)
            {
                throw new PropertyExceptionCollection(nameof(id), "Parameter cannot be null or empty");
            }

            using (var db = new SuperSmartDb())
            {
                var teachingClass = db.TeachingClasses.SingleOrDefault(a => a.Id == id);
                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "TeachingClass not found");
                }

                teachingClass.Active = false;

                db.SaveChanges();
            }
        }
    }
}

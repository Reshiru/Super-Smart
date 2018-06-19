using AutoMapper;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createTeachingClassViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(createTeachingClassViewModel, serviceProvider: null, items: null), validationResults, true))
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
                    referralLink = 8.RandomString();
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
            if (string.IsNullOrEmpty(referral))
            {
                throw new PropertyExceptionCollection(nameof(referral), "Parameter cannot be null or empty");
            }

            if (string.IsNullOrEmpty(loginToken))
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

        public void RemoveUser(RemoveUserFromTeachingClassViewModel removeUserFromTeachingClassViewModel, string loginToken)
        {
            if (removeUserFromTeachingClassViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(removeUserFromTeachingClassViewModel), "Parameter cannot be null or empty");
            }
            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null or empty");
            }
            using (var db = new SuperSmartDb())
            {
                var teachingClass = db.TeachingClasses.SingleOrDefault(a => a.Id == removeUserFromTeachingClassViewModel.TeachingClassId);
                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "TeachingClass not found");
                }

                var user = db.Accounts.SingleOrDefault(a => a.Id == removeUserFromTeachingClassViewModel.UserId);
                if (user == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "User not found");
                }

                var admin = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);
                if (admin == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "User not found");
                }

                if (teachingClass.Admin.Id != admin.Id)
                {
                    throw new PropertyExceptionCollection(nameof(admin), "User hasn't the permissions to remove a user from this class");
                }

                user.AssignedClasses.Remove(teachingClass);
                teachingClass.AssignedAccounts.Remove(user);
                db.SaveChanges();
            }
        }

        public void Delete(int id, string loginToken)
        {
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Your account couldn't be found. Pleas try to relogin");
                }

                var teachingClass = db.TeachingClasses.SingleOrDefault(a => a.Id == id);

                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "The given teaching class couldn't be found");
                }

                if (teachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account), "You are not permitted to this changes");
                }

                teachingClass.Active = false;

                db.SaveChanges();
            }
        }

        public void Manage(ManageTeachingClassViewModel manageTeachingClassViewModel, string loginToken)
        {
            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Your account couldn't be found. Pleas try to relogin");
                }

                var teachingClass = db.TeachingClasses.SingleOrDefault(c => c.Id == manageTeachingClassViewModel.Id);

                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(manageTeachingClassViewModel.Id), "The given teaching class does not exist");
                }

                if (teachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account), "You are not permitted to this changes");
                }

                teachingClass.Designation = manageTeachingClassViewModel.Designation;
                teachingClass.Started = manageTeachingClassViewModel.Started;

                db.SaveChanges();
            }
        }

        public void ChangeReferral(int id, string loginToken)
        {
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Your account couldn't be found. Pleas try to relogin");
                }

                var teachingClass = db.TeachingClasses.SingleOrDefault(t => t.Id == id);

                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(id), "The given teaching class does not exist");
                }

                if (teachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account), "You are not permitted to this changes");
                }

                var referralLink = string.Empty;

                do
                {
                    teachingClass.Referral = 8.RandomString();
                } while (db.TeachingClasses.SingleOrDefault(t => t.Referral == referralLink) != null);

                db.SaveChanges();
            }
        }

        public OverviewTeachingClassViewModel GetOverview(string loginToken)
        {
            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            using (var db = new SuperSmartDb())
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TeachingClass, TeachingClassViewModel>();
                });

                IMapper mapper = config.CreateMapper();


                List<TeachingClass> teachingClasses = db.TeachingClasses.Where(tc => tc.AssignedAccounts.Any(a => a.LoginToken == loginToken)).ToList();

                return new OverviewTeachingClassViewModel()
                {
                    TeachingClasses = mapper.Map<List<TeachingClassViewModel>>(teachingClasses)
                };
            }
        }
    }
}

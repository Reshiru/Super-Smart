using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    /// <summary>
    /// The teaching class persistence to manage teaching classes
    /// </summary>
    public class TeachingClassPersistence : ITeachingClassPersistence
    {
        /// <summary>
        /// Creates a new teaching class for the given model if valid
        /// </summary>
        /// <param name="createTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        public void Create(CreateTeachingClassViewModel createTeachingClassViewModel, string loginToken)
        {
            Guard.ModelStateCheck(createTeachingClassViewModel);
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Get teaching class to manage
        /// </summary>
        /// <param name="teachingClassId"></param>
        /// <param name="loginToken"></param>
        public ManageTeachingClassViewModel GetManagedTeachingClass(Int64 teachingClassId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var teachingClass = db.TeachingClasses.SingleOrDefault(s => s.Id == teachingClassId);

                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "TeachingClass not found");
                }

                if (teachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "User has no permissions to manage class");
                }

                var manageTeachingClassViewModel = this.GetTeachingClassManageMapper().Map<ManageTeachingClassViewModel>(teachingClass);

                return manageTeachingClassViewModel;
            }
        }

        /// <summary>
        /// Changes properties from a given teaching class
        /// </summary>
        /// <param name="manageTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        public void Manage(ManageTeachingClassViewModel manageTeachingClassViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageTeachingClassViewModel);
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Joins the given class (referral) with the given account
        /// </summary>
        /// <param name="referral"></param>
        /// <param name="loginToken"></param>
        public void Join(string referral, string loginToken)
        {
            Guard.NotNullOrEmpty(referral);
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Remove a user from a teaching class
        /// </summary>
        /// <param name="removeUserFromTeachingClassViewModel"></param>
        /// <param name="loginToken"></param>
        public void RemoveUser(RemoveUserFromTeachingClassViewModel removeUserFromTeachingClassViewModel, string loginToken)
        {
            Guard.ModelStateCheck(removeUserFromTeachingClassViewModel);
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Delete a existing teaching class with id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginToken"></param>
        public void Delete(int id, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

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
        
        /// <summary>
        /// Changes the referral from the given teaching class id
        /// </summary>
        /// <param name="id">teaching class id</param>
        /// <param name="loginToken"></param>
        public void ChangeReferral(int id, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Get teachingClasses for user by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        public OverviewTeachingClassViewModel GetOverview(string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var teachingClasses = db.Accounts.Include(a => a.AssignedClasses)
                                                                .SingleOrDefault(a => a.LoginToken == loginToken)
                                                                .AssignedClasses;

                var mappedTeachingClasses = GetOverviewMapper().Map<List<TeachingClassViewModel>>(teachingClasses);

                var overviewTeachingClassViewModel = new OverviewTeachingClassViewModel()
                {
                    TeachingClasses = mappedTeachingClasses,                                          
                };

                return overviewTeachingClassViewModel;
            }
        }

        private IMapper GetOverviewMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TeachingClass, TeachingClassViewModel>();
            }).CreateMapper();
            
            return mapper;
        }

        /// <summary>
        /// Map teaching class to manage teaching class view model
        /// </summary>
        /// <returns></returns>
        private IMapper GetTeachingClassManageMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TeachingClass, ManageTeachingClassViewModel>();
            }).CreateMapper();

            return mapper;
        }
    }
}
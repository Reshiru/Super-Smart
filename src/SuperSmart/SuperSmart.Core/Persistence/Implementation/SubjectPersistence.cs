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
    /// The subject persistence to manage subject
    /// data
    /// </summary>
    public class SubjectPersistence : ISubjectPersistence
    {
        /// <summary>
        /// Creates a new subject for a given teaching class
        /// </summary>
        /// <param name="createSubjectViewModel"></param>
        /// <param name="loginToken"></param>
        public void Create(CreateSubjectViewModel createSubjectViewModel, string loginToken)
        {
            Guard.ModelStateCheck(createSubjectViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var teachingClass = db.TeachingClasses
                    .Include(t => t.Subjects)
                    .SingleOrDefault(t => t.Id == createSubjectViewModel.TeachingClassId);

                if (teachingClass == null)
                {
                    throw new PropertyExceptionCollection(nameof(teachingClass), "Teaching class not found");
                }

                if (teachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account),
                        "No permissions granted");
                }

                var existingSubject = teachingClass.Subjects
                    .SingleOrDefault(s => s.Designation.ToLower().Trim() == createSubjectViewModel.Designation.ToLower().Trim());

                if (existingSubject != null)
                {
                    throw new PropertyExceptionCollection(nameof(createSubjectViewModel.Designation),
                        "A subject with the same name does already exist");
                }

                var subject = new Subject()
                {
                    Active = true,
                    Designation = createSubjectViewModel.Designation,
                    TeachingClass = teachingClass,
                };

                teachingClass.Subjects.Add(subject);
                db.Subjects.Add(subject);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Changes properties from a given subject class
        /// </summary>
        /// <param name="manageSubjectViewModel"></param>
        /// <param name="loginToken"></param>
        /// <returns>The id of the TeachingClass</returns>
        public Int64 Manage(ManageSubjectViewModel manageSubjectViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageSubjectViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var subject = db.Subjects.Include(s => s.TeachingClass)
                                         .ThenInclude(t => t.AssignedAccounts)
                                         .SingleOrDefault(itm => itm.Id == manageSubjectViewModel.Id);

                if (!subject.TeachingClass.AssignedAccounts.Contains(account))
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "No permissions granted");
                }

                if (subject == null)
                {
                    throw new PropertyExceptionCollection(nameof(subject), "Subject not found");
                }

                subject.Designation = manageSubjectViewModel.Designation;

                db.SaveChanges();

                return subject.TeachingClass.Id;
            }
        }

        /// <summary>
        /// Get Overview of subjects
        /// </summary>
        /// <param name="loginToken"></param>
        /// <param name="classId"></param>
        public OverviewSubjectViewModel GetOverview(string loginToken, Int64 classId)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var subjectQuery = db.Subjects.Include(s => s.TeachingClass)
                                              .ThenInclude(t => t.AssignedAccounts)
                                              .Where(s => s.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken) && 
                                                s.TeachingClass.Id == classId);


                var subjects = GetSubjectOverviewMapper().Map<List<SubjectViewModel>>(subjectQuery);

                var overviewSubjectViewModel = new OverviewSubjectViewModel()
                {
                    Subjects = subjects
                };

                return overviewSubjectViewModel;
            }
        }

        /// <summary>
        /// Check if account has rights to manage subject
        /// </summary>
        /// <param name="subjectId"></param>
        public bool IsAccountClassAdminOfSubject(Int64 subjectId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var subject = db.Subjects.SingleOrDefault(s => s.Id == subjectId);

                if (subject == null)
                {
                    throw new PropertyExceptionCollection(nameof(subject), "Subject not found");
                }

                var hasPermissions = subject.TeachingClass.Admin == account;

                return hasPermissions;
            }
        }

        /// <summary>
        /// Map subjects to subjects view model
        /// </summary>
        /// <returns></returns>
        public IMapper GetSubjectOverviewMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Subject, SubjectViewModel>();
            }).CreateMapper();

            return mapper;
        }
    }
}
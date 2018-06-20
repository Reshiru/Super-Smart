using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
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
        public void Manage(ManageSubjectViewModel manageSubjectViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageSubjectViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if(account == null)
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
            }
        }

        /// <summary>
        /// Get subjects for a overview by Id of a teaching class
        /// </summary>
        /// <param name="classId"></param>
        public List<OverviewSubjectViewModel> GetSubjectsForOverviewByClassId(long classId)
        {
            if (classId == 0)
            {
                throw new PropertyExceptionCollection(nameof(classId), "Parameter cannot be 0");
            }

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Subject, OverviewSubjectViewModel>()
                 .ForMember(vm => vm.Id, map => map.MapFrom(m => m.Id))
                 .ForMember(vm => vm.Designation, map => map.MapFrom(m => m.Designation));
            });

            using (SuperSmartDb db = new SuperSmartDb())
            {
                //TODO: Mapper
                var result = db.Subjects.Where(itm => itm.TeachingClass.Id == classId);

                return Mapper.Map<List<OverviewSubjectViewModel>>(result);
            }
        }
    }
}

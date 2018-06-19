using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class SubjectPersistence : ISubjectPersistence
    {
        public void Create(CreateSubjectViewModel createSubjectViewModel, string loginToken)
        {
            if (createSubjectViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(createSubjectViewModel), "Parameter cannot be null");
            }

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createSubjectViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(createSubjectViewModel, serviceProvider: null, items: null), validationResults, true))
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

                var teachingClass = db.TeachingClasses
                    .Include("Subjects")
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

        public void Manage(ManageSubjectViewModel manageSubjectViewModel, string loginToken)
        {
            if (manageSubjectViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(manageSubjectViewModel), "Parameter cannot be null");
            }

            if (manageSubjectViewModel.Id == 0)
            {
                throw new PropertyExceptionCollection(nameof(manageSubjectViewModel.Id), "Parameter cannot be null");
            }

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(manageSubjectViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(manageSubjectViewModel, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }

            using (var db = new SuperSmartDb())
            {
                var subject = db.Subjects.SingleOrDefault(itm => itm.Id == manageSubjectViewModel.Id);

                if (subject == null)
                {
                    throw new PropertyExceptionCollection(nameof(subject), "Subject not found");
                }

                subject.Designation = manageSubjectViewModel.Designation;

                db.SaveChanges();
            }
        }

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

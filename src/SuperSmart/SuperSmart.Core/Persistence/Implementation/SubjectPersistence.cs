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
            if (!Validator.TryValidateObject(createSubjectViewModel, new ValidationContext(createSubjectViewModel, serviceProvider: null, items: null), validationResults, true))
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

                if(!teachingClass.AssignedAccounts.Any(a => a == account))
                {
                    throw new PropertyExceptionCollection(nameof(account),
                        "No permissions granted");
                }

                var existingSubject = teachingClass.Subjects
                    .SingleOrDefault(s => s.Designation.ToLower().Trim() == createSubjectViewModel.Designation.ToLower().Trim());

                if(existingSubject != null)
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
    }
}

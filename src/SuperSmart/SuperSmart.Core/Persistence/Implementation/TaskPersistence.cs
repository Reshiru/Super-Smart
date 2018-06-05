using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class TaskPersistence : ITaskPersistence
    {
        public void Create(CreateTaskViewModel createTaskViewModel, string loginToken)
        {
            if (createTaskViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(createTaskViewModel), "Parameter cannot be null");
            }

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(createTaskViewModel, new ValidationContext(createTaskViewModel, serviceProvider: null, items: null), validationResults, true))
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

                var subject = db.Subjects
                    .Include(s => s.TeachingClass).ThenInclude(t => t.AssignedAccounts)
                    .Include(s => s.Tasks)
                    .SingleOrDefault(s => s.Id == createTaskViewModel.SubjectId);


                if (subject == null)
                {
                    throw new PropertyExceptionCollection(nameof(subject),
                        "Subject not found");
                }

                if (subject.TeachingClass.AssignedAccounts.All(a => a != account))
                {
                    throw new PropertyExceptionCollection(nameof(account),
                        "No permissions granted");
                }
                
                var task = new Task()
                {
                    Active = true,
                    Designation = createTaskViewModel.Designation,
                    Finished = createTaskViewModel.Finished,
                    Owner = account,
                    Subject = subject
                };

                subject.Tasks.Add(task);
                db.Tasks.Add(task);

                db.SaveChanges();
            }
        }
    }
}
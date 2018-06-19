﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Enumeration;
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
        public void Manage(ManageTaskViewModel manageTaskViewModel, string loginToken)
        {
            if (manageTaskViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(manageTaskViewModel), "Parameter cannot be null");
            }

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(manageTaskViewModel, new ValidationContext(manageTaskViewModel, serviceProvider: null, items: null), validationResults, true))
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

                var subject = db.Subjects.Include(s => s.TeachingClass)
                                         .ThenInclude(t => t.AssignedAccounts)
                                         .SingleOrDefault(s => s.Tasks.Any(t => t.Id == manageTaskViewModel.TaskId));


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

                var task = db.Tasks.SingleOrDefault(t => t.Id == manageTaskViewModel.TaskId);

                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task),
                        "Task not found");
                }

                task.Designation = manageTaskViewModel.Designation;
                task.Finished = manageTaskViewModel.Finished;

                db.SaveChanges();
            }
        }

        public TaskStatus GetTaskStatus(long taskId, long accountId)
        {

            return TaskStatus.New;

            using (var db = new SuperSmartDb())
            {
                AccountTask at = db.AccountTask.SingleOrDefault(itm => itm.Account.Id == accountId && itm.Task.Id == taskId);
                if (at != null)
                    return at.Status;
                else
                    return TaskStatus.New;
        }
        }
    }
}
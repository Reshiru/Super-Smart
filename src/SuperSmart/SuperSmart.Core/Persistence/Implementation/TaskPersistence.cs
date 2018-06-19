using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
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
            if (!Validator.TryValidateObject(createTaskViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(createTaskViewModel, serviceProvider: null, items: null), validationResults, true))
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
                    throw new PropertyExceptionCollection(nameof(subject), "Subject not found");
                }

                if (subject.TeachingClass.AssignedAccounts.All(a => a != account))
                {
                    throw new PropertyExceptionCollection(nameof(account), "No permissions granted");
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

        public bool HasAccountRightsForTask(long id ,string loginToken)
        {
            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null");
            }

            if (id == 0)
            {
                throw new PropertyExceptionCollection(nameof(id), "Parameter cannot be 0");
            }

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                var task = db.Tasks.SingleOrDefault(t => t.Id == id);

                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task), "Task not found");
                }

                return task.Owner == account && task.Subject.TeachingClass.Admin == account;
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
            if (!Validator.TryValidateObject(manageTaskViewModel, new System.ComponentModel.DataAnnotations.ValidationContext(manageTaskViewModel, serviceProvider: null, items: null), validationResults, true))
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
                    throw new PropertyExceptionCollection(nameof(subject), "Subject not found");
                }

                var task = db.Tasks.SingleOrDefault(t => t.Id == manageTaskViewModel.TaskId);

                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task), "Task not found");
                }

                if (task.Owner != account && subject.TeachingClass.Admin != account)
                {
                    throw new PropertyExceptionCollection(nameof(account), "No permissions granted");
                }

                task.Designation = manageTaskViewModel.Designation;
                task.Finished = manageTaskViewModel.Finished;

                db.SaveChanges();
            }
        }

        public TaskStatus GetTaskStatus(long taskId, long accountId)
        {
            using (var db = new SuperSmartDb())
            {
                AccountTask at = db.AccountTask.SingleOrDefault(itm => itm.Account.Id == accountId && itm.Task.Id == taskId);
                if (at != null)
                    return at.Status;
                else
                    return TaskStatus.ToDo;
            }
        }

        public void SaveTaskStatus(SaveTaskStatusViewModel saveTaskStatusViewModel)
        {
            using (var db = new SuperSmartDb())
            {
                if (saveTaskStatusViewModel == null)
                    throw new PropertyExceptionCollection(nameof(saveTaskStatusViewModel), "Parameter cannot be null");

                Account account = db.Accounts.SingleOrDefault(itm => itm.Id == saveTaskStatusViewModel.AccountId);
                if (account == null)
                    throw new PropertyExceptionCollection(nameof(account), "Account not found");

                Task task = db.Tasks.SingleOrDefault(itm => itm.Id == saveTaskStatusViewModel.TaskId);
                if (task == null)
                    throw new PropertyExceptionCollection(nameof(task), "Task not found");

                AccountTask at = db.AccountTask.SingleOrDefault(itm => itm.Account == account && itm.Task == task);

                if (at == null)
                {
                    at = new AccountTask();
                    db.AccountTask.Add(at);

                    at.Account = account;
                    at.Task = task;
                }

                at.Status = saveTaskStatusViewModel.Status;

                db.SaveChanges();
            }
        }

        public OverviewTaskViewModel GetOverview(string loginToken, long subjectId)
        {
            using (SuperSmartDb db = new SuperSmartDb())
            {
                if (string.IsNullOrWhiteSpace(loginToken))
                    throw new PropertyExceptionCollection(nameof(loginToken), "LoginToken cannot be empty");

                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                    throw new PropertyExceptionCollection(nameof(loginToken), "Your account couldn't be found. Pleas try to relogin");


                var tasksQuery = db.Tasks.Where(t => t.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken) && t.Subject.Id == subjectId);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Task, TaskViewModel>();
                });

                IMapper mapper = config.CreateMapper();


                List<TaskViewModel> tasks = mapper.Map<List<TaskViewModel>>(tasksQuery);

                ITaskPersistence taskPersistence = new TaskPersistence();

                tasks.ForEach(itm =>
                {
                    itm.Status = taskPersistence.GetTaskStatus(itm.Id, account.Id);
                });

                return new OverviewTaskViewModel()
                {
                    Tasks = tasks
                };
            }
        }
    }
}
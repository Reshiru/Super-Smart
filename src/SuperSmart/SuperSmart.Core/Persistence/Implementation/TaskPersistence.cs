using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Enumeration;
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
    /// The task persistence to manage 
    /// task data
    /// </summary>
    public class TaskPersistence : ITaskPersistence
    {
        /// <summary>
        /// Creates a new task for a given subject
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        public void Create(CreateTaskViewModel createTaskViewModel, string loginToken)
        {
            Guard.ModelStateCheck(createTaskViewModel);
            Guard.NotNullOrEmpty(loginToken);

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

        /// <summary>
        /// Get task to manage
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="loginToken"></param>
        public ManageTaskViewModel GetManagedTask(Int64 taskId, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var task = db.Tasks.SingleOrDefault(s => s.Id == taskId);

                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task), "Task not found");
                }

                if (task.Owner != account)
                {
                    throw new PropertyExceptionCollection(nameof(task), "User has no permissions to manage task");
                }

                var manageTaskViewModel = this.GetTaskManageMapper().Map<ManageTaskViewModel>(task);

                return manageTaskViewModel;
            }
        }

        /// <summary>
        /// Changes properties from a given task
        /// </summary>
        /// <param name="manageTaskViewModel"></param>
        /// <param name="loginToken"></param>
        public void Manage(ManageTaskViewModel manageTaskViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageTaskViewModel);
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

                manageTaskViewModel.SubjectId = task.Subject.Id;
            }
        }

        /// <summary>
        /// Has account rights to manage task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginToken"></param>
        public bool HasAccountRightsForTask(Int64 id, string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

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

                var hasPermissions = task.Owner == account && task.Subject.TeachingClass.Admin == account;

                return hasPermissions;
            }
        }

        /// <summary>
        /// Save status of a task
        /// </summary>
        /// <param name="saveTaskStatusViewModel"></param>
        public void SaveTaskStatus(SaveTaskStatusViewModel saveTaskStatusViewModel)
        {
            Guard.ModelStateCheck(saveTaskStatusViewModel);

            using (var db = new SuperSmartDb())
            {
                Account account = db.Accounts.SingleOrDefault(itm => itm.Id == saveTaskStatusViewModel.AccountId);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(account), "Account not found");
                }

                var task = db.Tasks.SingleOrDefault(itm => itm.Id == saveTaskStatusViewModel.TaskId);

                if (task == null)
                {
                    throw new PropertyExceptionCollection(nameof(task), "Task not found");
                }

                var accountTask = db.AccountTask.SingleOrDefault(itm => itm.Account == account && itm.Task == task);

                if (accountTask == null)
                {
                    accountTask = new AccountTask();
                    db.AccountTask.Add(accountTask);

                    accountTask.Account = account;
                    accountTask.Task = task;
                }

                accountTask.Status = saveTaskStatusViewModel.Status;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get TaskStatus by taskId and accountId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        public TaskStatus GetTaskStatus(Int64 taskId, Int64 accountId)
        {
            using (var db = new SuperSmartDb())
            {
                var accountTask = db.AccountTask.SingleOrDefault(itm => itm.Account.Id == accountId && itm.Task.Id == taskId);
                var taskState = TaskStatus.ToDo;

                if (accountTask != null)
                {
                    taskState = accountTask.Status;
                }

                return taskState;
            }
        }

        /// <summary>
        /// Get Overview of tasks
        /// </summary>
        /// <param name="loginToken"></param>
        public OverviewTaskViewModel GetOverview(string loginToken, Int64 subjectId)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var tasksQuery = db.Tasks.Where(t => t.Subject.TeachingClass.AssignedAccounts.Any(a => a.LoginToken == loginToken) && t.Subject.Id == subjectId);

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Task, TaskViewModel>();
                });

                IMapper mapper = config.CreateMapper();

                List<TaskViewModel> tasks = mapper.Map<List<TaskViewModel>>(tasksQuery);

                tasks.ForEach(itm =>
                {
                    itm.Status = this.GetTaskStatus(itm.Id, account.Id);
                });

                var overviewTaskViewModel = new OverviewTaskViewModel()
                {
                    Tasks = tasks,
                    SubjectId = subjectId
                };

                return overviewTaskViewModel;
            }
        }

        /// <summary>
        /// Map task to manage task view model
        /// </summary>
        /// <returns></returns>
        private IMapper GetTaskManageMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Task, ManageTaskViewModel>();
            }).CreateMapper();

            return mapper;
        }
    }
}
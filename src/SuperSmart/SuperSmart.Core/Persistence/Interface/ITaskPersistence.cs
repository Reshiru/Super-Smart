﻿using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;
using System;

namespace SuperSmart.Core.Persistence.Interface
{
    /// <summary>
    /// The task persistence to manage 
    /// task data
    /// </summary>
    public interface ITaskPersistence
    {
        /// <summary>
        /// Creates a new task for a given subject
        /// </summary>
        /// <param name="createTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Create(CreateTaskViewModel createTaskViewModel, string loginToken);

        /// <summary>
        /// Get task to manage
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="loginToken"></param>
        ManageTaskViewModel GetManagedTask(Int64 taskId, string loginToken);

        /// <summary>
        /// Changes properties from a given task
        /// </summary>
        /// <param name="manageTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Manage(ManageTaskViewModel manageTaskViewModel, string loginToken);

        /// <summary>
        /// Switches the state from a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="loginToken"></param>
        void InvertTaskStatus(Int64 taskId, string loginToken);

        /// <summary>
        /// Get Overview of tasks
        /// </summary>
        /// <param name="loginToken"></param>
        OverviewTaskViewModel GetOverview(string loginToken, Int64 subjectId);

        /// <summary>
        /// Get TaskStatus by taskId and accountId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        TaskStatus GetTaskStatus(Int64 taskId, Int64 accountId);
    }
}
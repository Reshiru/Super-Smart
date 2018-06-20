using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.ViewModels;

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
        long Create(CreateTaskViewModel createTaskViewModel, string loginToken);

        /// <summary>
        /// Changes properties from a given task
        /// </summary>
        /// <param name="manageTaskViewModel"></param>
        /// <param name="loginToken"></param>
        void Manage(ManageTaskViewModel manageTaskViewModel, string loginToken);

        /// <summary>
        /// Save status of a task
        /// </summary>
        /// <param name="saveTaskStatusViewModel"></param>
        void SaveTaskStatus(SaveTaskStatusViewModel saveTaskStatusViewModel);

        /// <summary>
        /// Has accoutn rights to manage task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loginToken"></param>
        bool HasAccountRightsForTask(long id, string loginToken);

        /// <summary>
        /// Get Overview of tasks
        /// </summary>
        /// <param name="loginToken"></param>
        OverviewTaskViewModel GetOverview(string loginToken, long subjectId);

        /// <summary>
        /// Get TaskStatus by taskId and accountId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="accountId"></param>
        TaskStatus GetTaskStatus(long taskId, long accountId);
    }
}

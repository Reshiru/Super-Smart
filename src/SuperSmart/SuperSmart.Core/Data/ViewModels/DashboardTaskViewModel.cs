using SuperSmart.Core.Data.Enumeration;
using System;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// Represents a task which will be shown on the dashboard view
    /// </summary>
    public class DashboardTaskViewModel
    {
        /// <summary>
        /// The database generated identifier for the task
        /// </summary>
        public Int64 TaskId { get; set; }

        /// <summary>
        /// The designation / title for the task (whats the task about?)
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The time when the task should be successfully finished
        /// </summary>
        public DateTime Finished { get; set; }

        /// <summary>
        /// The converted view visible finished date
        /// </summary>
        public string DisplayFinished { get => this.Finished.ToString("dddd") + ", the " + this.Finished.ToString("dd.mm.yyyy"); }

        /// <summary>
        /// The designation from the subject
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// The state from the task / user account state
        /// </summary>
        public TaskStatus Status { get; set; }
    }
}
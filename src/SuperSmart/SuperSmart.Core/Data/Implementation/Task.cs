using System;
using System.Collections.Generic;

namespace SuperSmart.Core.Data.Implementation
{
    /// <summary>
    /// A task for a subject which is created by an account
    /// </summary>
    public class Task
    {
        /// <summary>
        /// The dataabse generated identifier for the task
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The designation / title for the task (whats the task about?)
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The time when the task should be successfully finished
        /// </summary>
        public DateTime Finished { get; set; }

        /// <summary>
        /// The active state from the task, 
        /// false when "deleted"
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// All documents which are uploaded for the task
        /// </summary>
        public ICollection<Document> Documents { get; set; }

        /// <summary>
        /// The parent subject from the task
        /// </summary>
        public Subject Subject { get; set; }

        /// <summary>
        /// The creator from the task
        /// </summary>
        public Account Owner { get; set; }

        /// <summary>
        /// The Account of a Task
        /// </summary>
        public ICollection<AccountTask> TaskAccounts{ get; set; }
    }
}

using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Task
    /// </summary>
    public class TaskViewModel
    {
        /// <summary>
        /// The Task ID
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The description of the Task
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The TimeStamp of the finished Task
        /// </summary>
        public DateTime Finished { get; set; }

        /// <summary>
        /// The State of the Task
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Is owner of the Task
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// Displaying when the Task is finished
        /// </summary>
        public string DisplayFinished { get => this.Finished.ToString("dd.mm.yyyy"); }

    }
}

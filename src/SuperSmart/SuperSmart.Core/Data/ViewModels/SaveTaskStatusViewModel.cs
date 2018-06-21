using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Save Task Status
    /// </summary>
    public class SaveTaskStatusViewModel
    {
        /// <summary>
        /// The ID of the Account
        /// </summary>
        public Int64 AccountId { get; set; }

        /// <summary>
        /// The ID  of the Task
        /// </summary>
        public Int64 TaskId { get; set; }

        /// <summary>
        /// The State of the Task
        /// </summary>
        public TaskStatus Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Task Overview
    /// </summary>
   public class OverviewTaskViewModel
    {
        /// <summary>
        /// The ID of the Subject
        /// </summary>
        public Int64 SubjectId { get; set; }

        /// <summary>
        /// The List of the Tasks
        /// </summary>
        public List<TaskViewModel> Tasks { get; set; }
    }
}

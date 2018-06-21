using System;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ManageTask ViewModel
    /// </summary>
    public class ManageTaskViewModel
    {
        /// <summary>
        /// The TaskID
        /// </summary>
        public Int64 TaskId { get; set; }

        /// <summary>
        /// The SubjectID
        /// </summary>
        public Int64 SubjectId { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The TimeStamp when the Task is finished
        /// </summary>
        public DateTime Finished { get; set; }
    }
}

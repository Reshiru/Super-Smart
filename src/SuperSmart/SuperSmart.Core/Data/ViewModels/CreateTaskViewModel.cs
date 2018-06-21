using System;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The createTask ViewModel
    /// </summary>
    public class CreateTaskViewModel
    {
        /// <summary>
        /// The Subject ID
        /// </summary>
        public Int64 SubjectId { get; set; }

        /// <summary>
        /// The Task Designation
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The Date when the Task was finished
        /// </summary>
        public DateTime Finished { get; set; }
    }
}

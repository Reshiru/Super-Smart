using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The Create Subject ViewModel
    /// </summary>
    public class CreateSubjectViewModel
    {
        /// <summary>
        /// The TeachingClass ID
        /// </summary>
        public Int64 TeachingClassId { get; set; }

        /// <summary>
        /// The designation of the Subject
        /// </summary>
        [Required(ErrorMessage = "Please enter a designation")]
        [MinLength(1, ErrorMessage = "Please enter a designation")]
        public string Designation { get; set; }
    }
}

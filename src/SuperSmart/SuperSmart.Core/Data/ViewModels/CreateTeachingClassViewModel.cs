using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The create TeachingClass ViewModel
    /// </summary>
    public class CreateTeachingClassViewModel
    {
        /// <summary>
        /// The description of the TeachingClass
        /// </summary>
        [Required(ErrorMessage = "Please enter a designation")]
        [MinLength(1, ErrorMessage = "Please enter a designation")]
        public string Designation { get; set; }

        /// <summary>
        /// The Number of the Education Years
        /// </summary>
        [Required(ErrorMessage = "Please enter a number of education years")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of education years")]
        public int NumberOfEducationYears { get; set; }

        /// <summary>
        /// The Date when the education started
        /// </summary>
        [Required(ErrorMessage = "Please enter the time when the education started")]
        public DateTime Started { get; set; }
    }
}

﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the TeachinClass
    /// </summary>
    public class ManageTeachingClassViewModel
    {
        /// <summary>
        /// The Id of the TeachingClass
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The Designation of the TeachingClass
        /// </summary>
        [Required(ErrorMessage = "Please enter a designation")]
        [MinLength(1, ErrorMessage = "Please enter a designation")]
        public string Designation { get; set; }

        /// <summary>
        /// The TimeStamp when the TachingClass started
        /// </summary>
        [Required(ErrorMessage = "Please enter the time when the education started")]
        public DateTime Started { get; set; }

        /// <summary>
        /// The Number of the Education Years
        /// </summary>
        [Required(ErrorMessage = "Please enter a number of education years")]
        [Range(1, Int64.MaxValue, ErrorMessage = "Please enter a number of education years")]
        public Int64 NumberOfEducationYears { get; set; }
    }
}

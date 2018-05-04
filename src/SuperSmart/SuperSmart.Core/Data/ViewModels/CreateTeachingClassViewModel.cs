﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class CreateTeachingClassViewModel
    {
        [Required(ErrorMessage = "Please enter a designation")]
        [MinLength(1, ErrorMessage = "Please enter a designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please enter a number of education years")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a number of education years")]
        public int NumberOfEducationYears { get; set; }
        [Required(ErrorMessage = "Please enter the time when the education started")]
        public DateTime Started { get; set; }
    }
}

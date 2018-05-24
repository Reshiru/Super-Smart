using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class ManageTeachingClassViewModel
    {
        [Required(ErrorMessage = "Please enter a Referral")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a Referral")]
        public string Referral { get; set; }
        [Required(ErrorMessage = "Please enter a designation")]
        [MinLength(1, ErrorMessage = "Please enter a designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Please enter the time when the education started")]
        public DateTime Started { get; set; }
    }
}

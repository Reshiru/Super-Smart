using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class RemoveUserFromTeachingClassViewModel
    {
        [Required(ErrorMessage = "Id for teaching class required")]
        public Int64 TeachingClassId { get; set; }

        [Required(ErrorMessage = "Id for user required")]
        public Int64 UserId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Remove User from TeachingClass
    /// </summary>
    public class RemoveUserFromTeachingClassViewModel
    {
        /// <summary>
        /// The TeachingClass ID
        /// </summary>
        [Required(ErrorMessage = "Id for teaching class required")]
        public Int64 TeachingClassId { get; set; }
        
        /// <summary>
        /// The ID of the User
        /// </summary>
        [Required(ErrorMessage = "Id for user required")]
        public Int64 UserId { get; set; }
    }
}

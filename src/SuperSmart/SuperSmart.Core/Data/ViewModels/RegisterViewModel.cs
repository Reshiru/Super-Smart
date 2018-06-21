using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Register
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// The E-Mail for the registration
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        /// <summary>
        /// The Password for the registration
        /// </summary>
        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(6, ErrorMessage = "Please enter a password with at least 6 character")]
        public string Password { get; set; }

        /// <summary>
        /// The FirstName of the registration
        /// </summary>
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The LastName of the registration
        /// </summary>
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }
    }
}

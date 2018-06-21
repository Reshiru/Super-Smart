using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Login
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// The E-Mail for the Login
        /// </summary>
        [Required(ErrorMessage = "Please enter a valid email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        /// <summary>
        /// The Password of the Login
        /// </summary>
        [Required(ErrorMessage = "Please enter a password")]
        [MinLength(6, ErrorMessage = "Please enter a password with at least 6 character")]
        public string Password { get; set; }
    }
}

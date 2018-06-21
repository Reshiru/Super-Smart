using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class ManageAccountViewModel
    {
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Please enter Firstname")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter Lastname")]
        public string Lastname { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter a Email")]
        public string Email { get; set; }
    }
}

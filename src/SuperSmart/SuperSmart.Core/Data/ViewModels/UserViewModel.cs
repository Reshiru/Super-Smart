using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the User
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// The ID of the User
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The firstname of the User
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// The lastname of the User
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// The E-Mail of the User
        /// </summary>
        public string Email { get; set; }

    }
}

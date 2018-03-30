using System;
using System.Collections.Generic;

namespace SuperSmart.Core.Data.Implementation
{
    public class Account
    {
        /// <summary>
        /// The account identifier
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// The first name from the account user
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The last name from the account user
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// The email addess from the account
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The current session token / login token
        /// </summary>
        public string LoginToken { get; set; }
        /// <summary>
        /// The time when the user was last logged in
        /// Will be used to verify if the login token is still valid
        /// </summary>
        public DateTime LastLogin { get; set; }
        /// <summary>
        /// The hashed / secured password for the account
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The user defined salt to hash the password
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// The time when the account was created
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// The account linked teaching classes
        /// </summary>
        public ICollection<TeachingClass> TeachingClasses { get; set; }
    }
}

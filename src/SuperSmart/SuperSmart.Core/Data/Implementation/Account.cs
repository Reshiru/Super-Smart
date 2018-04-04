﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.Implementation
{
    public class Account
    {
        /// <summary>
        /// The email addess from the account
        /// </summary>
        [Key]
        public string Email { get; set; }
        /// <summary>
        /// The first name from the account user
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The last name from the account user
        /// </summary>
        public string LastName { get; set; }
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
        /// The assigned classes to the account
        /// </summary>
        public ICollection<TeachingClass> AssignedClasses { get; set; }
        /// <summary>
        /// The premission requested classes which aren't assigned yet
        /// </summary>
        public ICollection<TeachingClass> RequestedClasses { get; set; }
        /// <summary>
        /// The uploaded documents from this account
        /// </summary>
        public ICollection<Document> Documents { get; set; }
    }
}
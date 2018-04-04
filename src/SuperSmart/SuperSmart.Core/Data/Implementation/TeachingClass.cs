﻿using System;
using System.Collections.Generic;
namespace SuperSmart.Core.Data.Implementation
{
    public class TeachingClass
    {
        /// <summary>
        /// The database generated id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// The designation of the teaching class
        /// </summary>
        public string Designation { get; set; }
        /// <summary>
        /// The number of education years until the class is finished
        /// </summary>
        public int NumberOfEducationYears { get; set; }
        /// <summary>
        /// The date when the class was started (start year / month)
        /// </summary>
        public DateTime Started { get; set; }
        /// <summary>
        /// The subjects which are contained in the teaching class
        /// </summary>
        public ICollection<Subject> Subjects { get; set; }
        /// <summary>
        /// The assigned accounts to the class
        /// </summary>
        public ICollection<Account> AssignedAccounts { get; set; }
        /// <summary>
        /// The accounts which have requested access to this class
        /// </summary>
        public ICollection<Account> OpenRequests { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace SuperSmart.Core.Data.Implementation
{
    /// <summary>
    /// The subject
    /// </summary>
    public class Subject
    {
        public Subject()
        {
            this.Appointments = new List<Appointment>();
            this.Tasks = new List<Task>();
        }
        /// <summary>
        /// The database generated id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The designation from the subject
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The state of the subject / active = not archived
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// The appointments for the subject
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; }

        /// <summary>
        /// The tasks which are in todo for the appontment
        /// </summary>
        public ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// The teaching class from the subject
        /// </summary>
        public TeachingClass TeachingClass { get; set; }
    }
}

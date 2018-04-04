using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.Implementation
{
    public class Appointment
    {
        /// <summary>
        /// The database generated id
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// The day when the appointment should be made
        /// </summary>
        public DayOfWeek Day { get; set; }
        /// <summary>
        /// The classroom where the appointment is
        /// </summary>
        public string Classroom { get; set; }
        /// <summary>
        /// From when the appointment is
        /// </summary>
        public DateTime From { get; set; }
        /// <summary>
        /// Until when the appointment is hold
        /// </summary>
        public DateTime Until { get; set; }
        /// <summary>
        /// The subject liked to the appointment
        /// </summary>
        public Subject Subject { get; set; }
    }
}

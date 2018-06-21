using System;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The dashboard appointment view model
    /// </summary>
    public class DashboardAppointmentViewModel
    {
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
        /// The subject id liked to the appointment
        /// </summary>
        public int SubjectId { get; set; }
    }
}
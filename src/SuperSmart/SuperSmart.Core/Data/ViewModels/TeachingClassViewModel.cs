using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the TeachingClass
    /// </summary>
    public class TeachingClassViewModel
    {
        /// <summary>
        /// The ID of the TeacingClass
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The description of the TechingClass
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// The Referral of the TeachingClass
        /// </summary>
        public string Referral { get; set; }

        /// <summary>
        /// The Number of the Education Years
        /// </summary>
        public int NumberOfEducationYears { get; set; }

        /// <summary>
        /// The Timestamp when the TeachingClass started
        /// </summary>
        public DateTime Started { get; set; }

        /// <summary>
        /// Displays the Time when the TeachingClass started
        /// </summary>
        public string DisplayStarted { get => this.Started.ToString("dd.mm.yyyy"); }

        public bool IsAmdin { get; set; }

    }
}

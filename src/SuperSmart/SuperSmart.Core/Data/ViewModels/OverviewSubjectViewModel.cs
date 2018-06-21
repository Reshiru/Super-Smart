using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the OverviewSubject
    /// </summary>
    public class OverviewSubjectViewModel
    {
        /// <summary>
        /// The TeachingClass ID
        /// </summary>
        public Int64 TeachingClassId { get; set; }

        /// <summary>
        /// The List of the Subjects
        /// </summary>
        public List<SubjectViewModel> Subjects { get; set; }

        /// <summary>
        /// The Bool if the User is Class Admin or not
        /// </summary>
        public bool IsClassAdmin { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Subject
    /// </summary>
    public class SubjectViewModel
    {
        /// <summary>
        /// The Subject ID
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The Description of the Subject
        /// </summary>
        public string Designation { get; set; }
    }
}

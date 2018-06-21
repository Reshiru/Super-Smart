using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Overview of the TeachingClass
    /// </summary>
   public class OverviewTeachingClassViewModel
    {
        /// <summary>
        /// The List of the TeachingClass
        /// </summary>
        public List<TeachingClassViewModel> TeachingClasses { get; set; }
    }
}

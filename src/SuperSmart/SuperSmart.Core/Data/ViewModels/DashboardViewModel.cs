using SuperSmart.Core.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Dashboard
    /// </summary>
    public class DashboardViewModel
    {
        /// <summary>
        /// The Tasks that are shown on the Dashboard
        /// </summary>
        public List<DashboardTaskViewModel> Tasks { get; set; }

        /// <summary>
        /// The Appointments that are shown on the Dashboard
        /// </summary>
        public List<DashboardAppointmentViewModel> Appointments { get; set; }

    }
}

using SuperSmart.Core.Data.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DashboardViewModel
    {
        public List<DashboardTaskViewModel> Tasks { get; set; }
        public List<DashboardAppointmentViewModel> Appointments { get; set; }

    }
}

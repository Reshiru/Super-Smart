using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DashboardAppointmentViewModel
    {
        public DayOfWeek Day { get; set; }
        public string Classroom { get; set; }
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public int SubjectId { get; set; }
    }
}

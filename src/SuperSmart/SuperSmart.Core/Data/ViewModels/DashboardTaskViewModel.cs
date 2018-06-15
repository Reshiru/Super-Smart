using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DashboardTaskViewModel
    {
        public Int64 TaskId { get; set; }
        public string Designation { get; set; }
        public DateTime Finished { get; set; }
        public string DisplayFinished { get => this.Finished.ToString("dddd") + ", the " + this.Finished.ToString("dd.mm.yyyy"); }
        public string SubjectName { get; set; }
        public TaskStatus Status { get; set; }
    }
}

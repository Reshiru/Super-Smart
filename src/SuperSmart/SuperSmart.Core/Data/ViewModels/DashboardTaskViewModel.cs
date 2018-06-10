﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DashboardTaskViewModel
    {
        public Int64 TaskId { get; set; }
        public string Designation { get; set; }
        public DateTime Finished { get; set; }
        public string SubjectName { get; set; }
    }
}

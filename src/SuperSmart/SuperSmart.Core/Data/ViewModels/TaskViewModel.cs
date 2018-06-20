using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    public class TaskViewModel
    {
        public Int64 Id { get; set; }

        public string Designation { get; set; }

        public DateTime Finished { get; set; }

        public TaskStatus Status { get; set; }
        
        public string DisplayFinished { get => this.Finished.ToString("dd.mm.yyyy"); }

    }
}

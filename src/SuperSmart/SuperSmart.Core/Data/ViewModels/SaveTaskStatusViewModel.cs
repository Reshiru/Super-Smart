using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    public class SaveTaskStatusViewModel
    {
        public long AccountId { get; set; }
        public long TaskId { get; set; }
        public TaskStatus Status { get; set; }
    }
}

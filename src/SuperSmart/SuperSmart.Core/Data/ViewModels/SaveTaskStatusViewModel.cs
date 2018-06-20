using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmart.Core.Data.ViewModels
{
    public class SaveTaskStatusViewModel
    {
        public Int64 AccountId { get; set; }
        public Int64 TaskId { get; set; }
        public TaskStatus Status { get; set; }
    }
}

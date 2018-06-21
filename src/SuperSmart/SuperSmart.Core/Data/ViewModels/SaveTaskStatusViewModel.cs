using SuperSmart.Core.Data.Enumeration;
using System;

namespace SuperSmart.Core.Data.ViewModels
{
    public class SaveTaskStatusViewModel
    {
        public Int64 TaskId { get; set; }
        public TaskStatus Status { get; set; }
    }
}

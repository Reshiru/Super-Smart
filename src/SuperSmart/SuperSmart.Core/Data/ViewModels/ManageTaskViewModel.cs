using System;

namespace SuperSmart.Core.Data.ViewModels
{
    public class ManageTaskViewModel
    {
        public Int64 TaskId { get; set; }
        public Int64 SubjectId { get; set; }
        public string Designation { get; set; }
        public DateTime Finished { get; set; }
    }
}

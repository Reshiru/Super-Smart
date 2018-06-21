using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
   public class OverviewTaskViewModel
    {
        public Int64 SubjectId { get; set; }

        public List<TaskViewModel> Tasks { get; set; }
    }
}

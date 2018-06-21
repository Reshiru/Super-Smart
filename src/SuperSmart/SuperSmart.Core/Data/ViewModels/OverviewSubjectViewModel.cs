using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class OverviewSubjectViewModel
    {
        public Int64 TeachingClassId { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }

        public bool IsClassAdmin { get; set; }
    }
}

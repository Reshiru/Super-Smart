using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class OverviewStudentsViewModel
    {
        public Int64 TeachingClassId { get; set; }
        public bool IsAdmin { get; set; }
        public Int64 AdminId { get; set; }
        public string Referral { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
    }
}

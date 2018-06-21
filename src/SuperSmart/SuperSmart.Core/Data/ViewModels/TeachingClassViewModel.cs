using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class TeachingClassViewModel
    {
        public Int64 Id { get; set; }
        public string Designation { get; set; }
        public string Referral { get; set; }
        public Int64 NumberOfEducationYears { get; set; }
        public DateTime Started { get; set; }
        public string DisplayStarted { get => this.Started.ToString("dd.mm.yyyy"); }

    }
}

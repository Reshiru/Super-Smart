using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class TeachingClassViewModel
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public int Referral { get; set; }
        public int NumberOfEducationYears { get; set; }
        public DateTime Started { get; set; }
        public string DisplayStarted { get => this.Started.ToString("dd.mm.yyyy"); }

    }
}

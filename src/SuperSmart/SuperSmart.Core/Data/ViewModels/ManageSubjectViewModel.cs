using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class ManageSubjectViewModel
    {
        public Int64 Id { get; set; }
        public Int64 TeachingClassId { get; set; }
        public string Designation { get; set; }
    }
}

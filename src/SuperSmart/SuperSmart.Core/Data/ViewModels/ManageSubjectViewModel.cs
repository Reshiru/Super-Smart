using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ManageSubject ViewModel
    /// </summary>
    public class ManageSubjectViewModel
    {
        /// <summary>
        /// The ID of the Subject
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The Description
        /// </summary>
        public string Designation { get; set; }
    }
}

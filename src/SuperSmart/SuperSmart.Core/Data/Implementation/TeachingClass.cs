using System.Collections.Generic;
namespace SuperSmart.Core.Data.Implementation
{
    public class TeachingClass
    {
        /// <summary>
        /// The designation of the teaching class
        /// </summary>
        public string Designation { get; set; }
        /// <summary>
        /// The subjects which are contained in the teaching class
        /// </summary>
        public ICollection<Subject> Subjects { get; set; }
    }
}

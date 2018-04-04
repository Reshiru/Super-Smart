using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.Implementation
{
    public class Task
    {
        public Int64 Id { get; set; }
        public string Designation { get; set; }
        public DateTime Finished { get; set; }
        public bool Active { get; set; }
        public ICollection<Document> Documents { get; set; }
        public Subject Subject { get; set; }
    }
}

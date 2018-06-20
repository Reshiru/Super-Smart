using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DocumentViewModel
    {
        public Int64 Id { get; set; }
        public string Filename { get; set; }
        public DateTime Uploaded { get; set; }
        public string Uploader { get; set; }
        public DocumentType DocumentType { get; set; }
        public byte[] File { get; set; }
    }
}

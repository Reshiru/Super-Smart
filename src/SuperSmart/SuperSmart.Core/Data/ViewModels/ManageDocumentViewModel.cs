using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SuperSmart.Core.Data.ViewModels
{
    public class ManageDocumentViewModel
    {
        public Int64 Id { get; set; }

        public Int64 TaskId { get; set; }
               
        public byte[] File { get; set; }
             
        public DocumentType DocumentType { get; set; }
          
        public string FileName { get; set; }
            
        public string ContentType { get; set; }
        
        public HttpPostedFileBase FileUpload { get; set; }
    }
}

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
    /// <summary>
    /// The ManageDocument ViewModel
    /// </summary>
    public class ManageDocumentViewModel
    {
        /// <summary>
        /// The ID of the Document
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The ID of the Task
        /// </summary>
        public Int64 TaskId { get; set; }
               
        /// <summary>
        /// The File itself
        /// </summary>
        public byte[] File { get; set; }
             
        /// <summary>
        /// The Type of the Document
        /// </summary>
        public DocumentType DocumentType { get; set; }
        
        /// <summary>
        /// The name of the File
        /// </summary>  
        public string FileName { get; set; }
            
        /// <summary>
        /// The Type of the Content
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary>
        /// The FileUpload
        /// </summary>
        public HttpPostedFileBase FileUpload { get; set; }
    }
}

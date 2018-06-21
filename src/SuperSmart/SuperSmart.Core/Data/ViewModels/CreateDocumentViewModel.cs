using SuperSmart.Core.Data.Enumeration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The create Document ViewModel
    /// </summary>
    public class CreateDocumentViewModel
    {
        /// <summary>
        /// The File that should be uploaded
        /// </summary>
        [Required(ErrorMessage = "Please upload a File")]
        [MinLength(1, ErrorMessage = "Please upload a File")]
        public byte[] File { get; set; }

        /// <summary>
        /// The type of the document
        /// </summary>
        [Required(ErrorMessage = "Please enter a documenttype")]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        [Required(ErrorMessage = "Please enter a filename")]
        public string FileName { get; set; }

        /// <summary>
        /// The type of the content
        /// </summary>
        [Required(ErrorMessage = "Please enter a ContentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// The ID of the Task
        /// </summary>
        [Required(ErrorMessage = "Please enter a taskid")]
        public Int64 TaskId { get; set; }
        
        /// <summary>
        /// The File upload
        /// </summary>
        public HttpPostedFileBase FileUpload { get; set; }
    }
}


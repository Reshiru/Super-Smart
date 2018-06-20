using SuperSmart.Core.Data.Enumeration;
using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class CreateDocumentViewModel
    {
        [Required(ErrorMessage = "Please upload a File")]
        [MinLength(1, ErrorMessage = "Please upload a File")]
        public byte[] File { get; set; }
        [Required(ErrorMessage = "Please enter a documenttype")]
        public DocumentType DocumentType { get; set; }
        [Required(ErrorMessage = "Please enter a filename")]
        public string FileName { get; set; }
        [Required(ErrorMessage = "Please enter a taskid")]
        public long TaskId { get; set; }
    }
}


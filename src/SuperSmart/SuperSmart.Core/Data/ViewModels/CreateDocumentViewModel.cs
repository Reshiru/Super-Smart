﻿using SuperSmart.Core.Data.Enumeration;
using System;
using System.ComponentModel.DataAnnotations;

namespace SuperSmart.Core.Data.ViewModels
{
    public class CreateDocumentViewModel
    {
        [Required(ErrorMessage = "Please uploda a File")]
        public byte[] File { get; set; }
        [Required(ErrorMessage = "Please enter a documenttype")]
        public DocumentType DocumentType { get; set; }
        [Required(ErrorMessage = "Please enter a filename")]
        public string FileName { get; set; }
        [Required(ErrorMessage = "Please enter a taskid")]
        public Int32 TaskId { get; set; }
    }
}

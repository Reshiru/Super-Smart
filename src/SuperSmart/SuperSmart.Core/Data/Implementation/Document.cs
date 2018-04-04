﻿using SuperSmart.Core.Data.Enumeration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.Implementation
{
    public class Document
    {
        /// <summary>
        /// The system generated file name 
        /// </summary>
        [Key]
        public string SystemFileName { get; set; }
        /// <summary>
        /// The document type from the document
        /// </summary>
        public DocumentType DocumentType { get; set; }
        /// <summary>
        /// The original file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The time when the file was uploaded to the server
        /// </summary>
        public DateTime Uploaded { get; set; }
        /// <summary>
        /// The account who uploaded the file
        /// </summary>
        public Account Uploader { get; set; }
        /// <summary>
        /// The task linked to the document
        /// </summary>
        public Task Task { get; set; }
    }
}
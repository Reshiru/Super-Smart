﻿using SuperSmart.Core.Data.Enumeration;
using System;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DocumentViewModel
    {
        public Int64 Id { get; set; }
        public string Filename { get; set; }
        public DateTime Uploaded { get; set; }
        public string Uploader { get; set; }
        public bool IsOwner { get; set; }
        public DocumentType DocumentType { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
    }
}
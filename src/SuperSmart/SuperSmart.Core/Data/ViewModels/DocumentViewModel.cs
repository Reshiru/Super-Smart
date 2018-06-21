using SuperSmart.Core.Data.Enumeration;
using System;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Document
    /// </summary>
    public class DocumentViewModel
    {
        /// <summary>
        /// The Document ID
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The Name of the File
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The TImeStamp when the Document was uploaded
        /// </summary>
        public DateTime Uploaded { get; set; }

        /// <summary>
        /// The Person who uploaded the File
        /// </summary>
        public string Uploader { get; set; }

        /// <summary>
        /// The Owner of the Document
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// The document type
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// The File on its own
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// The type of the Content
        /// </summary>
        public string ContentType { get; set; }
    }
}
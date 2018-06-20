using SuperSmart.Core.Data.Enumeration;
using System;

namespace SuperSmart.Core.Data.Implementation
{
    /// <summary>
    /// Represents a document database entry
    /// </summary>
    public class Document
    {
        /// <summary>
        /// The database generated identifier
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The system generated file name 
        /// </summary>
        public byte[] File { get; set; }

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

        /// <summary>
        /// The state of the document / active = not archived
        /// </summary>
        public bool Active { get; set; }
    }
}
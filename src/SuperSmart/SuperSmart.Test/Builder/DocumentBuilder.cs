using SuperSmart.Core.Data.Enumeration;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Extension;
using System;

namespace SuperSmart.Test.Builder
{
    /// <summary>
    /// The document builder class to initialize 
    /// a new document instance on build
    /// </summary>
    public class DocumentBuilder
    {
        private string filename;
        private byte[] file;
        private DocumentType documenType;
        private string contentType;
        private DateTime uploaded;
        private bool active;
        private Account owner;
        private Task task;

        /// <summary>
        /// Initializes a new account builder class
        /// </summary>
        public DocumentBuilder()
        {
            var guid = Guid.NewGuid().ToString();

            this.filename = "Testfile";
            this.file = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            this.documenType = DocumentType.Document;
            this.contentType = "image/jpg";
            this.uploaded = DateTime.Now;
            this.active = true;
        }

        public DocumentBuilder WithFilename(string filename)
        {
            this.filename = filename;

            return this;
        }

        public DocumentBuilder WithFile(byte[] file)
        {
            this.file = file;

            return this;
        }

        public DocumentBuilder WitDocumenType(DocumentType documenType)
        {
            this.documenType = documenType;

            return this;
        }

        public DocumentBuilder WithContentType(string contentType)
        {
            this.contentType = contentType;

            return this;
        }

        public DocumentBuilder WithUploaded(DateTime uploaded)
        {
            this.uploaded = uploaded;

            return this;
        }

        public DocumentBuilder WithActive(bool active)
        {
            this.active = active;

            return this;
        }

        public DocumentBuilder WithTask(Task task)
        {
            this.task = task;

            return this;
        }

        public DocumentBuilder WithOwner(Account account)
        {
            this.owner = account;

            return this;
        }

        public Document Build()
        {
            var document = new Document()
            {
                File = this.file,
                FileName = this.filename,
                Active = this.active,
                ContentType = this.contentType,
                DocumentType = this.documenType,
                Task = this.task,
                Uploaded = this.uploaded,
                Uploader = this.owner
            };

            return document;
        }
    }
}
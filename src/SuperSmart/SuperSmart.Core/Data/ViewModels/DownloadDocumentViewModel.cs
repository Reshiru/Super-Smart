using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The DownloadDocument ViewModel
    /// </summary>
    public class DownloadDocumentViewModel
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The File on its own
        /// </summary>
        public byte[] File { get; set; }

        /// <summary>
        /// The type of the content
        /// </summary>
        public string ContentType { get; set; }
    }
}

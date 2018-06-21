using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class DownloadDocumentViewModel
    {
        public string Filename { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    /// <summary>
    /// The ViewModel of the Document Overview
    /// </summary>
    public class OverviewDocumentViewModel
    {
        /// <summary>
        /// The Task ID
        /// </summary>
        public Int64 TaskId { get; set; }

        /// <summary>
        /// The List of the documents
        /// </summary>
        public List<DocumentViewModel> Documents { get; set; }
    }
}

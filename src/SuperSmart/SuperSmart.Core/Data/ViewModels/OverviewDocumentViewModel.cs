using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Data.ViewModels
{
    public class OverviewDocumentViewModel
    {
        public Int64 TaskId { get; set; }
        public List<DocumentViewModel> Documents { get; set; }
    }
}

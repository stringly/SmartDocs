using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Types
{
    public class DocumentListViewModelItem
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedDate { get; set; }

        public DocumentListViewModelItem()
        {
        }

        public DocumentListViewModelItem(SmartPPA ppa)
        {
            DocumentId = ppa.PPAId;
            DocumentName = ppa.DocumentName;
            DocumentType = "SmartPPA";
            CreatedDate = ppa.Created;
        }
    }
}

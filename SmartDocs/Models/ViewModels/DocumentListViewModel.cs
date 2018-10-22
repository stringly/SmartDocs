using SmartDocs.Models.Types;
using System.Collections.Generic;

namespace SmartDocs.Models.ViewModels
{
    public class DocumentListViewModel
    {
        public List<DocumentListViewModelItem> Documents { get; set; }

        public DocumentListViewModel()
        {
        }        
    }
}

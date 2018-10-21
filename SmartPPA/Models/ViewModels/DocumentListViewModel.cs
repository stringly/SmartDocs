using SmartPPA.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.ViewModels
{
    public class DocumentListViewModel
    {
        public List<DocumentListViewModelItem> Documents { get; set; }

        public DocumentListViewModel()
        {
        }        
    }
}

using SmartDocs.Models.Types;
using System.Collections.Generic;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// ViewModel that displays a list of Documents
    /// </summary>    
    public class DocumentListViewModel
    {
        /// <summary>
        /// Gets or sets a list of <see cref="T:SmartDocs.Models.Types.DocumentListViewModelItem"/>.
        /// </summary>
        /// <value>
        /// The documents.
        /// </value>
        public List<DocumentListViewModelItem> Documents { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.Types.DocumentListViewModel"/> class.
        /// </summary>
        public DocumentListViewModel()
        {
        }        
    }
}

using System;

namespace SmartDocs.Models.Types
{
    /// <summary>
    /// Class that is used to in views that show a list of Documents in the repo
    /// </summary>
    public class DocumentListViewModelItem
    {
        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        /// <value>
        /// The name of the document.
        /// </value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>
        /// The type of the document.
        /// </value>
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the date the document was created.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.DocumentListViewModelItem"/> class.
        /// <remarks>
        /// Parameterless constructor
        /// </remarks>
        /// </summary>
        public DocumentListViewModelItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.DocumentListViewModelItem"/> class.
        /// </summary>
        /// <remarks>
        /// Builds a viewmodel from an existing <see cref="T:SmartDocs.Models.SmartPPA"/>
        /// </remarks>
        /// <param name="ppa">A <see cref="T:SmartDocs.Models.SmartPPA"/>.</param>
        public DocumentListViewModelItem(SmartPPA ppa)
        {
            DocumentId = ppa.PPAId;
            DocumentName = ppa.DocumentName;
            DocumentType = "SmartPPA";
            CreatedDate = ppa.Created;
        }
    }
}

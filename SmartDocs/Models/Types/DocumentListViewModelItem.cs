using SmartDocs.Extensions;
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
        public string DocumentTypeDisplayName { get; set; }

        public SmartDocument.SmartDocumentType Type { get; set; }

        public string ParentControllerName { get; set; }

        /// <summary>
        /// Gets or sets the date the document was created.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentListViewModelItem"/> class.
        /// <remarks>
        /// Parameterless constructor
        /// </remarks>
        /// </summary>
        public DocumentListViewModelItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentListViewModelItem"/> class.
        /// </summary>
        /// <remarks>
        /// Builds a viewmodel from an existing <see cref="SmartDocument"/>
        /// </remarks>
        /// <param name="doc">A <see cref="SmartDocument"/>.</param>
        public DocumentListViewModelItem(SmartDocument doc)
        {
            DocumentId = doc.DocumentId;
            DocumentName = doc.FileName;
            Type = doc.Type;
            switch (doc.Type)
            {
                case SmartDocument.SmartDocumentType.PPA:
                    DocumentTypeDisplayName = "Smart PPA";
                    ParentControllerName = "SmartPPA";
                    break;
                case SmartDocument.SmartDocumentType.JobDescription:
                    DocumentTypeDisplayName = "Job Description";
                    ParentControllerName = "SmartJobDescription";
                    break;
                case SmartDocument.SmartDocumentType.AwardForm:
                    DocumentTypeDisplayName = "Award Form";
                    ParentControllerName = "AwardForm";
                    break;
                case SmartDocument.SmartDocumentType.PAF:
                    DocumentTypeDisplayName = "Performance Assessement Form";
                    ParentControllerName = "SmartPAF";
                    break;
            }            
            CreatedDate = doc.Created;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that represents a document entry in storage.
    /// </summary>
    public class SmartDocument
    {
        /// <summary>
        /// The integer Id of the document entity.
        /// </summary>
        [Key]
        public int DocumentId { get; set; }
        /// <summary>
        /// The integer id of the <see cref="SmartUser"/> that authored the document.
        /// </summary>
        public int AuthorUserId { get;set; }
        /// <summary>
        /// Navigation field for the <see cref="SmartUser"/> who authored the document.
        /// </summary>
        public virtual SmartUser Author { get;set;}
        /// <summary>
        /// The integer Id of the template associated with the document type.
        /// </summary>
        public int TemplateId { get; set; }
        /// <summary>
        /// Navigation field to the <see cref="SmartTemplate"/> associated with the document type.
        /// </summary>
        public virtual SmartTemplate Template { get; set; }
        /// <summary>
        /// Enumeration for the document's <see cref="SmartDocumentType"/>.
        /// </summary>
        public SmartDocumentType Type { get;set;}
        /// <summary>
        /// The Date the document was created.
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// The Date that the document was last edited.
        /// </summary>
        public DateTime Edited { get; set; }
        /// <summary>
        /// The file name of the document.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// The document's user-provided form data, serialized to XML.
        /// </summary>
        [Column(TypeName = "xml")]
        public string FormData { get; set; }
        /// <summary>
        /// Returns an <see cref="XElement"/> assembled from the document's user-provided form data XML.
        /// </summary>
        [NotMapped]
        public XElement FormDataXml {
            get { return XElement.Parse(FormData); }
            set { FormData = value.ToString(); }
        }
        /// <summary>
        /// Enumeration that represents the type of document.
        /// </summary>
        public enum SmartDocumentType
        {
            /// <summary>
            /// Past Performance Appraisal
            /// </summary>
            [Display(Name = "Past Performance Appraisal")]
            PPA,
            /// <summary>
            /// Counseling Form / Performance Assessment Form
            /// </summary>
            [Display(Name = "Performance Assessment Form")]
            CounselingForm,
            /// <summary>
            /// Job Description Form
            /// </summary>
            [Display(Name = "Job Description")]
            JobDescription,
            /// <summary>
            /// Award Form
            /// </summary>
            [Display(Name = "Award Form")]
            AwardForm

        }
    }


}

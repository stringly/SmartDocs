using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that represents the Template documents in the database
    /// </summary>
    public class SmartTemplate
    {
        /// <summary>
        /// Gets or sets the Template identifier.
        /// </summary>
        /// <value>
        /// The identifier for the Template.
        /// </value>
        [Key]
        public int TemplateId { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the Template Document.
        /// </summary>
        /// <value>
        /// The name of the template document.
        /// </value>
        public string Name { get; set; }
        public string Description { get;set;}
        public virtual ICollection<SmartDocument> Documents { get; set;}

        /// <summary>
        /// Gets or sets the serialized data stream of the template document.
        /// </summary>
        /// <value>
        /// The serialized data stream of the template document.
        /// </value>
        public byte[] DataStream { get; set; }

        //[Column(TypeName = "xml")]
        //public string FieldMap { get; set; }
        //[NotMapped]
        //public XElement FieldMapXml {
        //    get { return XElement.Parse(FieldMap); }
        //    set { FieldMap = value.ToString(); }
        //}
    }
}

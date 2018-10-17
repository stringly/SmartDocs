using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartPPA.Models
{
    public class SmartDocument
    {
        [Key]
        public int DocumentId { get; set; }
        public SmartTemplate Template { get; set; }
        [Column(TypeName="xml")]
        public string FormData { get; set; }
        public User User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [NotMapped]
        public XElement MyXmlColumn {
            get { return XElement.Parse(FormData); }
            set { FormData = value.ToString(); }
        }
    }
}

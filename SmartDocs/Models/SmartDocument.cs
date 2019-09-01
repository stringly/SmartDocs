using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models
{
    public class SmartDocument
    {
        [Key]
        public int DocumentId { get; set; }
        public int AuthorUserId { get;set; }
        public string TemplateId { get; set; }
        public virtual SmartTemplate Template { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string FileName { get; set; }
        [Column(TypeName = "xml")]
        public string FormData { get; set; }
        [NotMapped]
        public XElement FormDataXml {
            get { return XElement.Parse(FormData); }
            set { FormData = value.ToString(); }
        }
    }

}

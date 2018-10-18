using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartPPA.Models
{
    public class SmartPPA
    {
        [Key]
        public int PPAId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public SmartUser Owner { get; set; }
        public SmartTemplate Template { get; set; }
        [Column(TypeName="xml")]
        public string FormData { get; set; }
        public SmartJob Job { get; set;}


        [NotMapped]
        public XElement FormDataXml {
            get { return XElement.Parse(FormData); }
            set { FormData = value.ToString(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartPPA.Models
{
    public class SmartJob
    {
        [Key]
        public int JobId { get; set; }
        public string JobName { get; set; }
        [Column(TypeName="xml")]
        public string JobData { get; set; }

        [NotMapped]
        public XElement JobDataXml {
            get { return XElement.Parse(JobData); }
            set { JobData = value.ToString(); }
        }
    }
}

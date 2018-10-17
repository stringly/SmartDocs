using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartPPA.Models
{
    public class SmartRecord
    {
        [Key]
        public int DocumentId { get; set; }
        public SmartUser User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }


    }
}

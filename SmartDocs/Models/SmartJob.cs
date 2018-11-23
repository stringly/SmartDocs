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
    /// The SmartJob Entity
    /// </summary>
    /// <remarks>
    /// This class represents a DB Job Description object
    /// </remarks>
    public class SmartJob
    {
        /// <summary>
        /// Gets or sets the SmartJob identifier.
        /// </summary>
        /// <value>
        /// The SmartJob identifier.
        /// </value>
        [Key]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the name of the SmartJob.
        /// </summary>
        /// <value>
        /// The name of the SmartJob.
        /// </value>
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

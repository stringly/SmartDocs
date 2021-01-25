using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SmartDocs.OldModels
{
    public partial class Jobs
    {
        public Jobs()
        {
            Ppas = new HashSet<Ppas>();
        }

        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobData { get; set; }
        /// <summary>
        /// Gets or sets the job data XML via XElement.
        /// </summary>
        /// <value>
        /// The job data XML in an XElement Object.
        /// </value>
        [NotMapped]
        public XElement JobDataXml {
            get { return XElement.Parse(JobData); }
            set { JobData = value.ToString(); }
        }

        public ICollection<Ppas> Ppas { get; set; }
    }
}

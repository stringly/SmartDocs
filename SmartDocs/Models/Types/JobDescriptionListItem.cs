using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Types
{
    /// <summary>
    /// Class used in Views that display a list of Job Descripitons
    /// </summary>
    public class JobDescriptionListItem
    {
        /// <summary>
        /// Gets or sets the identifier of the item.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.Types.JobDescriptionListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless constructor
        /// </remarks>
        public JobDescriptionListItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.Types.JobDescriptionListItem"/> class.
        /// </summary>
        /// <remarks>
        /// Constructor that takes a <see cref="T:SmartDocs.Models.SmartJob"/> parameter
        /// </remarks>
        /// <param name="dBJob">A <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
        public JobDescriptionListItem(SmartJob dBJob)
        {
            JobId = dBJob.JobId;
            Rank = dBJob.JobDataXml.Element("Rank").Value;
            Grade = dBJob.JobDataXml.Element("Grade").Value;
            DisplayName = dBJob.JobName;            
        }
    }
   
}

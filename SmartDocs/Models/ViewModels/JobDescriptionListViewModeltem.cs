using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// Class used to populate <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModel"/>
    /// </summary>
    public class JobDescriptionListViewModeltem
    {
        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the name of the job.
        /// </summary>
        /// <value>
        /// The name of the job.
        /// </value>
        public string JobName { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>
        /// The rank.
        /// </value>
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        /// <value>
        /// The grade.
        /// </value>
        public string Grade {get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModeltem"/> class.
        /// </summary>
        /// <remarks>
        /// Parameterless class constructor.
        /// </remarks>
        public JobDescriptionListViewModeltem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModeltem"/> class.
        /// </summary>
        /// <remarks>
        /// Class constructor that takes a <see cref="T:SmartDocs.Models.SmartJob"/> parameter
        /// </remarks>
        /// <param name="dbJob">The <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
        public JobDescriptionListViewModeltem(SmartJob dbJob)
        {
            JobId = dbJob.JobId;
            JobName = dbJob.JobName;
            Grade = dbJob.JobDataXml.Element("Grade").Value;
            Rank = dbJob.JobDataXml.Element("Rank").Value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.ViewModels
{
    public class JobDescriptionListViewModeltem
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string Rank { get; set; }
        public string Grade {get; set; }

        public JobDescriptionListViewModeltem()
        {

        }

        public JobDescriptionListViewModeltem(SmartJob dbJob)
        {
            JobId = dbJob.JobId;
            JobName = dbJob.JobName;
            Grade = dbJob.JobDataXml.Element("Grade").Value;
            Rank = dbJob.JobDataXml.Element("Rank").Value;
        }
    }
}

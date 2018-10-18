using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models.Types
{
    public class JobDescriptionListItem
    {
        public int JobId { get; set; }
        public string Rank { get; set; }
        public string DisplayName { get; set; }
        public string Grade { get; set; }
        

        public JobDescriptionListItem()
        {

        }

        public JobDescriptionListItem(SmartJob dBJob)
        {
            JobId = dBJob.JobId;
            Rank = dBJob.JobDataXml.Element("Rank").Value;
            Grade = dBJob.JobDataXml.Element("Grade").Value;
            DisplayName = dBJob.JobName;            
        }
    }
   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class SmartDocumentRepository : IDocumentRepository
    {
        private SmartDocContext context;

        public SmartDocumentRepository(SmartDocContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<SmartUser> Users => context.Users;
        public IEnumerable<SmartRecord> Documents => context.Documents;
        public IEnumerable<SmartTemplate> Templates => context.Templates;
        public IEnumerable<SmartJob> Jobs => context.Jobs;
        public IEnumerable<SmartPPA> PPAs => context.PPAs;

        public void SaveJob(SmartJob job)
        {
            if (job.JobId == 0)
            {
                context.Jobs.Add(job);
            } else
            {
                SmartJob dbJob = context.Jobs.FirstOrDefault(j => j.JobId == job.JobId);
                if (dbJob != null)
                {
                    dbJob.Name = job.Name;
                    dbJob.JobData = job.JobData;                    
                }
            }
            context.SaveChanges();
        }
    }
}

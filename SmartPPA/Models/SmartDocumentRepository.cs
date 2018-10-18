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
                    dbJob.JobName = job.JobName;
                    dbJob.JobData = job.JobData;                    
                }
            }
            context.SaveChanges();
        }

        public void SaveSmartPPA(SmartPPA ppa)
        {
            if (ppa.PPAId == 0)
            {

                context.PPAs.Add(ppa);
            }
            else
            {
                SmartPPA dbPPA = context.PPAs.FirstOrDefault(p => p.PPAId == ppa.PPAId);
                if (dbPPA != null)
                {
                    dbPPA.FormDataXml = ppa.FormDataXml;
                    dbPPA.Job = ppa.Job;
                    dbPPA.Template = ppa.Template;
                    dbPPA.Modified = DateTime.Now;                    
                }
            }
            context.SaveChanges();
        }
    }
}

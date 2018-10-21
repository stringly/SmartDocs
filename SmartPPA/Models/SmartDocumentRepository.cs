using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class SmartDocumentRepository : IDocumentRepository
    {
        private SmartDocContext context;
        public SmartUser currentUser;

        public SmartDocumentRepository(SmartDocContext ctx, UserResolverService userService)
        {
            context = ctx;
            currentUser = context.Users.FirstOrDefault(u => u.LogonName == userService.GetUserName());
        }

        public IEnumerable<SmartUser> Users => context.Users;
        public IEnumerable<SmartTemplate> Templates => context.Templates;
        public IEnumerable<SmartJob> Jobs => context.Jobs;
        public IEnumerable<SmartPPA> PPAs => context.PPAs.Where(x => x.Owner.UserId == currentUser.UserId).Include(y => y.Job).Include(z => z.Owner);

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
                ppa.Owner = currentUser;
                context.PPAs.Add(ppa);
            }
            else
            {
                SmartPPA dbPPA = context.PPAs.FirstOrDefault(p => p.PPAId == ppa.PPAId);
                if (dbPPA != null)
                {
                    dbPPA.EmployeeFirstName = ppa.EmployeeFirstName;
                    dbPPA.EmployeeLastName = ppa.EmployeeLastName;
                    dbPPA.DepartmentIdNumber = ppa.DepartmentIdNumber;
                    dbPPA.PayrollIdNumber = ppa.DepartmentIdNumber;
                    dbPPA.PositionNumber = ppa.PositionNumber;
                    dbPPA.DepartmentDivision = ppa.DepartmentDivision;
                    dbPPA.DepartmentDivisionCode = ppa.DepartmentDivisionCode;
                    dbPPA.WorkplaceAddress = ppa.WorkplaceAddress;
                    dbPPA.SupervisedByEmployee = ppa.SupervisedByEmployee;
                    dbPPA.StartDate = ppa.StartDate;
                    dbPPA.EndDate = ppa.EndDate;
                    dbPPA.AssessmentComments = ppa.AssessmentComments;
                    dbPPA.RecommendationComments = ppa.RecommendationComments;
                    dbPPA.CategoryScore_1 = ppa.CategoryScore_1;
                    dbPPA.CategoryScore_2 = ppa.CategoryScore_2;
                    dbPPA.CategoryScore_3 = ppa.CategoryScore_3;
                    dbPPA.CategoryScore_4 = ppa.CategoryScore_4;
                    dbPPA.CategoryScore_5 = ppa.CategoryScore_5;
                    dbPPA.CategoryScore_6 = ppa.CategoryScore_6;
                    dbPPA.Job = ppa.Job;
                    dbPPA.Template = ppa.Template;
                    dbPPA.Modified = DateTime.Now; 
                    dbPPA.Owner = currentUser;
                }
            }
            context.SaveChanges();
        }

        public SmartUser GetCurrentUser()
        {
            return currentUser;
        }
    }
}

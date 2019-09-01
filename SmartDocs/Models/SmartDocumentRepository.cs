using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartDocs.Models
{
    /// <summary>
    /// Repository that implements DB interactions
    /// </summary>
    /// <seealso cref="T:SmartDocs.Models.IDocumentRepository" />
    public class SmartDocumentRepository : IDocumentRepository
    {
        private SmartDocContext context;        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.SmartDocumentRepository"/> class.
        /// </summary>
        /// <param name="ctx">An injected <see cref="T:SmartDocs.Models.SmartDocContext"/> database context.</param>
        /// <param name="userService">An injected <see cref="T:SmartDocs.Models.UserResolverService"/>.</param>
        public SmartDocumentRepository(SmartDocContext ctx)
        {
            context = ctx;

            
        }

        /// <summary>
        /// Gets the Users.
        /// </summary>
        /// <value>
        /// The <see cref="T:SmartDocs.Models.SmartUser"/>s.
        /// </value>
        public IEnumerable<SmartUser> Users => context.Users;

        /// <summary>
        /// Gets the Templates.
        /// </summary>
        /// <value>
        /// The <see cref="T:SmartDocs.Models.SmartTemplate"/>s.
        /// </value>
        public IEnumerable<SmartTemplate> Templates => context.Templates;

        /// <summary>
        /// Gets the Jobs.
        /// </summary>
        /// <value>
        /// The <see cref="SmartDocs.Models.SmartJob"/>s.
        /// </value>
        public IEnumerable<SmartJob> Jobs => context.Jobs.OrderBy(x => x.JobName);

        /// <summary>
        /// Gets the PPAs.
        /// </summary>
        /// <value>
        /// The <see cref="T:SmartDocs.Models.SmartPPA"/>s.
        /// </value>
        /// <remarks>
        /// This list will return only the PPA records that belong to the Repo's currentUser
        /// </remarks>
        public IEnumerable<SmartPPA> PPAs => context.PPAs.Include(y => y.Job).Include(z => z.Owner).Include(x => x.Template);

        /// <summary>
        /// Gets the Components.
        /// </summary>
        /// <value>
        /// The <see cref="T:SmartDocs.Models.OrganizationComponent"/>s.
        /// </value>
        public IEnumerable<OrganizationComponent> Components => context.Components;

        /// <summary>
        /// Saves/Updates the <see cref="T:SmartDocs.Models.SmartJob"/>.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartJob with a non-zero SmartJob.JobId, it will update the corresponding SmartJob in the repo. If this method is passed a SmartJob with a SmartJob.JobId = 0, then it will create a new SmartJob in the repo.
        /// </remarks>
        /// <param name="job">The <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
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

        /// <summary>
        /// Removes the <see cref="T:SmartDocs.Models.RemoveJob"/> from the repo.
        /// </summary>
        /// <param name="job">The <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
        public void RemoveJob(SmartJob job)
        {
            context.Jobs.Remove(job);
            context.SaveChanges();
        }

        /// <summary>
        /// Saves/Updates a <see cref="T:SmartDocs.Models.SmartTemplate"/>
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartTemplate with a non-zero SmartTemplate.TemplateId, it will update the corresponding SmartTemplate in the repo. If this method is passed a SmartJob with a SmartTemplate.TemplateId = 0, then it will create a new SmartTemplate in the repo.
        /// </remarks>
        /// <param name="template">A <see cref="T:SmartDocs.Models.SmartTemplate"/> to add/update.</param>
        public void SaveTemplate(SmartTemplate template)
        {
            if (template.TemplateId == 0)
            {
                context.Templates.Add(template);
            }
            else
            {
                SmartTemplate dbTemplate = context.Templates.FirstOrDefault(x => x.TemplateId == template.TemplateId);
                dbTemplate.Name = template.Name;
                dbTemplate.Description = template.Description;
                dbTemplate.DataStream = template.DataStream;
            }            
            context.SaveChanges();
        }

        /// <summary>
        /// Saves/Updates a <see cref="T:SmartDocs.Models.SmartPPA"/>
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartPPA with a non-zero SmartPPA.PPAId, it will update the corresponding SmartPPA in the repo. If this method is passed a SmartJob with a SmartPPA.PPAId = 0, then it will create a new SmartPPA in the repo.
        /// </remarks>
        /// <param name="ppa">A <see cref="T:SmartDocs.Models.SmartPPA"/> to add/update.</param>
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
                    dbPPA.EmployeeFirstName = ppa.EmployeeFirstName;
                    dbPPA.EmployeeLastName = ppa.EmployeeLastName;
                    dbPPA.DepartmentIdNumber = ppa.DepartmentIdNumber;
                    dbPPA.PayrollIdNumber = ppa.PayrollIdNumber;
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
                    //dbPPA.Owner = currentUser; they shouldnt be able to change the owner
                    dbPPA.DocumentName = ppa.DocumentName;
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Removes a <see cref="T:SmartDocs.Models.SmartPPA"/>.
        /// </summary>
        /// <param name="ppa">The <see cref="T:SmartDocs.Models.SmartPPA"/> to remove.</param>
        public void RemoveSmartPPA(SmartPPA ppa)
        {
            context.PPAs.Remove(ppa);
            context.SaveChanges();
        }

        public void SaveSmartDoc(SmartDocument doc)
        {
            if (doc.DocumentId == 0)
            {
                context.Documents.Add(doc);
            }
            else
            {
                SmartDocument toEdit = context.Documents.Where(x => x.DocumentId == doc.DocumentId).FirstOrDefault();
                if (toEdit != null)
                {
                    toEdit.AuthorUserId = doc.AuthorUserId;
                    toEdit.TemplateId = doc.TemplateId;
                    toEdit.Created = doc.Created;
                    toEdit.Edited = DateTime.Now;
                    toEdit.FileName = doc.FileName;
                    toEdit.FormData = doc.FormData;
                }
            }
            context.SaveChanges();
        }
        public void RemoveSmartDoc(SmartDocument doc)
        {
            context.Documents.Remove(doc);
            context.SaveChanges();
        }
        /// <summary>
        /// Saves/Updates a <see cref="T:SmartDocs.Models.SmartUser"/> the user.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartUser with a non-zero SmartUser.UserId, it will update the corresponding SmartUser in the repo. If this method is passed a SmartUser with a SmartUser.UserId = 0, then it will create a new SmartUser in the repo.
        /// </remarks>
        /// <param name="user">The <see cref="T:SmartDocs.Models.SmartUser"/> to be added/updated.</param>
        public void SaveUser(SmartUser user)
        {
            if (user.UserId == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                SmartUser dbUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (dbUser != null)
                {
                    dbUser.DisplayName = user.DisplayName;                    
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Removes a <see cref="T:SmartDocs.Models.SmartUser"/>.
        /// </summary>
        /// <param name="user">The <see cref="T:SmartDocs.Models.SmartUser"/> to remove.</param>
        public void RemoveUser(SmartUser user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Saves/Updates a <see cref="T:SmartDocs.Models.OrganizationComponent"/> the user.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartUser with a non-zero OrganizationComponent.ComponentId, it will update the corresponding OrganizationComponent in the repo. If this method is passed a OrganizationComponent with a OrganizationComponent.ComponentId = 0, then it will create a new OrganizationComponent in the repo.
        /// </remarks>
        /// <param name="component">The <see cref="T:SmartDocs.Models.OrganizationComponent"/> to be added/updated.</param>
        public void SaveComponent(OrganizationComponent component)
        {
            if (component.ComponentId == 0)
            {
                context.Components.Add(component);
            }
            else
            {
                OrganizationComponent dbComponent = context.Components.FirstOrDefault(x => x.ComponentId == component.ComponentId);
                dbComponent.Name = component.Name;
                dbComponent.Address = component.Address;
                dbComponent.DepartmentCode = component.DepartmentCode;               
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Removes a <see cref="T:SmartDocs.Models.OrganizationComponent"/>.
        /// </summary>
        /// <param name="component">The <see cref="T:SmartDocs.Models.OrganizationComponent"/> to remove.</param>
        public void RemoveComponent(OrganizationComponent component)
        {
            context.Components.Remove(component);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>A <see cref="T:SmartDocs.Models.SmartUser"/></returns>
        public SmartUser GetUserByLogonName(string logonName)
        {
            return context.Users.FirstOrDefault(x => x.LogonName == logonName);
        }
    }
}

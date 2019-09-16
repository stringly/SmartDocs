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
        public IEnumerable<SmartDocument> Documents => context.Documents.Include(x => x.Template).Include(x => x.Author);

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
        public IEnumerable<SmartDocument> PPAs => context.Documents.Include(y => y.Template).Include(z => z.Author).Where(x => x.Type == SmartDocument.SmartDocumentType.PPA);

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
        public int SaveSmartDoc(SmartDocument doc)
        {
            int returnId = 0;
            try
            {
                if (doc.DocumentId == 0)
                {
                    context.Documents.Add(doc);                
                    returnId = doc.DocumentId;
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
                        returnId = toEdit.DocumentId;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return 0;
            }
            
            return returnId;
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

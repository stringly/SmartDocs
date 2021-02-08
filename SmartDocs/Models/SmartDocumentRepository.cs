using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace SmartDocs.Models
{
    /// <summary>
    /// Repository that implements DB interactions
    /// </summary>
    /// <seealso cref="IDocumentRepository" />
    public class SmartDocumentRepository : IDocumentRepository
    {
        private SmartDocContext context;        
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartDocumentRepository"/> class.
        /// </summary>
        /// <param name="ctx">An injected <see cref="SmartDocContext"/> database context.</param>
        public SmartDocumentRepository(SmartDocContext ctx)
        {
            context = ctx;            
        }
        /// <summary>
        /// Gets the Users.
        /// </summary>
        /// <value>
        /// The <see cref="SmartUser"/>s.
        /// </value>
        public IEnumerable<SmartUser> Users => context.Users;

        /// <summary>
        /// Gets the Templates.
        /// </summary>
        /// <value>
        /// The <see cref="SmartTemplate"/>s.
        /// </value>
        public IEnumerable<SmartTemplate> Templates => context.Templates;
        /// <summary>
        /// Gets the Documents.
        /// </summary>
        /// <value>
        /// The <see cref="SmartTemplate"/>s.
        /// </value>
        public IEnumerable<SmartDocument> Documents => context.Documents.Include(x => x.Template).Include(x => x.Author);

        /// <summary>
        /// Gets the Jobs.
        /// </summary>
        /// <value>
        /// The <see cref="SmartJob"/>s.
        /// </value>
        public IEnumerable<SmartJob> Jobs => context.Jobs.OrderBy(x => x.JobName);

        /// <summary>
        /// Gets the Components.
        /// </summary>
        /// <value>
        /// The <see cref="OrganizationUnit"/>s.
        /// </value>
        public IEnumerable<OrganizationUnit> Units => context.Units.OrderBy(x => x.Title);

        /// <summary>
        /// Gets the <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.PPA"/>.
        /// </summary>
        /// <value>
        /// The <see cref="SmartDocument"/>s in the repo that are of the type <see cref="SmartDocument.SmartDocumentType.PPA"/>
        /// </value>
        public IEnumerable<SmartDocument> PerformanceAppraisalForms => context.Documents.Include(y => y.Template).Include(z => z.Author).Where(x => x.Type == SmartDocument.SmartDocumentType.PPA);
        /// <summary>
        /// Gets the <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.PAF"/>.
        /// </summary>
        /// <value>
        /// The <see cref="SmartDocument"/>s in the repo that are of the type <see cref="SmartDocument.SmartDocumentType.PAF"/>
        /// </value>
        public IEnumerable<SmartDocument> PerformanceAssessmentForms => context.Documents.Include(y => y.Template).Include(z => z.Author).Where(x => x.Type == SmartDocument.SmartDocumentType.PAF);
        /// <summary>
        /// Gets the <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.AwardForm"/>.
        /// </summary>
        /// <value>
        /// The <see cref="SmartDocument"/>s in the repo that are of the type <see cref="SmartDocument.SmartDocumentType.AwardForm"/>
        /// </value>
        public IEnumerable<SmartDocument> AwardForms => context.Documents.Include(y => y.Template).Include(z => z.Author).Where(x => x.Type == SmartDocument.SmartDocumentType.AwardForm);
        /// <summary>
        /// Gets the <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.JobDescription"/>.
        /// </summary>
        /// <value>
        /// The <see cref="SmartDocument"/>s in the repo that are of the type <see cref="SmartDocument.SmartDocumentType.JobDescription"/>
        /// </value>
        public IEnumerable<SmartDocument> JobDescriptionForms => context.Documents.Include(y => y.Template).Include(z => z.Author).Where(x => x.Type == SmartDocument.SmartDocumentType.JobDescription);
        /// <summary>
        /// Saves/Updates the <see cref="SmartJob"/>.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartJob with a non-zero SmartJob.JobId, it will update the corresponding SmartJob in the repo. If this method is passed a SmartJob with a SmartJob.JobId = 0, then it will create a new SmartJob in the repo.
        /// </remarks>
        /// <param name="job">The <see cref="SmartJob"/>.</param>
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
        /// Removes the <see cref="SmartJob"/> from the repo.
        /// </summary>
        /// <param name="job">The <see cref="SmartJob"/>.</param>
        public void RemoveJob(SmartJob job)
        {
            context.Jobs.Remove(job);
            context.SaveChanges();
        }

        /// <summary>
        /// Saves/Updates a <see cref="SmartTemplate"/>
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartTemplate with a non-zero SmartTemplate.TemplateId, it will update the corresponding SmartTemplate in the repo. If this method is passed a SmartJob with a SmartTemplate.TemplateId = 0, then it will create a new SmartTemplate in the repo.
        /// </remarks>
        /// <param name="template">A <see cref="SmartTemplate"/> to add/update.</param>
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
        /// Saves a <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="doc">The <see cref="SmartDocument"/> to save.</param>
        /// <returns>The saved <see cref="SmartDocument"/></returns>
        public SmartDocument SaveSmartDoc(SmartDocument doc)
        {
            SmartDocument returnDoc = new SmartDocument();
            try
            {
                if (doc.DocumentId == 0)
                {
                    context.Documents.Add(doc);
                    context.SaveChanges();
                    returnDoc = doc;
                }
                else
                {
                    SmartDocument toEdit = context.Documents.Where(x => x.DocumentId == doc.DocumentId).FirstOrDefault();
                    if (toEdit != null)
                    {
                        toEdit.AuthorUserId = doc.AuthorUserId;
                        toEdit.TemplateId = doc.TemplateId;
                        toEdit.Type = doc.Type;
                        toEdit.Created = doc.Created;
                        toEdit.Edited = DateTime.Now;
                        toEdit.FileName = doc.FileName;
                        toEdit.FormData = doc.FormData;                        
                    }
                    context.SaveChanges();
                    returnDoc = toEdit;
                }               
                
                return returnDoc;
            }
            catch (Exception)
            {
                return null;
            }            
        }
        /// <summary>
        /// Removes a <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="doc">The <see cref="SmartDocument"/> to remove.</param>
        public void RemoveSmartDoc(SmartDocument doc)
        {
            context.Documents.Remove(doc);
            context.SaveChanges();
        }
        /// <summary>
        /// Saves/Updates a <see cref="SmartUser"/> the user.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartUser with a non-zero SmartUser.UserId, it will update the corresponding SmartUser in the repo. If this method is passed a SmartUser with a SmartUser.UserId = 0, then it will create a new SmartUser in the repo.
        /// </remarks>
        /// <param name="user">The <see cref="SmartUser"/> to be added/updated.</param>
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
        /// Removes a <see cref="SmartUser"/>.
        /// </summary>
        /// <param name="user">The <see cref="SmartUser"/> to remove.</param>
        public void RemoveUser(SmartUser user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Saves/Updates a <see cref="OrganizationUnit"/>.
        /// </summary>
        /// <remarks>
        /// If this method is passed a SmartUser with a non-zero OrganizationUnit.Id, it will update the corresponding OrganizationUnit in the repo. If this method is passed a OrganizationUnit with a OrganizationUnit.Id = 0, then it will create a new OrganizationUnit in the repo.
        /// </remarks>
        /// <param name="unit">The <see cref="OrganizationUnit"/> to be added/updated.</param>
        public void SaveUnit(OrganizationUnit unit)
        {
            if (unit.Id == 0)
            {
                context.Units.Add(unit);
            }
            else
            {
                OrganizationUnit dbUnit = context.Units.FirstOrDefault(x => x.Id == unit.Id);
                dbUnit.Title = unit.Title;
                dbUnit.Address = unit.Address;
                dbUnit.Code = unit.Code;
                dbUnit.Division = unit.Division;
                dbUnit.Bureau = unit.Bureau;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Removes a <see cref="OrganizationUnit"/>.
        /// </summary>
        /// <param name="unit">The <see cref="OrganizationUnit"/> to remove.</param>
        public void RemoveUnit(OrganizationUnit unit)
        {
            context.Units.Remove(unit);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>A <see cref="SmartUser"/></returns>
        public SmartUser GetUserByLogonName(string logonName)
        {
            return context.Users.FirstOrDefault(x => x.LogonName == logonName);
        }
        /// <summary>
        /// Gets the list of Documents for the User.
        /// </summary>
        public IEnumerable<SmartDocument> GetDocumentsForUser(int userId)
        {
            return context.Documents.Where(x => x.AuthorUserId == userId);
        }
    }
}

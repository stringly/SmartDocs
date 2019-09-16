using Microsoft.EntityFrameworkCore;
using SmartDocs.Models.SmartPPAClasses;
using SmartDocs.Models.ViewModels;
using SmartDocs.OldModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models
{
    public class DataInitializer
    {
        public static void SeedTemplate(SmartDocContext context)
        {
            SmartTemplate template = new SmartTemplate
            {
                Name = "SmartPPA",
                Description = "Performance Appraisal Package: PPA, PAF, and Job Description.",
                Uploaded = DateTime.Now,
                IsActive = true,
                DataStream = File.ReadAllBytes("TemplateNoJobDescriptionCell.docx")
            };
            context.Add(template);
            context.SaveChanges();
        }
        public static void SeedData(IDocumentRepository repository, SmartDocsContext oldContext)
        {
            List<Ppas> oldppas = oldContext.Ppas
                .Include(x => x.Job)
                .Include(x => x.OwnerUser)
                .ToList();
            SmartPPAFactory fact = new SmartPPAFactory(repository);
            foreach(Ppas ppa in oldppas)
            {
                PPAFormViewModel vm = new PPAFormViewModel(ppa);
                fact.CreatePPA(vm);
            }
        }
    }
}

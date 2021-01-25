using Microsoft.EntityFrameworkCore;
using SmartDocs.Models.SmartDocumentClasses;
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

        public SmartDocContext _newContext { get; private set; }
        public SmartDocsContext _oldContext { get; private set; }

        public DataInitializer(SmartDocContext newContext, SmartDocsContext oldContext)
        {
            _newContext = newContext;
            _oldContext = oldContext;
        }
        public void SeedTemplates()
        {
            if (!_newContext.Templates.Any(x => x.Name == "SmartPPA"))
            {
                SmartTemplate template = new SmartTemplate
                {
                    Name = "SmartPPA",
                    Description = "Performance Appraisal Package: PPA, PAF, and Job Description.",
                    Uploaded = DateTime.Now,
                    IsActive = true,
                    DataStream = File.ReadAllBytes("Smart_PPA_Template.docx")
                };
                _newContext.Add(template);
            }
            if(!_newContext.Templates.Any(x => x.Name == "SmartJobDescription"))
            {
                SmartTemplate template = new SmartTemplate
                {
                    Name = "SmartJobDescription",
                    Description = "Job Description Standalone Template.",
                    Uploaded = DateTime.Now,
                    IsActive = true,
                    DataStream = File.ReadAllBytes("Job_Description_Template.docx")
                };
                _newContext.Add(template);
            }
            if(!_newContext.Templates.Any(x => x.Name == "SmartAwardForm"))
            {
                SmartTemplate template = new SmartTemplate
                {
                    Name = "SmartAwardForm",
                    Description = "Award Nomination Form Template",
                    Uploaded = DateTime.Now,
                    IsActive = true,
                    DataStream = File.ReadAllBytes("Award_Form_Template.docx")
                };
                _newContext.Add(template);
            }

            _newContext.SaveChanges();
        }
        public void SeedOldPPAs()
        {
            if(_newContext.Documents.Count() != 0)
            {
                return;
            }
            List<Ppas> oldppas = _oldContext.Ppas
                .Include(x => x.Job)
                .Include(x => x.OwnerUser)
                .ToList();
            SmartPPAFactory fact = new SmartPPAFactory(new SmartDocumentRepository(_newContext));
            foreach(Ppas ppa in oldppas)
            {
                PPAFormViewModel vm = new PPAFormViewModel(ppa);
                fact.CreatePPA(vm);
            }
        }
    }
}

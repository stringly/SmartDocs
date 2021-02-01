using System;
using System.IO;
using System.Linq;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that initializes Metadata for the application
    /// </summary>
    public class DataInitializer
    {        
        /// <summary>
        /// Constructor that initializes the class
        /// </summary>
        /// <param name="newContext">An instance of <see cref="SmartDocContext"/></param>
        public DataInitializer(SmartDocContext newContext)
        {
            _newContext = newContext;
        }
        /// <summary>
        /// An instance of <see cref="SmartDocContext"/>
        /// </summary>
        private SmartDocContext _newContext { get; set; }
        /// <summary>
        /// Method that seeds the Template documents into the Database from the FileSystem.
        /// </summary>
        public void SeedTemplates()
        {
            // Verify that the pre-2020 PPA template is loaded.
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
            // Verify that the 2021 Updated PPA form is loaded.
            if(!_newContext.Templates.Any(x => x.Name == "SmartPPA_2021"))
            {
                SmartTemplate template = new SmartTemplate
                {
                    Name = "SmartPPA_2021",
                    Description = "Performance Appraisal Package: PPA, PAF, and Job Description using the 2021 OHRM Updated Forms",
                    Uploaded = DateTime.Now,
                    IsActive = true,
                    DataStream = File.ReadAllBytes("SmartPPA_2021_Template.docx")
                };
                _newContext.Add(template);
            }
            if (!_newContext.Templates.Any(x => x.Name == "SmartPAF_2021"))
            {
                SmartTemplate template = new SmartTemplate
                {
                    Name = "SmartPAF_2021",
                    Description = "Periodic Performance Assessment Form using the 2021 OHRM Updated Forms",
                    Uploaded = DateTime.Now,
                    IsActive = true,
                    DataStream = File.ReadAllBytes("SmartPAF_2021_Template.docx")
                };
                _newContext.Add(template);
            }
            // As of 2021, the Job Description form is unchanged.
            if (!_newContext.Templates.Any(x => x.Name == "SmartJobDescription"))
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
    }
}

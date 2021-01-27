using DocumentFormat.OpenXml.Packaging;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    /// <summary>
    /// Factory class that generates and updates <see cref="SmartDocument"/>
    /// </summary>
    public class SmartPPAFactory
    {
        /// <summary>
        /// Constructs a new instance of the class
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public SmartPPAFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        /// <summary>
        /// Constructs a new instance of the class from an existing <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        /// <param name="ppa">An <see cref="SmartDocument"/> representing an existing PPA.</param>
        public SmartPPAFactory(IDocumentRepository repo, SmartDocument ppa)
        {
            _repository = repo;
            PPA = ppa;
        }
        private IDocumentRepository _repository { get; set; }
        /// <summary>
        /// A <see cref="SmartDocument"/> that represents the PPA being generated. 
        /// </summary>
        public SmartDocument PPA { get; private set; }
        private static string ACTIVE_TEMPLATE_NAME = "SmartPPA_2021";
        /// <summary>
        /// Creates a new <see cref="SmartDocument"/> from the user-provided form data.
        /// </summary>
        /// <param name="vm">A <see cref="PPAFormViewModel"/></param>
        public void CreatePPA(PPAFormViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Type = SmartDocument.SmartDocumentType.PPA,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.LastName}, {vm.FirstName} PPA {vm.StartDate:MM-dd-yy} to {vm.EndDate:MM-dd-yy}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == ACTIVE_TEMPLATE_NAME),
                FormDataXml = ViewModelToXML(vm)

            };
            PPA  = _repository.SaveSmartDoc(newDoc);
        }
        /// <summary>
        /// Updates an existing <see cref="SmartDocument"/> PPA from user-provided form data.
        /// </summary>
        /// <param name="vm">A <see cref="PPAFormViewModel"/></param>
        public void UpdatePPA(PPAFormViewModel vm)
        {
            SmartDocument toEdit = _repository.Documents.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.LastName}, {vm.FirstName} PPA {vm.StartDate:MM-dd-yy} to {vm.EndDate:MM-dd-yy}.docx";
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == ACTIVE_TEMPLATE_NAME);
                _repository.SaveSmartDoc(toEdit);
            }
            PPA = toEdit;
        }
        /// <summary>
        /// Private method that converts the form data to XML
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>An <see cref="XElement"/></returns>
        private XElement ViewModelToXML(PPAFormViewModel vm)
        {
            XElement root = new XElement("SmartPPA");
            PropertyInfo[] properties = typeof(PPAFormViewModel).GetProperties();
            foreach(PropertyInfo property in properties)
            {
                if (property.Name != "Categories" && property.Name != "Components" && property.Name != "job" && property.Name != "JobList" && property.Name != "Users" && property.Name != "DocumentId")
                {
                    root.Add(new XElement(property.Name, property.GetValue(vm), new XAttribute("id", property.Name)));
                }                
            }
            SmartUser author = _repository.Users.FirstOrDefault(x => x.UserId == vm.AuthorUserId);
            
            root.Add(new XElement("AuthorName", author?.DisplayName ?? "Unknown", new XAttribute("AuthorName", author?.DisplayName ?? "Unknown")));
            XElement job = new XElement("JobDescription");
            job.Add(new XElement("ClassTitle", vm.Job.ClassTitle, new XAttribute("id", "ClassTitle")));
            job.Add(new XElement("WorkingTitle", vm.Job.WorkingTitle, new XAttribute("id", "WorkingTitle")));
            job.Add(new XElement("Grade", vm.Job.Grade, new XAttribute("id", "Grade")));
            job.Add(new XElement("WorkingHours", vm.Job.WorkingHours, new XAttribute("id", "WorkingHours")));
            job.Add(new XElement("JobId", vm.Job.SmartJobId, new XAttribute("id", "JobId")));


            XElement categories = new XElement("Categories", new XAttribute("id", "Categories"));
            foreach (JobDescriptionCategory c in vm.Job.Categories)
            {
                XElement category = new XElement("Category", new XAttribute("id", "Category"));
                category.Add(new XElement("Letter", c.Letter, new XAttribute("id", "Letter")));
                category.Add(new XElement("Weight", c.Weight, new XAttribute("id", "Weight")));
                category.Add(new XElement("Title", c.Title, new XAttribute("id", "Title")));
                category.Add(new XElement("SelectedScore", c.SelectedScore, new XAttribute("id", "SelectedScore")));
                XElement positionDescriptionFields = new XElement("PositionDescriptionFields", new XAttribute("id", "PositionDescriptionFields"));
                foreach(PositionDescriptionItem p in c.PositionDescriptionItems)
                {
                    positionDescriptionFields.Add(new XElement("PositionDescriptionItem", p.Detail));
                }
                category.Add(positionDescriptionFields);
                XElement performanceStandardFields = new XElement("PerformanceStandardFields", new XAttribute("id", "PerformanceStandardFields"));
                foreach(PerformanceStandardItem p in c.PerformanceStandardItems)
                {
                    performanceStandardFields.Add(new XElement("PerformanceStandardItem", p.Detail, new XAttribute("initial", p.Initial)));
                }
                category.Add(performanceStandardFields);
                categories.Add(category);

            }
            job.Add(categories);
            root.Add(job);            
            return(root);
        }
        /// <summary>
        /// Converts an existing <see cref="SmartDocument"/> 
        /// </summary>
        /// <returns></returns>
        public PPAFormViewModel GetViewModelFromXML()
        {
            
            XElement root = PPA.FormDataXml;
            PPAFormViewModel vm = new PPAFormViewModel{
                DocumentId = PPA.DocumentId,
                FirstName = root.Element("FirstName").Value,
                LastName = root.Element("LastName").Value,
                DepartmentIdNumber = root.Element("DepartmentIdNumber").Value,
                PayrollIdNumber = root.Element("PayrollIdNumber").Value,
                PositionNumber = root.Element("PositionNumber").Value,
                DepartmentDivision = root.Element("DepartmentDivision").Value,
                DepartmentDivisionCode = root.Element("DepartmentDivisionCode").Value,
                WorkPlaceAddress = root.Element("WorkPlaceAddress").Value,
                SupervisedByEmployee = root.Element("SupervisedByEmployee").Value,
                StartDate = DateTime.Parse(root.Element("StartDate").Value),
                EndDate = DateTime.Parse(root.Element("EndDate").Value),
                JobId = Convert.ToInt32(root.Element("JobId").Value),
                AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
                Assessment = root.Element("Assessment").Value,
                Recommendation = root.Element("Recommendation").Value,
            };
            vm.Categories = new List<JobDescriptionCategory>();
            vm.Job = new JobDescription();
                        
            XElement job = root.Element("JobDescription");
            vm.Job.SmartJobId = Convert.ToInt32(job.Element("JobId").Value);
            vm.Job.ClassTitle = job.Element("ClassTitle").Value;
            vm.Job.WorkingTitle = job.Element("WorkingTitle").Value;
            vm.Job.Grade = job.Element("Grade").Value;
            vm.Job.WorkingHours = job.Element("WorkingHours").Value;            
            IEnumerable<XElement> CategoryList = job.Element("Categories").Elements("Category");
            foreach(XElement category in CategoryList)
            {
                JobDescriptionCategory cat = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value, 
                    SelectedScore = Convert.ToInt32(category.Element("SelectedScore").Value)
                };
                // each category contains a child element named "PositionDescriptionFields" that contains children named "PositionDescriptionItem"
                IEnumerable<XElement> positionDescriptionFields = category.Element("PositionDescriptionFields").Elements("PositionDescriptionItem");

                // loop through the PositionDescriptionItems and map to PositionDescriptionItem class objects
                foreach (XElement positionDescriptionItem in positionDescriptionFields)
                {
                    PositionDescriptionItem item = new PositionDescriptionItem { Detail = positionDescriptionItem.Value };
                    // add each object to the Category Object's collection
                    cat.PositionDescriptionItems.Add(item);
                }

                // each category contains a child element named "PerformanceStandardFields" that contains children named "PerformanceStandardItem"
                IEnumerable<XElement> performanceStandardFields = category.Element("PerformanceStandardFields").Elements("PerformanceStandardItem");

                // loop through the PerformanceStandardItems and map to PerformanceStandardItem class objects
                foreach (XElement performanceStandardItem in performanceStandardFields)
                {
                    PerformanceStandardItem item = new PerformanceStandardItem { Initial = performanceStandardItem.Attribute("initial").Value, Detail = performanceStandardItem.Value };
                    // add each object to the Category Object's collection
                    cat.PerformanceStandardItems.Add(item);
                }
                vm.Categories.Add(cat);
                vm.Job.Categories.Add(cat);
            }
            return vm;
        }
        /// <summary>
        /// Generates the Word Document
        /// </summary>
        /// <returns>A <see cref="MemoryStream"/> containing the form field values injected into the template.</returns>
        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            try
            {
                byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.Name == ACTIVE_TEMPLATE_NAME).DataStream;
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    SmartPPAMappedFieldSet fields = new SmartPPAMappedFieldSet(mainPart);
                    fields.WriteXMLToFields(PPA.FormDataXml);
                    mainPart.Document.Save();
                }
            }
            catch
            {
                mem.Dispose();
                throw;
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        } 
    }
}

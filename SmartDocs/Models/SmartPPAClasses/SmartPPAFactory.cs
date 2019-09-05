using DocumentFormat.OpenXml.Packaging;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartPPAClasses
{
    public class SmartPPAFactory
    {
        private IDocumentRepository _repository { get; set; }
        public SmartDocument _PPA { get; private set; }
        public SmartPPAFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public SmartPPAFactory(IDocumentRepository repo, SmartDocument PPA)
        {
            _repository = repo;
            _PPA = PPA;

        } 

        public void CreatePPA(PPAFormViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.LastName}, {vm.FirstName} PPA {vm.StartDate.ToString("MM-dd-yy")} to {vm.EndDate.ToString("MM-dd-yy")}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartPPA"),
                FormDataXml = ViewModelToXML(vm)
            };
            _repository.SaveSmartDoc(newDoc);
            _PPA = newDoc;
        }
        public void UpdatePPA(PPAFormViewModel vm)
        {
            SmartDocument toEdit = _repository.Documents.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.LastName}, {vm.FirstName} PPA {vm.StartDate.ToString("MM-dd-yy")} to {vm.EndDate.ToString("MM-dd-yy")}.docx";
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartPPA");
                _repository.SaveSmartDoc(toEdit);
            }
            _PPA = toEdit;
        }
        private XElement ViewModelToXML(PPAFormViewModel vm)
        {
            XElement root = new XElement("SmartPPA");
            PropertyInfo[] properties = typeof(PPAFormViewModel).GetProperties();
            root.Add(new XElement("DocumentId", _PPA?.DocumentId ?? vm.DocumentId, new XAttribute("DocumentId", _PPA?.DocumentId ?? vm.DocumentId)));
            foreach(PropertyInfo property in properties)
            {
                if (property.Name != "Categories" && property.Name != "Components" && property.Name != "job" && property.Name != "JobList" && property.Name != "Users" && property.Name != "DocumentId")
                root.Add(new XElement(property.Name, property.GetValue(vm), new XAttribute("id", property.Name)));
            }
            SmartUser author = _repository.Users.FirstOrDefault(x => x.UserId == vm.AuthorUserId);
            
            root.Add(new XElement("AuthorName", author?.DisplayName ?? "Unknown", new XAttribute("AuthorName", author?.DisplayName ?? "Unknown")));
            XElement job = new XElement("JobDescription");
            job.Add(new XElement("ClassTitle", vm.job.ClassTitle, new XAttribute("id", "ClassTitle")));
            job.Add(new XElement("WorkingTitle", vm.job.WorkingTitle, new XAttribute("id", "WorkingTitle")));
            job.Add(new XElement("Grade", vm.job.Grade, new XAttribute("id", "Grade")));
            job.Add(new XElement("WorkingHours", vm.job.WorkingHours, new XAttribute("id", "WorkingHours")));
            job.Add(new XElement("JobId", vm.job.SmartJobId, new XAttribute("id", "JobId")));


            XElement categories = new XElement("Categories", new XAttribute("id", "Categories"));
            foreach (JobDescriptionCategory c in vm.job.Categories)
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
        public PPAFormViewModel GetViewModelFromXML()
        {
            
            XElement root = _PPA.FormDataXml;
            PPAFormViewModel vm = new PPAFormViewModel{
                DocumentId = _PPA.DocumentId,
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
            vm.job = new JobDescription();
                        
            XElement job = root.Element("JobDescription");
            vm.job.SmartJobId = Convert.ToInt32(job.Element("JobId").Value);
            vm.job.ClassTitle = job.Element("ClassTitle").Value;
            vm.job.WorkingTitle = job.Element("WorkingTitle").Value;
            vm.job.Grade = job.Element("Grade").Value;
            vm.job.WorkingHours = job.Element("WorkingHours").Value;            
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
                vm.job.Categories.Add(cat);
            }
            return vm;
        }

        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            //try
            //{
                byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.TemplateId == 1).DataStream;
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    SmartPPAMappedFieldSet fields = new SmartPPAMappedFieldSet(mainPart);
                    fields.WriteXMLToFields(_PPA.FormDataXml);
                    mainPart.Document.Save();
                }
            //}
            //catch
            //{
            //    mem.Dispose();
            //    throw;
            //}
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        } 
    }
}

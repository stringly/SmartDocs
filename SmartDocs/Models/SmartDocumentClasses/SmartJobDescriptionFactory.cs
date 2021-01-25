using DocumentFormat.OpenXml.Packaging;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmartDocs.Models.SmartDocumentClasses
{
    public class SmartJobDescriptionFactory
    {
        private IDocumentRepository _repository { get; set; }
        public SmartDocument _jobDescription { get; private set; }

        public SmartJobDescriptionFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public SmartJobDescriptionFactory(IDocumentRepository repo, SmartDocument job)
        {
            if(job.Type != SmartDocument.SmartDocumentType.JobDescription)
            {
                throw new InvalidOperationException("The SmartDocument Parameter passed to the factory constructor is of the wrong type.");
            }
            else
            {
                _repository = repo;
                _jobDescription = job;
            }            
        }
        public void CreateSmartJobDescription(SmartJobDescriptionViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Type= SmartDocument.SmartDocumentType.JobDescription,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.LastName} {vm.FirstName} Job Description {DateTime.Now.ToString("MM-dd-yy")}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartJobDescription"),
                FormDataXml = ViewModelToXML(vm)
            };
            _repository.SaveSmartDoc(newDoc);
            _jobDescription = newDoc;
        }
        public void UpdateSmartJobDescription(SmartJobDescriptionViewModel vm)
        {
            SmartDocument toEdit = _repository.Documents.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.LastName}, {vm.FirstName} Job Description {DateTime.Now.ToString("MM-dd-yy")}.docx";
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartJobDescription");
                _repository.SaveSmartDoc(toEdit);
            }
            _jobDescription = toEdit;
        }
        private XElement ViewModelToXML(SmartJobDescriptionViewModel vm)
        {
            XElement root = new XElement("SmartJobDescription");
            PropertyInfo[] properties = typeof(SmartJobDescriptionViewModel).GetProperties();
            root.Add(new XElement("DocumentId", _jobDescription?.DocumentId ?? vm.DocumentId, new XAttribute("DocumentId", _jobDescription?.DocumentId ?? vm.DocumentId)));
            foreach (PropertyInfo property in properties)
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
                XElement positionDescriptionFields = new XElement("PositionDescriptionFields", new XAttribute("id", "PositionDescriptionFields"));
                foreach (PositionDescriptionItem p in c.PositionDescriptionItems)
                {
                    positionDescriptionFields.Add(new XElement("PositionDescriptionItem", p.Detail));
                }
                category.Add(positionDescriptionFields);
                XElement performanceStandardFields = new XElement("PerformanceStandardFields", new XAttribute("id", "PerformanceStandardFields"));
                foreach (PerformanceStandardItem p in c.PerformanceStandardItems)
                {
                    performanceStandardFields.Add(new XElement("PerformanceStandardItem", p.Detail, new XAttribute("initial", p.Initial)));
                }
                category.Add(performanceStandardFields);
                categories.Add(category);

            }
            job.Add(categories);
            root.Add(job);
            return (root);
        }
        public SmartJobDescriptionViewModel GetViewModelFromXML()
        {

            XElement root = _jobDescription.FormDataXml;
            SmartJobDescriptionViewModel vm = new SmartJobDescriptionViewModel
            {
                DocumentId = _jobDescription.DocumentId,
                FirstName = root.Element("FirstName").Value,
                LastName = root.Element("LastName").Value,
                DepartmentIdNumber = root.Element("DepartmentIdNumber").Value,
                PositionNumber = root.Element("PositionNumber").Value,
                DepartmentDivision = root.Element("DepartmentDivision").Value,
                DepartmentDivisionCode = root.Element("DepartmentDivisionCode").Value,
                WorkPlaceAddress = root.Element("WorkPlaceAddress").Value,
                SupervisedByEmployee = root.Element("SupervisedByEmployee").Value,
                JobId = Convert.ToInt32(root.Element("JobId").Value),
                AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
            };
            vm.job = new JobDescription();

            XElement job = root.Element("JobDescription");
            vm.job.SmartJobId = Convert.ToInt32(job.Element("JobId").Value);
            vm.job.ClassTitle = job.Element("ClassTitle").Value;
            vm.job.WorkingTitle = job.Element("WorkingTitle").Value;
            vm.job.Grade = job.Element("Grade").Value;
            vm.job.WorkingHours = job.Element("WorkingHours").Value;
            IEnumerable<XElement> CategoryList = job.Element("Categories").Elements("Category");
            foreach (XElement category in CategoryList)
            {
                JobDescriptionCategory cat = new JobDescriptionCategory
                {
                    Letter = category.Element("Letter").Value,
                    Weight = Convert.ToInt32(category.Element("Weight").Value),
                    Title = category.Element("Title").Value
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
                vm.job.Categories.Add(cat);
            }
            return vm;
        }
        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            //try
            //{
            byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.Name == "SmartJobDescription").DataStream;
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                SmartJobDescriptionMappedFieldSet fields = new SmartJobDescriptionMappedFieldSet(mainPart);
                fields.WriteXMLToFields(_jobDescription.FormDataXml);
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

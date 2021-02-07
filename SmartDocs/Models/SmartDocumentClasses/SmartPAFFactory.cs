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
    /// <summary>
    /// Factory class that generates and updates <see cref="SmartDocument"/>
    /// </summary>
    public class SmartPAFFactory
    {
        /// <summary>
        /// Constructs a new instance of the class
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public SmartPAFFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        /// <summary>
        /// Constructs a new instance of the class from an existing <see cref="SmartDocument"/>
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        /// <param name="ppa">An <see cref="SmartDocument"/> representing an existing PPA.</param>
        public SmartPAFFactory(IDocumentRepository repo, SmartDocument paf)
        {
            _repository = repo;
            PAF = paf;
        }
        private IDocumentRepository _repository { get; set; }
        /// <summary>
        /// A <see cref="SmartDocument"/> that represents the PAF being generated. 
        /// </summary>
        public SmartDocument PAF { get; private set; }
        private static string ACTIVE_TEMPLATE_NAME = "SmartPAF_2021";
        /// <summary>
        /// Creates a new <see cref="SmartDocument"/> from the user-provided form data.
        /// </summary>
        /// <param name="vm">A <see cref="PAFFormViewModel"/></param>
        public void CreatePAF(PAFFormViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Type = SmartDocument.SmartDocumentType.PAF,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.LastName}, {vm.FirstName} {vm.SelectedPAFType} {DateTime.Now:MM-dd-yy}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == ACTIVE_TEMPLATE_NAME),
                FormDataXml = ViewModelToXML(vm)

            };
            PAF = _repository.SaveSmartDoc(newDoc);
        }
        /// <summary>
        /// Updates an existing <see cref="SmartDocument"/> PAF from user-provided form data.
        /// </summary>
        /// <param name="vm">A <see cref="PAFFormViewModel"/></param>
        public void UpdatePAF(PAFFormViewModel vm)
        {
            SmartDocument toEdit = _repository.PerformanceAssessmentForms.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.LastName}, {vm.FirstName} {vm.SelectedPAFType} {DateTime.Now:MM-dd-yy}.docx";
                toEdit.FormDataXml = ViewModelToXML(vm);
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == ACTIVE_TEMPLATE_NAME);
                _repository.SaveSmartDoc(toEdit);
            }
            PAF = toEdit;
        }
        /// <summary>
        /// Private method that converts the form data to XML
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>An <see cref="XElement"/></returns>
        private XElement ViewModelToXML(PAFFormViewModel vm)
        {
            XElement root = new XElement("SmartPAF");
            PropertyInfo[] properties = typeof(PAFFormViewModel).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "Components" && property.Name != "job" && property.Name != "JobList" && property.Name != "Users" && property.Name != "DocumentId" && property.Name != "PAFTypeChoices")
                {
                    root.Add(new XElement(property.Name, property.GetValue(vm), new XAttribute("id", property.Name)));
                }
            }
            SmartUser author = _repository.Users.FirstOrDefault(x => x.UserId == vm.AuthorUserId);
            SmartJob smartJob = _repository.Jobs.FirstOrDefault(x => x.JobId == vm.JobId);
            JobDescription job = new JobDescription(smartJob);
            root.Add(new XElement("AuthorName", author?.DisplayName ?? "Unknown", new XAttribute("AuthorName", author?.DisplayName ?? "Unknown")));
            XElement xJob = new XElement("JobDescription");
            xJob.Add(new XElement("ClassTitle", job.ClassTitle, new XAttribute("id", "ClassTitle")));
            xJob.Add(new XElement("WorkingTitle", job.WorkingTitle, new XAttribute("id", "WorkingTitle")));
            xJob.Add(new XElement("Grade", job.Grade, new XAttribute("id", "Grade")));
            xJob.Add(new XElement("WorkingHours", job.WorkingHours, new XAttribute("id", "WorkingHours")));
            xJob.Add(new XElement("JobId", job.SmartJobId, new XAttribute("id", "JobId")));
            root.Add(xJob);
            return (root);
        }
        /// <summary>
        /// Converts an existing <see cref="SmartDocument"/> 
        /// </summary>
        /// <returns></returns>
        public PAFFormViewModel GetViewModelFromXML()
        {

            XElement root = PAF.FormDataXml;
            PAFFormViewModel vm = new PAFFormViewModel()
            {
                DocumentId = PAF.DocumentId,
                SelectedPAFType = root.Element("SelectedPAFType").Value,
                FirstName = root.Element("FirstName").Value,
                LastName = root.Element("LastName").Value,
                DepartmentIdNumber = root.Element("DepartmentIdNumber").Value,
                PayrollIdNumber = root.Element("PayrollIdNumber").Value,
                DepartmentDivision = root.Element("DepartmentDivision").Value,
                DepartmentDivisionCode = root.Element("DepartmentDivisionCode").Value,
                StartDate = DateTime.Parse(root.Element("StartDate").Value),
                EndDate = DateTime.Parse(root.Element("EndDate").Value),
                JobId = Convert.ToInt32(root.Element("JobId").Value),
                AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
                Assessment = root.Element("Assessment").Value,
                Recommendation = root.Element("Recommendation").Value,
            };
            vm.HydrateLists(_repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(), _repository.Components.ToList(), _repository.Users.Select(x => new UserListItem(x)).ToList());
            return vm;
        }
        /// <summary>
        /// Generates the Word Document
        /// </summary>
        /// <returns>A <see cref="MemoryStream"/> containing the form field values injected into the template.</returns>
        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            //try
            //{
                byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.Name == ACTIVE_TEMPLATE_NAME).DataStream;
                mem.Write(byteArray, 0, byteArray.Length);
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
                {
                    MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                    SmartPAFMappedFieldSet fields = new SmartPAFMappedFieldSet(mainPart);
                    fields.WriteXMLToFields(PAF.FormDataXml);
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

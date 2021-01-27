using DocumentFormat.OpenXml.Packaging;
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
    /// Factory that builds Award Forms
    /// </summary>
    public class SmartAwardFactory
    {
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public SmartAwardFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        /// <param name="award">The <see cref="SmartDocument"/> Award Form being generated.</param>
        public SmartAwardFactory(IDocumentRepository repo, SmartDocument award)
        {
            if (award.Type != SmartDocument.SmartDocumentType.AwardForm)
            {
                throw new InvalidOperationException("The SmartDocument Parameter passed to the factory constructor is of the wrong type.");
            }
            else
            {
                _repository = repo;
                awardForm = award;
            }
        }
        private IDocumentRepository _repository { get; set; }
        /// <summary>
        /// The <see cref="SmartDocument"/> Award Form being generated.
        /// </summary>
        public SmartDocument awardForm { get; private set; }
        /// <summary>
        /// Creates a new <see cref="SmartDocument"/> from the user-provided form data.
        /// </summary>
        /// <param name="vm">An instance of <see cref="SmartAwardViewModel"/> view model.</param>
        public void CreateSmartAwardForm(SmartAwardViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Type = SmartDocument.SmartDocumentType.AwardForm,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.NomineeName} {vm.AwardName} Form {DateTime.Now:MM-dd-yy}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartAwardForm"),
                FormDataXml = ViewModelToXML(vm)
            };
            _repository.SaveSmartDoc(newDoc);
            awardForm = newDoc;
        }
        /// <summary>
        /// Updates an existing <see cref="SmartDocument"/> award form from user-provided form data.
        /// </summary>
        /// <param name="vm"></param>
        public void UpdateAwardForm(SmartAwardViewModel vm)
        {
            SmartDocument toEdit = _repository.Documents.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.NomineeName} {vm.AwardName} Form {DateTime.Now:MM-dd-yy}.docx";
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartAwardForm");
                toEdit.FormDataXml = ViewModelToXML(vm);
                _repository.SaveSmartDoc(toEdit);
            }
            awardForm = toEdit;
        }
        /// <summary>
        /// Private method that converts the user-provided form data into the XML field of the <see cref="SmartDocument"/>.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>An <see cref="XElement"/></returns>
        private XElement ViewModelToXML(SmartAwardViewModel vm)
        {
            XElement root = new XElement("SmartAward");
            PropertyInfo[] properties;
            switch (vm.SelectedAward)
            {
                case 1:
                    properties = typeof(GoodConductAwardViewModel).GetProperties();
                    break;
                case 2:
                    properties = typeof(OutstandingPerformanceAwardViewModel).GetProperties();
                    break;
                default:
                    throw new NotImplementedException("The Viewmodel has an unrecognized SelectedAwardType");
            }
            
            root.Add(new XElement("DocumentId", awardForm?.DocumentId ?? vm.DocumentId, new XAttribute("DocumentId", awardForm?.DocumentId ?? vm.DocumentId)));
            foreach(PropertyInfo property in properties)
            {
                if (property.Name != "DocumentId" && property.Name != "Components" && property.Name != "Users" && property.Name != "AwardList" && property.Name != "Components" && property.Name != "Users" && property.Name != "AwardTypes")
                {
                    root.Add(new XElement(property.Name, property.GetValue(vm), new XAttribute("id", property.Name)));
                }
            }
            SmartUser author = _repository.Users.FirstOrDefault(x => x.UserId == vm.AuthorUserId);

            root.Add(new XElement("AuthorName", author?.DisplayName ?? "Unknown", new XAttribute("AuthorName", author?.DisplayName ?? "Unknown")));
            XElement award = new XElement("AwardType");
            return (root);
        }

        /// <summary>
        /// Generates a <see cref="SmartAwardViewModel"/> from the <see cref="SmartDocument"/>
        /// </summary>
        /// <returns>A <see cref="SmartAwardViewModel"/></returns>
        public SmartAwardViewModel GetViewModelFromXML()
        {
            XElement root = awardForm.FormDataXml;           
            SmartAwardViewModel vm;
            switch (root.Element("ComponentViewName").Value)
            {
                case "GoodConduct":
                    vm = new GoodConductAwardViewModel
                    {
                        DocumentId = awardForm.DocumentId,
                        AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
                        AgencyName = root.Element("AgencyName").Value,
                        NomineeName = root.Element("NomineeName").Value,
                        ClassTitle = root.Element("ClassTitle").Value,
                        Division = root.Element("Division").Value,
                        SelectedAward = 1,
                        Kind = root.Element("Kind").Value,
                        AwardClass = root.Element("AwardClass").Value,
                        AwardName = root.Element("AwardName").Value,
                        ComponentViewName = root.Element("ComponentViewName").Value,
                        Description = root.Element("Description").Value,
                        HasRibbon = Convert.ToBoolean(root.Element("HasRibbon").Value),
                        EligibilityConfirmationDate = Convert.ToDateTime(root.Element("EligibilityConfirmationDate").Value)

                    };
                    break;
                case "Exemplary":
                    vm = new OutstandingPerformanceAwardViewModel
                    {
                        DocumentId = awardForm.DocumentId,
                        AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
                        AgencyName = root.Element("AgencyName").Value,
                        NomineeName = root.Element("NomineeName").Value,
                        ClassTitle = root.Element("ClassTitle").Value,
                        Division = root.Element("Division").Value,
                        SelectedAward = 2,
                        Kind = root.Element("Kind").Value,
                        AwardClass = root.Element("AwardClass").Value,
                        AwardName = root.Element("AwardName").Value,
                        ComponentViewName = root.Element("ComponentViewName").Value,
                        Description = root.Element("Description").Value,
                        HasRibbon = Convert.ToBoolean(root.Element("HasRibbon").Value),
                        StartDate = Convert.ToDateTime(root.Element("StartDate").Value),
                        EndDate = Convert.ToDateTime(root.Element("EndDate").Value),
                        SelectedAwardType = Convert.ToInt32(root.Element("SelectedAwardType").Value)
                    };
                    break;
                default:
                    throw new NotImplementedException("The Award Type specified in the FormData collection is missing or invalid.");
            }
            vm.Components = _repository.Components.ToList();
            vm.Users = _repository.Users.ToList();
            vm.AwardList = new List<AwardSelectListOption>
            {
                new AwardSelectListOption
                {
                    Text = "Good Conduct Award",
                    Value = "1",
                    SubText = "(No Sustained Discipline in past 24 months.)"

                },
                new AwardSelectListOption
                {
                    Text = "Exemplary Performance",
                    Value = "2",
                    SubText = "(Exceeds Satisfactory or above on appraisal)"
                },
            };
            return vm;
        }
        /// <summary>
        /// Generates and serialized the document.
        /// </summary>
        /// <returns></returns>
        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.Name == "SmartAwardForm").DataStream;
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                SmartAwardMappedFieldSet fields = new SmartAwardMappedFieldSet(mainPart);
                fields.WriteXMLToFields(awardForm.FormDataXml);
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }
    }
}

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
    public class SmartAwardFactory
    {
        private IDocumentRepository _repository { get; set; }
        public SmartDocument _awardForm { get; private set;}

        public SmartAwardFactory(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public SmartAwardFactory(IDocumentRepository repo, SmartDocument award)
        {
            if (award.Type != SmartDocument.SmartDocumentType.AwardForm)
            {
                throw new InvalidOperationException("The SmartDocument Parameter passed to the factory constructor is of the wrong type.");
            }
            else
            {
                _repository = repo;
                _awardForm = award;
            }
        }
        public void CreateSmartAwardForm(AwardFormViewModel vm)
        {
            SmartDocument newDoc = new SmartDocument
            {
                AuthorUserId = vm.AuthorUserId,
                Type = SmartDocument.SmartDocumentType.AwardForm,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                FileName = $"{vm.NomineeName} {vm.Award.Name} Form {DateTime.Now.ToString("MM-dd-yy")}.docx",
                Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartAwardForm"),
                FormDataXml = ViewModelToXML(vm)
            };
            _repository.SaveSmartDoc(newDoc);
            _awardForm = newDoc;
        }
        public void UpdateAwardForm(AwardFormViewModel vm)
        {
            SmartDocument toEdit = _repository.Documents.FirstOrDefault(x => x.DocumentId == vm.DocumentId);
            if (toEdit != null)
            {
                toEdit.AuthorUserId = vm.AuthorUserId;
                toEdit.Edited = DateTime.Now;
                toEdit.FileName = $"{vm.NomineeName} {vm.Award.Name} Form {DateTime.Now.ToString("MM-dd-yy")}.docx";
                toEdit.Template = _repository.Templates.FirstOrDefault(x => x.Name == "SmartAwardForm");
                _repository.SaveSmartDoc(toEdit);
            }
            _awardForm = toEdit;
        }

        private XElement ViewModelToXML(AwardFormViewModel vm)
        {
            XElement root = new XElement("SmartAward");
            PropertyInfo[] properties = typeof(AwardFormViewModel).GetProperties();
            root.Add(new XElement("DocumentId", _awardForm?.DocumentId ?? vm.DocumentId, new XAttribute("DocumentId", _awardForm?.DocumentId ?? vm.DocumentId)));
            foreach(PropertyInfo property in properties)
            {
                if (property.Name != "Award" && property.Name != "DocumentId" && property.Name != "Components" && property.Name != "Users" && property.Name != "AwardList")
                {
                    root.Add(new XElement(property.Name, property.GetValue(vm), new XAttribute("id", property.Name)));
                }
            }
            SmartUser author = _repository.Users.FirstOrDefault(x => x.UserId == vm.AuthorUserId);

            root.Add(new XElement("AuthorName", author?.DisplayName ?? "Unknown", new XAttribute("AuthorName", author?.DisplayName ?? "Unknown")));
            XElement award = new XElement("AwardType");
            PropertyInfo[] awardProperties;
            switch (vm.Award.Kind)
            {
                case "GoodConductAward":
                    awardProperties = typeof(GoodConductAward).GetProperties();
                    break;
                case "OutstandingPerformanceAward":
                    awardProperties = typeof(OutstandingPerformanceAward).GetProperties();
                    break;
                default:
                    awardProperties = typeof(AwardType).GetProperties();
                    break;
            }            
            foreach(PropertyInfo property in awardProperties)
            {
                award.Add(new XElement(property.Name, property.GetValue(vm.Award), new XAttribute("id", property.Name)));
            }
            root.Add(award);
            return (root);
        }

        public AwardFormViewModel GetViewModelFromXML()
        {
            XElement root = _awardForm.FormDataXml;
            AwardFormViewModel vm = new AwardFormViewModel
            {
                DocumentId = _awardForm.DocumentId,
                AuthorUserId = Convert.ToInt32(root.Element("AuthorUserId").Value),
                AgencyName = root.Element("AgencyName").Value,
                NomineeName = root.Element("Nominee").Value,
                ClassTitle = root.Element("ClassTitle").Value,
                Division = root.Element("Division").Value
            };
            XElement award = root.Element("AwardType");
            AwardType awardType;
            switch (award.Element("ComponentViewName").Value)
            {
                case "GoodConduct":
                    awardType = new GoodConductAward
                    {
                        EligibilityConfirmationDate = Convert.ToDateTime(award.Element("EligibilityConfirmationDate").Value),
                    };
                    break;
                case "Exemplary":
                    awardType = new OutstandingPerformanceAward
                    {
                        StartDate = Convert.ToDateTime(award.Element("StartDate").Value),
                        EndDate = Convert.ToDateTime(award.Element("EndDate").Value),
                        SelectedAwardType = Convert.ToInt32(award.Element("SelectedAwardType").Value)
                    };
                    break;
                default:
                    throw new NotImplementedException("The Award Type specified in the FormData collection is missing or invalid.");
            }
            vm.Award = awardType;
            return vm;
        }

        public MemoryStream GenerateDocument()
        {
            var mem = new MemoryStream();
            byte[] byteArray = _repository.Templates.FirstOrDefault(t => t.Name == "SmartAwardForm").DataStream;
            mem.Write(byteArray, 0, byteArray.Length);
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true))
            {
                MainDocumentPart mainPart = wordDocument.MainDocumentPart;
                SmartAwardMappedFieldSet fields = new SmartAwardMappedFieldSet(mainPart);
                fields.WriteXMLToFields(_awardForm.FormDataXml);
                mainPart.Document.Save();
            }
            mem.Seek(0, SeekOrigin.Begin);
            return mem;
        }
    }
}

using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        }
    }
}

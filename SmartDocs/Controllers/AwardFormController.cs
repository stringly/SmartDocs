using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    [Authorize(Roles = "User, Administrator")]
    public class AwardFormController : Controller
    {
        private IDocumentRepository _repository;


        public AwardFormController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(string NomineeName, string ClassTitle, string Division, string Agency = "Prince George's County Police Department")
        {
            int UserId = 0;
            if(User.HasClaim(x => x.Type == "UserId"))
            {
                UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                EmptyAwardViewModel vm = new EmptyAwardViewModel();
                vm.AuthorUserId = UserId;
                vm.AgencyName = Agency;
                vm.NomineeName = NomineeName;
                vm.ClassTitle = ClassTitle;
                vm.Division = Division;
                vm.Components = _repository.Components.ToList();
                vm.Users = _repository.Users.ToList();

                ViewData["Title"] = "Create Award Form";
                return View(vm);
            }
            else
            {
                return RedirectToAction("Access Denied", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentId,AuthorUserId,AgencyName,NomineeName,ClassTitle,Division,SelectedAward,Kind,AwardClass,AwardName,ComponentViewName,Description,HasRibbon,EligibilityConfirmationDate,StartDate,EndDate,SelectedAwardType")] SmartAwardViewModel form)
        {            
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Create Award Form: Error";
                form.Components = _repository.Components.ToList();
                form.Users = _repository.Users.ToList();
                return View(form);
            }
            else
            {
                // do Award Form Factory stuff
                SmartAwardFactory factory = new SmartAwardFactory(_repository);                
                factory.CreateSmartAwardForm(form);
                return RedirectToAction("SaveSuccess", new { id = factory.awardForm.DocumentId });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            SmartDocument award = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            if (award == null)
            {
                return NotFound();
            }
            SmartAwardFactory factory = new SmartAwardFactory(_repository, award);
            SmartAwardViewModel vm = factory.GetViewModelFromXML();
            ViewData["Title"] = "Edit Award Form";
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,AuthorUserId,AgencyName,NomineeName,ClassTitle,Division,SelectedAward,Kind,AwardClass,AwardName,ComponentViewName,Description,HasRibbon,EligibilityConfirmationDate,StartDate,EndDate,SelectedAwardType")] SmartAwardViewModel form)
        {
            if(id != form.DocumentId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // rebuild and return VM
                ViewData["Title"] = "Edit Award Form: Error";
                form.Components = _repository.Components.ToList();
                form.Users = _repository.Users.ToList();
                return View(form);
            }
            SmartDocument awardDoc = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            if (awardDoc == null)
            {
                return NotFound();
            }
            SmartAwardFactory factory = new SmartAwardFactory(_repository, awardDoc);
            factory.UpdateAwardForm(form);
            return RedirectToAction("SaveSuccess", new { id = factory.awardForm.DocumentId });

        }
        public IActionResult SaveSuccess(int id)
        {
            
            // this is a simple view, so use VB instead of a VM            
            ViewBag.SmartDocumentId = id;
            ViewBag.FileName = _repository.Documents.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }
        public IActionResult GetAwardFormViewComponent(int awardId)
        {
            AwardTypeFormViewComponentViewModel awardVM;

            switch (awardId)
            {
                case 1:
                    GoodConductAwardFormViewComponentViewModel goodConductAward = new GoodConductAwardFormViewComponentViewModel();
                    goodConductAward.EligibilityConfirmationDate = DateTime.Now;
                    awardVM = goodConductAward;                    
                    break;
                case 2:
                    OutstandingPerformanceAwardFormViewComponentViewModel outAward = new OutstandingPerformanceAwardFormViewComponentViewModel();
                    outAward.EndDate = DateTime.Now;
                    outAward.StartDate = outAward.EndDate.AddYears(-1);
                    awardVM = outAward;
                    break;
                default:
                    return NotFound();
            }
            if(awardVM != null)
            {
                // return the ViewComponent
                return ViewComponent("AwardTypeForm", awardVM);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult Delete(int? id)
        {
            // query string is empty, return 404
            if (id == null)
            {
                return NotFound();
            }
            // retrieve the SmartPPA from the repo
            var toDelete = _repository.Documents.FirstOrDefault(m => m.DocumentId == id);

            if (toDelete == null)
            {
                // no SmartPPA could be found with the provided id
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            ViewData["Title"] = "Delete Award Form";
            return View(toDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var toDelete = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            // invoke the repo method to remove the SmartPPA
            _repository.RemoveSmartDoc(toDelete);
            // redirect to the Index
            return RedirectToAction("Index", "Home");
        }
    }
}
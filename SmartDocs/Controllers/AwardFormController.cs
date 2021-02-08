using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for Award Form interactions
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Policy = "IsUser")]
    public class AwardFormController : Controller
    {
        private IDocumentRepository _repository;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public AwardFormController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        /// <summary>
        /// Returns a view that will allow a User to create a new Award Form.
        /// </summary>
        /// <param name="NomineeName">A string containing the name of the award nominee.</param>
        /// <param name="ClassTitle">A string containing the nominee's class titme.</param>
        /// <param name="Division">A string containing the nominee's Department/Division name.</param>
        /// <param name="Agency">A string containing the name of the nominee's agency.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpGet]
        public IActionResult Create(string NomineeName, string ClassTitle, string Division, string Agency = "Prince George's County Police Department")
        {
            int UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            EmptyAwardViewModel vm = new EmptyAwardViewModel();
            vm.AuthorUserId = UserId;
            vm.AgencyName = Agency;
            vm.NomineeName = NomineeName;
            vm.ClassTitle = ClassTitle;
            vm.Division = Division;
            vm.Units = _repository.Units.ToList();
            vm.Users = _repository.Users.ToList();
            ViewData["Title"] = "Create Award Form";
            return View(vm);

        }
        /// <summary>
        /// Handles the form POST from the GET/Create View
        /// </summary>
        /// <param name="form">User-provided form data bound to a <see cref="SmartAwardViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DocumentId,AuthorUserId,AgencyName,NomineeName,ClassTitle,Division,SelectedAward,Kind,AwardClass,AwardName,ComponentViewName,Description,HasRibbon,EligibilityConfirmationDate,StartDate,EndDate,SelectedAwardType")] SmartAwardViewModel form)
        {            
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Create Award Form: Error";
                form.Units = _repository.Units.ToList();
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
        /// <summary>
        /// Returns a view that allows a User to edit an existing Award Form SmartDoc.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the document to edit.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Edit(int id)
        {
            SmartDocument award = _repository.AwardForms.FirstOrDefault(x => x.DocumentId == id);
            if (award == null)
            {
                return NotFound();
            }
            SmartAwardFactory factory = new SmartAwardFactory(_repository, award);
            SmartAwardViewModel vm = factory.GetViewModelFromXML();
            ViewData["Title"] = "Edit Award Form";
            return View(vm);

        }
        /// <summary>
        /// Handles the form POST from the GET/Edit view.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the document being edited.</param>
        /// <param name="form">The user-provided form data, bound to a <see cref="SmartAwardViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Edit(int id, [Bind("DocumentId,AuthorUserId,AgencyName,NomineeName,ClassTitle,Division,SelectedAward,Kind,AwardClass,AwardName,ComponentViewName,Description,HasRibbon,EligibilityConfirmationDate,StartDate,EndDate,SelectedAwardType")] SmartAwardViewModel form)
        {
            if(id != form.DocumentId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // rebuild and return VM
                ViewData["Title"] = "Edit Award Form: Error";
                form.Units = _repository.Units.ToList();
                form.Users = _repository.Users.ToList();
                return View(form);
            }
            SmartDocument awardDoc = _repository.AwardForms.FirstOrDefault(x => x.DocumentId == id);
            if (awardDoc == null)
            {
                return NotFound();
            }
            SmartAwardFactory factory = new SmartAwardFactory(_repository, awardDoc);
            factory.UpdateAwardForm(form);
            return RedirectToAction("SaveSuccess", new { id = factory.awardForm.DocumentId });

        }
        /// <summary>
        /// Shows the "SaveSuccess" View
        /// </summary>
        /// <param name="id">The id of the successfully saved <see cref="SmartDocument"/></param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult SaveSuccess(int id)
        {
            
            // this is a simple view, so use VB instead of a VM            
            ViewBag.SmartDocumentId = id;
            ViewBag.FileName = _repository.AwardForms.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }
        /// <summary>
        /// Returns a View Component to prompt the User for information specific to a type of award.
        /// </summary>
        /// <param name="awardId">The id of the award</param>
        /// <returns>An <see cref="IActionResult"/></returns>
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
        /// <summary>
        /// Returns the GET/Delete view to prompt a user to confirm that they want to delete an award.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the award document to be deleted.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Delete(int id)
        {            
            // retrieve the SmartDoc from the repo
            var toDelete = _repository.AwardForms.FirstOrDefault(m => m.DocumentId == id);

            if (toDelete == null)
            {
                // SmartDoc could not be found or is not award type.
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            ViewData["Title"] = "Delete Award Form";
            return View(toDelete);
        }
        /// <summary>
        /// Deletes a document.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the document to be deleted.</param>
        /// <returns>A <see cref="RedirectToActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartDoc from the repo
            var toDelete = _repository.AwardForms.FirstOrDefault(x => x.DocumentId == id);
            // invoke the repo method to remove the SmartDoc
            if (toDelete.Type == SmartDocument.SmartDocumentType.AwardForm)
            {
                _repository.RemoveSmartDoc(toDelete);
            }            
            // redirect to the Index
            return RedirectToAction("Index", "Home");
        }
    }
}
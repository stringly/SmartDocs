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
    public class SmartJobDescriptionController : Controller
    {
        private IDocumentRepository _repository;

        public SmartJobDescriptionController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        [HttpGet]
        public IActionResult Create()
        {
            int UserId = 0;
            if (User.HasClaim(x => x.Type == "UserId"))
            {
                UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                // create a new, empty ViewModel
                SmartJobDescriptionViewModel vm = new SmartJobDescriptionViewModel
                {
                    // populate the ViewModel's lists that serve the <selects> on the form
                    JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                    Users = _repository.Users.Select(x => new UserListItem(x)).ToList(),
                    Components = _repository.Components.ToList(),
                    // default the "Author" <select> with the session user
                    AuthorUserId = UserId
                };
                ViewData["Title"] = "Create Job Description";
                return View(vm);
            }
            else
            {
                return RedirectToAction("Access Denied", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(
            "FirstName," +
            "LastName," +
            "DepartmentIdNumber," +
            "PositionNumber," +
            "DepartmentDivision," +
            "DepartmentDivisionCode," +
            "WorkPlaceAddress," +
            "AuthorUserId," +
            "SupervisedByEmployee," +
            "JobId"
            )] SmartJobDescriptionViewModel form)
        {
            if (!ModelState.IsValid)
            {
                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                // return the View with the validation messages
                ViewData["Title"] = "Create Job Description: Error";
                return View(form);
            }
            else
            {
                // validation success, create new generator and pass repo
                SmartJobDescriptionFactory factory = new SmartJobDescriptionFactory(_repository);
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    form.job = job;
                }
                else
                {
                    return NotFound();
                }

                // call generator method to pass form data
                factory.CreateSmartJobDescription(form);
                // redirect to success view with PPA as querystring param
                return RedirectToAction("SaveSuccess", new { id = factory._jobDescription.DocumentId });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            SmartDocument smartJob = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            SmartJobDescriptionFactory factory = new SmartJobDescriptionFactory(_repository, smartJob);
            SmartJobDescriptionViewModel vm = factory.GetViewModelFromXML();
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            vm.Components = _repository.Components.ToList();

            // return the view
            ViewData["Title"] = "Edit Job Description";
            return View(vm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
            "DocumentId," +
            "FirstName," +
            "LastName," +
            "DepartmentIdNumber," +
            "PositionNumber," +
            "DepartmentDivision," +
            "DepartmentDivisionCode," +
            "WorkPlaceAddress," +
            "AuthorUserId," +
            "SupervisedByEmployee," +
            "JobId"
            )] SmartJobDescriptionViewModel form)
        {
            // if the querystring parameter id doesn't match the POSTed DcocumentId, return 404
            if (id != form.DocumentId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // Model Validation failed
                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                ViewData["Title"] = "Edit Job Description: Error";
                return View(form);
            }
            else
            {
                // validation success, create new generator and pass repo
                SmartJobDescriptionFactory factory = new SmartJobDescriptionFactory(_repository);
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    form.job = job;
                }
                else
                {
                    return NotFound();
                }

                // call generator method to pass form data
                factory.UpdateSmartJobDescription(form);
                // redirect to success view with PPA as querystring param
                return RedirectToAction("SaveSuccess", new { id = factory._jobDescription.DocumentId });
            }
        }
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM            
            ViewBag.SmartJobDescriptionId = id;
            ViewBag.FileName = _repository.Documents.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }

        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            // retrieve the SmartJob from the repo
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            // return the ViewComponent
            return ViewComponent("JobDescriptionDetail", job);
        }

        public IActionResult Delete(int? id)
        {
            // query string is empty, return 404
            if (id == null)
            {
                return NotFound();
            }
            // retrieve the SmartPPA from the repo
            var smartJob = _repository.Documents.FirstOrDefault(m => m.DocumentId == id);

            if (smartJob == null)
            {
                // no SmartPPA could be found with the provided id
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            ViewData["Title"] = "Delete Job Description";
            return View(smartJob);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var smartJob = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            // invoke the repo method to remove the SmartPPA
            _repository.RemoveSmartDoc(smartJob);
            // redirect to the Index
            return RedirectToAction("Index", "Home");
        }
    }
}
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
    /// <summary>
    /// Controller class that handles interactions to create, edit, and delete Job Description Forms that are served on an employee.
    /// </summary>
    [Authorize(Policy = "IsUser")]
    public class SmartJobDescriptionController : Controller
    {
        private IDocumentRepository _repository;
        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public SmartJobDescriptionController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        /// <summary>
        /// GET: SmartJobDescription/Create view that allows a user to create a new form to serve.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        public IActionResult Create()
        {            
            int UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
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
        /// <summary>
        /// POST: SmartJobDescription/Create handles the POSTed form data and creates a new document.
        /// </summary>
        /// <param name="form">The user-provided form data, mapped to a <see cref="SmartJobDescriptionViewModel"/></param>
        /// <returns>A <see cref="IActionResult"/></returns>
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
        /// <summary>
        /// GET: SmartJobDescription/Edit?{0} view that allows a user to edit an existing Job Description Form.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the Job Description form to edit.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Edit(int id)
        {
            SmartDocument smartJob = _repository.JobDescriptionForms.FirstOrDefault(x => x.DocumentId == id);
            SmartJobDescriptionFactory factory = new SmartJobDescriptionFactory(_repository, smartJob);
            SmartJobDescriptionViewModel vm = factory.GetViewModelFromXML();
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            vm.Components = _repository.Components.ToList();

            // return the view
            ViewData["Title"] = "Edit Job Description";
            return View(vm);

        }
        /// <summary>
        /// POST: SmartJobDescription/Edit?{0} handles the POSTed form data and updates the document.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the Job Description form being edited.</param>
        /// <param name="form">The user-provided form data, bound to a <see cref="SmartJobDescriptionViewModel"/></param>        
        /// <returns>A <see cref="IActionResult"/></returns>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Edit(int id, [Bind(
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
        /// <summary>
        /// Shows the "SaveSuccess" View
        /// </summary>
        /// <param name="id">The id of the successfully saved <see cref="SmartDocument"/></param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM            
            ViewBag.SmartJobDescriptionId = id;
            ViewBag.FileName = _repository.JobDescriptionForms.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }
        /// <summary>
        /// Returns a View Component that displays Job Description details.
        /// </summary>
        /// <param name="jobId">The <see cref="SmartJob.JobId"/> of Job Description.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [AllowAnonymous]
        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            // retrieve the SmartJob from the repo
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            // return the ViewComponent
            return ViewComponent("JobDescriptionDetail", job);
        }
        /// <summary>
        /// GET: SmartJobDescription/Delete?id={0} view which Prompts the User to confirm that they want to delete a Job Description Form.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the Job Description Form to be deleted.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Delete(int? id)
        {
            // query string is empty, return 404
            if (id == null)
            {
                return NotFound();
            }
            // retrieve the SmartDoc from the repo
            var smartJob = _repository.JobDescriptionForms.FirstOrDefault(m => m.DocumentId == id);

            if (smartJob == null)
            {
                // no SmartDoc could be found with the provided id
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            ViewData["Title"] = "Delete Job Description";
            return View(smartJob);
        }

        /// <summary>
        /// Deletes a Job Description Form and redirects the user.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the Job Description Form to delete.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var smartJob = _repository.JobDescriptionForms.FirstOrDefault(x => x.DocumentId == id);
            if (smartJob != null)
            {
                // invoke the repo method to remove the SmartPPA
                _repository.RemoveSmartDoc(smartJob);
            }            
            // redirect to the Index
            return RedirectToAction("Index", "Home");
        }
    }
}
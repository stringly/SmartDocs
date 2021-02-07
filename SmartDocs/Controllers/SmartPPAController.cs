using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="SmartDocument.SmartDocumentType.PPA"/> interactions
    /// </summary>    
    [Authorize(Policy = "IsUser")]
    public class SmartPPAController : Controller
    {        
        private IDocumentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartPPAController"/> class.
        /// </summary>
        /// <remarks>
        /// This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="Startup.ConfigureServices"/>
        /// </remarks>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
        public SmartPPAController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Shows the view to create a new SmartPPA.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/></returns>
        public ActionResult Create()
        {
            int UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
            // create a new, empty ViewModel
            PPAFormViewModel vm = new PPAFormViewModel
            {
                // populate the ViewModel's lists that serve the <selects> on the form
                JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                Users = _repository.Users.Select(x => new UserListItem(x)).ToList(),
                Components = _repository.Components.ToList(),
                // default the "Author" <select> with the session user
                AuthorUserId = UserId
            };
            ViewData["Title"] = "Create PPA";
            return View(vm);
        }

        /// <summary>
        /// Creates a <see cref="SmartDocument.SmartDocumentType.PPA"/> from the POSTed form data.
        /// </summary>
        /// <param name="form">The POSTed form data, bound to a <see cref="PPAFormViewModel"/></param>
        /// <returns>An <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(
            "FirstName," +
            "LastName," +
            "DepartmentIdNumber," +
            "PayrollIdNumber," +
            "PositionNumber," +
            "DepartmentDivision," +
            "DepartmentDivisionCode," +
            "WorkPlaceAddress," +
            "AuthorUserId," +
            "SupervisedByEmployee," +
            "StartDate," +
            "EndDate," +
            "JobId," +
            "Categories," +
            "Assessment," +
            "Recommendation")] PPAFormViewModel form)
        {
            if (!ModelState.IsValid)
            {
                // AS OF VERSION 1.1: Validation occurs clientside via jquery.validate
                // Model Validation failed, so I need to re-constitute the VM with the Job and the selected categories
                // This is done very clumsily, but I'm lazy...
                // first, reform the VM JobDescription member from the JobId in the submitted form data
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    // next, loop through the submitted form categories and assign the JobDescription member's selected scores
                    for (int i = 0; i < job.Categories.Count(); i++)
                    {
                        job.Categories[i].SelectedScore = form.Categories[i]?.SelectedScore ?? 0;
                    }
                    form.Job = job;
                }

                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                // return the View with the validation messages
                ViewData["Title"] = "Create PPA: Error";
                return View(form);
            }
            else
            {
                // validation success, create new generator and pass repo
                SmartPPAFactory factory = new SmartPPAFactory(_repository);
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    // next, loop through the submitted form categories and assign the JobDescription member's selected scores
                    for (int i = 0; i < job.Categories.Count(); i++)
                    {
                        job.Categories[i].SelectedScore = form.Categories[i]?.SelectedScore ?? 0;
                    }
                    form.Job = job;
                }
                else
                {
                    return NotFound();
                }
                
                // call generator method to pass form data
                factory.CreatePPA(form);
                // redirect to success view with PPA as querystring param
                return RedirectToAction("SaveSuccess", new { id = factory.PPA.DocumentId });
            }
        }

        /// <summary>
        /// View that shows success message and allows user to download file or navigate back to MyDocuments.
        /// </summary>
        /// <remarks>This method displays a link that invokes the <see cref="HomeController.Download(int)"/> method.</remarks>
        /// <param name="id">The id of the newly generated <see cref="SmartDocument.SmartDocumentType.PPA"/></param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM            
            ViewBag.PPAId = id;
            ViewBag.FileName = _repository.PerformanceAppraisalForms.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }

        /// <summary>
        /// GET: SmartPPA/Edit?id={0} returns a view that allows a user to edit and existing PPA. 
        /// </summary>
        /// <param name="id">The identifier of the <see cref="SmartDocument.SmartDocumentType.PPA"/> to edit.</param>
        /// <returns>An <see cref="ActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public ActionResult Edit(int id)
        {
            // pull the PPA from the repo
            SmartDocument ppa = _repository.PerformanceAppraisalForms.FirstOrDefault(x => x.DocumentId == id);
            SmartPPAFactory factory = new SmartPPAFactory(_repository, ppa);
            // pass the PPA to the factory method takes a SmartPPA parameter
            PPAFormViewModel vm = factory.GetViewModelFromXML();
            // populate the VM <select> lists
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            vm.Components = _repository.Components.ToList();
            // return the view
            ViewData["Title"] = "Edit PPA";
            return View(vm);
        }

        /// <summary>
        /// POST: SmartPPA/Edit?=""
        /// </summary>
        /// <param name="id">The identifier for the <see cref="SmartDocument.SmartDocumentType.PPA"/> to be edited.</param>
        /// <param name="form">The POSTed form data, bound to a <see cref="PPAFormViewModel"/>.</param>
        /// <returns>An <see cref="ActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(
            "DocumentId," +
            "FirstName," +
            "LastName," +
            "DepartmentIdNumber," +
            "PayrollIdNumber," +
            "PositionNumber," +
            "DepartmentDivision," +
            "DepartmentDivisionCode," +
            "WorkPlaceAddress," +
            "AuthorUserId," +
            "SupervisedByEmployee," +
            "StartDate," +
            "EndDate," +
            "JobId," +
            "Categories," +
            "Assessment," +
            "Recommendation")] PPAFormViewModel form)
        {
            // if the querystring parameter id doesn't match the POSTed PPAId, return 404
            if (id != form.DocumentId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // Model Validation failed, so I need to re-constitute the VM with the Job and the selected categories
                // This is done very clumsily, but I'm lazy...
                // first, reform the VM JobDescription member from the JobId in the submitted form data
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    // next, loop through the submitted form categories and assign the JobDescription member's selected scores
                    for (int i = 0; i < job.Categories.Count(); i++)
                    {
                        job.Categories[i].SelectedScore = form.Categories[i]?.SelectedScore ?? 0;
                    }
                    form.Job = job;
                }
                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                ViewData["Title"] = "Edit PPA: Error";
                return View(form);
            }
            else
            {
                if (form.JobId != 0)
                {
                    JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == form.JobId));
                    // next, loop through the submitted form categories and assign the JobDescription member's selected scores
                    for (int i = 0; i < job.Categories.Count(); i++)
                    {
                        job.Categories[i].SelectedScore = form.Categories[i]?.SelectedScore ?? 0;
                    }
                    form.Job = job;
                }
                else
                {
                    return NotFound();
                }
                // validation success, create a new PPAGenerator and pass the repo as a parameter
                SmartPPAFactory factory = new SmartPPAFactory(_repository);
                // populate the form info into the generator
                factory.UpdatePPA(form);
                // redirect to the SaveSuccess view, passing the newly created PPA as a querystring param
                return RedirectToAction("SaveSuccess", new { id = factory.PPA.DocumentId });
            }
        }

        /// <summary>
        /// Shows the view to confirm deletion of a <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.PPA"/>.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the <see cref="SmartDocument.SmartDocumentType.PPA"/> to be deleted.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        public IActionResult Delete(int id)
        {
            // retrieve the SmartPPA from the repo
            var smartPPA = _repository.PerformanceAppraisalForms.FirstOrDefault(m => m.DocumentId == id);

            if (smartPPA == null)
            {
                // no SmartPPA could be found with the provided id
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            ViewData["Title"] = "Delete PPA";
            return View(smartPPA);
        }


        /// <summary>
        /// Deletes <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.PPA"/> from the database.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the <see cref="SmartDocument.SmartDocumentType.PPA"/>> to be deleted.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditDocument")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var smartPPA = _repository.PerformanceAppraisalForms.FirstOrDefault(x => x.DocumentId == id);
            // invoke the repo method to remove the SmartPPA
            _repository.RemoveSmartDoc(smartPPA);
            // redirect to the Index
            return RedirectToAction("Index","Home");
        }

        /// <summary>
        /// Determines if a <see cref="SmartDocument"/> of the type <see cref="SmartDocument.SmartDocumentType.PPA"/> with a given <see cref="SmartDocument.DocumentId"/> exists.
        /// </summary>
        /// <param name="id">The <see cref="SmartDocument.DocumentId"/> of the <see cref="SmartDocument.SmartDocumentType.PPA"/>.</param>
        /// <returns></returns>
        private bool SmartPPAExists(int id)
        {            
            return _repository.PerformanceAppraisalForms.Any(e => e.DocumentId == id);
        }

        /// <summary>
        /// Gets a <see cref="ViewComponents.JobDescriptionCategoryListViewComponent"/> for a specified JobId.
        /// </summary>
        /// <param name="jobId">The <see cref="SmartJob.JobId"/> of a <see cref="SmartJob"/>.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [AllowAnonymous]
        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            // retrieve the SmartJob from the repo
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            // return the ViewComponent
            return ViewComponent("JobDescriptionCategoryList", job);
        }
    }
}
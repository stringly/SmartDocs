using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="T:SmartDocs.Models.SmartPPA"/> interactions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class SmartPPAController : Controller
    {
        private IDocumentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.SmartPPAController"/> class.
        /// </summary>
        /// <remarks>
        /// This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="M:SmartDocs.Startup.ConfigureServices"/>
        /// </remarks>
        /// <param name="repo">An <see cref="T:SmartDocs.Models.IDocumentRepository"/></param>
        public SmartPPAController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Shows a view with a list of all <see cref="T:SmartDocs.Models.SmartPPA"/> in the DB.
        /// </summary>
        /// <remarks>
        /// The list returned from this method is created via <see cref="M:SmartDocs.Models.SmartDocumentRepository.PPAs"/>,
        /// which limits the result set to the <see cref="T:SmartDocs.Models.SmartPPA"/> objects authored by the
        /// <see cref="M:SmartDocs.Models.SmartDocumentRepository.GetCurrentUser"/>
        /// </remarks>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Index()
        {
            // Check if user is known to Windows Auth
            if (User.Identity.IsAuthenticated)
            {
                // create a new view model
                DocumentListViewModel vm = new DocumentListViewModel();
                // assign the Documents property of the viewmodel to the a list of DocumentListViewModelItems
                // that is created by passing each of the repository's PPAs to the DocumentListViewModelItem
                // constructor that takes a SmartPPA parameter
                vm.Documents = _repository.PPAs.Select(x => new DocumentListViewModelItem(x)).ToList();
                return View(vm);
            }
            else
            {
                // TODO: if windows auth fails, the application will return a 401 long before they get here? Shouldn't this check against a null _repo.GetCurrentUser()?
                return RedirectToAction("Access Denied", "Home");
            }
        }

        /// <summary>
        /// Downloads an existing SmartPPA.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartPPA"/> to be downloaded</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.FileStreamResult"/></returns>
        public ActionResult Download(int id)
        {
            // create a generator, passing the repository as a parameter
            SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
            // call the ReDownload method with the querystring id parameter
            generator.ReDownloadPPA(id);
            // set the FileResult name 
            string resultDocName = generator.dbPPA.DocumentName;
            // return the FileResult to the client
            return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
        }

        /// <summary>
        /// Shows the view to create a new SmartPPA.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public ActionResult Create()
        {
            // create a new, empty ViewModel
            PPAFormViewModel vm = new PPAFormViewModel
            {
                // populate the ViewModel's lists that serve the <selects> on the form
                JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                Users = _repository.Users.Select(x => new UserListItem(x)).ToList(),                               
                Components = _repository.Components.ToList(),
                // default the "Author" <select> with the session user
                AuthorUserId = _repository.GetCurrentUser().UserId
            };
            return View(vm);
        }

        /// <summary>
        /// Creates a <see cref="T:SmartDocs.Models.SmartPPA"/> from the POSTed form data.
        /// </summary>
        /// <param name="form">The POSTed form data, bound to a <see cref="T:SmartDocs.Models.ViewModels.PPAFormViewModel"/></param>
        /// <returns></returns>
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
                    form.job = job;
                }

                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                // return the View with the validation messages
                return View(form);
            }
            else
            {
                // validation success, create new generator and pass repo
                SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
                // call generator method to pass form data
                generator.SeedFormInfo(form);
                // redirect to success view with PPA as querystring param
                return RedirectToAction("SaveSuccess", new { id = generator.dbPPA.PPAId });
            }
        }

        /// <summary>
        /// View that shows success message and allows user to download file or navigate back to MyDocuments.
        /// </summary>
        /// <remarks>This method displays a link that invokes the <see cref="M:SmartDocs.Controllers.SmartPPAController.Download(int)"/> method.</remarks>
        /// <param name="id">The id of the newly generated <see cref="T:SmartDocs.Models.SmartPPA"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM            
            ViewBag.PPAId = id;
            ViewBag.FileName = _repository.PPAs.FirstOrDefault(x => x.PPAId == id).DocumentName;
            return View();

        }

        /// <summary>
        /// GET: SmartPPA/Edit?id="" 
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartPPA"/> to edit.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public ActionResult Edit(int id)
        {
            // pull the PPA from the repo
            SmartPPA ppa = _repository.PPAs.FirstOrDefault(x => x.PPAId == id);
            // pass the PPA to the VM constructor that takes a SmartPPA parameter
            PPAFormViewModel vm = new PPAFormViewModel(ppa);
            // populate the VM <select> lists
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            vm.Components = _repository.Components.ToList();
            // return the view
            return View(vm);
        }

        /// <summary>
        /// POST: SmartPPA/Edit?=""
        /// </summary>
        /// <param name="id">The identifier for the <see cref="T:SmartDocs.Models.SmartPPA"/> to be edited.</param>
        /// <param name="form">The POSTed form data, bound to a <see cref="T:SmartDocs.Models.ViewModels.PPAFormViewModel"/>.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,[Bind(
            "PPAId," +
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
            if (id != form.PPAId)
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
                    form.job = job;
                }
                // next, re-populate the VM drop down lists
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                form.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
                form.Components = _repository.Components.ToList();
                return View(form);
            }
            else
            {
                // validation success, create a new PPAGenerator and pass the repo as a parameter
                SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
                // populate the form info into the generator
                generator.SeedFormInfo(form);
                // redirect to the SaveSuccess view, passing the newly created PPA as a querystring param
                return RedirectToAction("SaveSuccess", new { id = generator.dbPPA.PPAId });
            }
        }

        /// <summary>
        /// Shows the view to confirm deletion of a <see cref="T:SmartDocs.Models.SmartPPA"/>.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartPPA"/>.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            // query string is empty, return 404
            if (id == null)
            {
                return NotFound();
            }
            // retrieve the SmartPPA from the repo
            var smartPPA = _repository.PPAs.FirstOrDefault(m => m.PPAId == id);

            if (smartPPA == null)
            {
                // no SmartPPA could be found with the provided id
                return NotFound();
            }
            // return the View (which uses the Domain Object as a model)
            return View(smartPPA);
        }


        /// <summary>
        /// Deletes <see cref="T:SmartDocs.Models.SmartPPA"/> with the provided ID.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartPPA"/> to be deleted.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var smartPPA = _repository.PPAs.FirstOrDefault(x => x.PPAId == id);
            // invoke the repo method to remove the SmartPPA
            _repository.RemoveSmartPPA(smartPPA);
            // redirect to the Index
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a <see cref="T:SmartDocs.Models.SmartPPA"/> with a given id exists in the repo.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartPPA"/>.</param>
        /// <returns></returns>
        private bool SmartPPAExists(int id)
        {            
            return _repository.PPAs.Any(e => e.PPAId == id);
        }

        /// <summary>
        /// Gets the <see cref="T:SmartDocs.ViewComponents.JobDescriptionCategoryListViewComponent"/> for a specified JobId.
        /// </summary>
        /// <param name="jobId">The identifier of a <see cref="T:SmartDocs.Models.SmartJob"/>.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            // retrieve the SmartJob from the repo
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            // return the ViewComponent
            return ViewComponent("JobDescriptionCategoryList", job);
        }

        /// <summary>
        /// View that is returned when a user attempts an action for which they are not authorized.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult NotAuthorized()
        {
            ViewBag.Message = "You are not authorized to take this action";
            return View();
        }
    }
}
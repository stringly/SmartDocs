using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for "Job Description" views interactions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class JobDescriptionController : Controller
    {        
        private IDocumentRepository _repository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JobDescriptionController"/> class.
        /// </summary>
        /// <remarks>This controller requires the Hosting Environment and a Repository to be injected when it is created. Refer to middleware in <see cref="M:SmartDocs.Startup.ConfigureServices"/></remarks>
        /// <param name="hostingEnvironment">An <see cref="T:IHostingEnvironment"/></param>
        /// <param name="repo">An <see cref="T:SmartDocs.Models.IDocumentRepository"/></param>
        public JobDescriptionController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Shows the Index view.
        /// </summary>
        /// <remarks>This is a list of all the current Job Descriptions in the database. It includes links to Add/Edit/Delete. This is an administrative view.
        /// A User-viewable list of Jobs is returned by <see cref="T:SmartDocs.Models.UserIndex"/>
        /// </remarks>
        /// <seealso cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModel"/>
        /// <seealso cref="T:SmartDocs.Models.Types.JobDescriptionListViewModelItem"/>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Index()
        {            
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel
            {
                Jobs = _repository.Jobs.Select(x => new JobDescriptionListViewModeltem(x)).ToList()
            };
            return View(vm);
        }

        /// <summary>
        /// Shows the User Index view.
        /// </summary>
        /// <remarks>This shows a list of all current Job Descriptions in the database. It is accessible to non-admin users.</remarks>'
        /// <seealso cref="T:SmartDocs.Models.ViewModels.JobDescriptionListViewModel"/>
        /// <seealso cref="T:SmartDocs.Models.Types.JobDescriptionListViewModelItem"/>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult UserIndex()
        {
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel
            {
                Jobs = _repository.Jobs.Select(x => new JobDescriptionListViewModeltem(x)).ToList()
            };

            return View(vm);
        }

        /// <summary>
        /// GET: JobDescription/Edit?id=""
        /// </summary>
        /// <remarks>Shows a view to allow editing an existing Job Description.</remarks>
        /// <param name="id">The id of the <see cref="T:SmartDocs.Models.SmartJob"/> to edit.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Edit(int id)
        {
            SmartJob job = _repository.Jobs.FirstOrDefault(j => j.JobId == id);
            JobDescriptionViewModel vm = new JobDescriptionViewModel(job);
            return View(vm);
        }

        /// <summary>
        /// POST: JobDescription/Edit
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartJob"/> that the user is attempting to edit.</param>
        /// <param name="form">The POSTed form data bound to a <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("JobId,WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (id != form.JobId)
            {
                // the JobId in the form POST does not match the Querystring ID Parameter
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                // model binding validation failed, return the VM to the view with validation messages
                return View(form);
            }
            else
            {
                // POST is valid

                // create a JobDescription Object via the constructor that accepts a JobDescriptionViewModel object
                // I need to do this because I need the .JobDescriptionToXml() method to write the Job Description data 
                // to the SmartJob entity JobData field, which is XML.
                JobDescription job = new JobDescription(form);  
                SmartJob DbJob = new SmartJob
                {
                    JobId = id,
                    JobName = $"{job.Rank}-{job.WorkingTitle}",
                    JobDataXml = job.JobDescriptionToXml() // call the JobDescription method to convert Job Data to XML and write to entity column
                    
                };
                // EF context saves are encapsulated in the IDocumentRepository instance                
                _repository.SaveJob(DbJob);
                // return the user to the Index view
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// GET: JobDescription/Create
        /// </summary>
        /// <remarks>Shows a view to allow creation of a Job Description.</remarks>        
        /// <seealso cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Create()
        {
            JobDescriptionViewModel vm = new JobDescriptionViewModel();
            return View(vm);
        }

        /// <summary>
        /// POST: JobDescription/Create
        /// </summary>        
        /// <param name="form">The POSTed form data bound to a <see cref="T:SmartDocs.Models.ViewModels.JobDescriptionViewModel"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (!ModelState.IsValid)
            {
                // model state validation failed, return VM to user with validation error messages
                return View(form);
            }
            else
            {
                // POST is valid

                // create a JobDescription Object via the constructor that accepts a JobDescriptionViewModel object
                // I need to do this because I need the .JobDescriptionToXml() method to write the Job Description data 
                // to the SmartJob entity JobData field, which is XML.
                JobDescription job = new JobDescription(form);                
                SmartJob DbJob = new SmartJob
                {
                    JobName = $"{job.Rank}-{job.WorkingTitle}",
                    JobDataXml = job.JobDescriptionToXml() // call the JobDescription method to convert Job Data to XML and write to entity column

                };
                // EF context saves are encapsulated in the IDocumentRepository instance  
                _repository.SaveJob(DbJob);
                // return the user to the Index view
                return RedirectToAction(nameof(Index));
            }
        }
        /// <summary>
        /// Shows the Details view for a Job Description.
        /// </summary>
        /// <remarks>This view shows full details for a Job Description. It only displays the information; no CRUD is available via this view.</remarks>
        /// <param name="id">The identifier of the SmartJob</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult ShowJobDetails(int? id)
        {
            if (id == null)
            {
                // querystring param is null
                return NotFound();
            }
            // find the SmartJob entity with the parameter id
            var SmartJob = _repository.Jobs.FirstOrDefault(j => j.JobId == id);
            if (SmartJob == null)
            {
                // no job with the provided id exists in the DB
                return NotFound();
            }
            // this should probably be a VM, but it just displays the info and has no interactive elements,
            // so the View uses the Models.JobDescription as a model
            // the Models.JobDescription object has a constructor that takes a Models.SmartJob Object as a param
            JobDescription vmJob = new JobDescription(SmartJob);
            return View(vmJob);
        }

        /// <summary>
        /// Show the default Details page for the Job Description Object.
        /// </summary>
        /// <remarks>This is deprecated.</remarks>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.JobDescription"/> .</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SmartJob = _repository.Jobs.FirstOrDefault( j => j.JobId == id);
            if (SmartJob == null)
            {
                return NotFound();
            }
            JobDescription vmJob = new JobDescription(SmartJob);
            return View(vmJob);
        }

        /// <summary>
        /// GET: JobDescription/Delete?id=""
        /// </summary>
        /// <remarks>This shows a view that asks the user to confirm deletion of the Job Description</remarks>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartJob"/> to be deleted.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SmartJob = _repository.Jobs
                .FirstOrDefault(m => m.JobId == id);
            if (SmartJob == null)
            {
                return NotFound();
            }
            if (_repository.GetCurrentUser().UserId != 1)
            {
                return RedirectToAction("AccessDenied", "Home", new { area = "" });
            }

            return View(SmartJob);
        }

        /// <summary>
        /// POST: JobDescription/Delete
        /// </summary>
        /// <remarks>Deletes the <see cref="T:SmartDocs.Models.SmartJob"/> with the provided id.</remarks>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartJob"/> to be deleted.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var smartJob = _repository.Jobs.FirstOrDefault(x => x.JobId == id);
            if (smartJob == null)
            {
                // no Job with the provided Id exists in the DB
                return NotFound();
            }
            // context updates are encapsulated in the repository
            _repository.RemoveJob(smartJob);
            // redirect to the Index
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a <see cref="T:SmartDocs.Models.SmartUser"/> with the provided id exists in the DB.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartUser"/></param>
        /// <returns>True if the user exists, otherwise false</returns>
        private bool SmartJobExists(int id)
        {
            return _repository.Jobs.Any(e => e.JobId == id);
        }
    }
}
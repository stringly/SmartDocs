using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for "Job Description" views interactions
    /// </summary>
    /// <seealso cref="Controller" />    
    public class JobDescriptionController : Controller
    {
        private IDocumentRepository _repository;
        private int PageSize = 15;

        /// <summary>
        /// Initializes a new instance of the <see cref="JobDescriptionController"/> class.
        /// </summary>
        /// <remarks>This controller requires the Hosting Environment and a Repository to be injected when it is created. Refer to middleware in <see cref="Startup.ConfigureServices"/></remarks>
        /// <param name="repo">An <see cref="IDocumentRepository"/></param>
        public JobDescriptionController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Returns an Index list of all Job Descriptions in the database.
        /// </summary>
        /// <remarks>
        /// This view includes links to Create/Edit/Delete Job Descriptions, and is an administrator-level view. Users can see the Index via GET/UserIndex
        /// </remarks>
        /// <param name="sortOrder">An optional string to sort the result list.</param>
        /// <param name="searchString">An optional string to search and filter the Job Description Names in the result list.</param>
        /// <param name="SelectedRank">An optional string parameter that correspondes to a rank as it appears in the XMLData of a Job Description.</param>
        /// <param name="SelectedGrade">An optional string parameter that correspondes to a grade as it appears in the XMLData of a Job Description.</param>
        /// <param name="page">An optional paging index. Defaults to 1.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Index(string sortOrder, string searchString, string SelectedRank, string SelectedGrade, int page = 1)
        {
            // init the VM class
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel
            {
                // set the VM properties to the parameters
                CurrentFilter = searchString,
                CurrentSort = sortOrder,
                SelectedGrade = SelectedGrade,
                SelectedRank = SelectedRank,
                // set the sorts to the opposite of the current sortOrder to facilitate building sorting hyperlinks
                GradeSort = String.IsNullOrEmpty(sortOrder) ? "grade_desc" : "",
                NameSort = sortOrder == "JobName" ? "jobName_desc" : "JobName",
                RankSort = sortOrder == "Rank" ? "rank_desc" : "Rank"
            };
            // lower any search string text to facilitate comparison.
            string lowerSearchString = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                lowerSearchString = new string(arr);
                lowerSearchString = lowerSearchString.ToLower();
            }
            // Query for documents that match the provided parameters, if any, and convert them to List Items
            vm.Jobs = _repository.Jobs.Where(x => (String.IsNullOrEmpty(SelectedRank) || x.JobDataXml.Element("Rank").Value == SelectedRank)
                && (String.IsNullOrEmpty(SelectedGrade) || x.JobDataXml.Element("Grade").Value == SelectedGrade)
                && (String.IsNullOrEmpty(searchString) || x.JobName.ToLower().Contains(lowerSearchString)))
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList().ConvertAll(x => new JobDescriptionListViewModeltem(x));
            // Separate Query to determine total number of matching documents, as the list above is Skip().Take()
            int totalItems = _repository.Jobs.Where(x => (String.IsNullOrEmpty(SelectedRank) || x.JobDataXml.Element("Rank").Value == SelectedRank)
                && (String.IsNullOrEmpty(SelectedGrade) || x.JobDataXml.Element("Grade").Value == SelectedGrade)
                && (String.IsNullOrEmpty(searchString) || x.JobName.ToLower().Contains(lowerSearchString)))
                .Count();
            // Sort the list according to the provided sorting parameter, if any. Defaults to order by ascending Grade if no sorting parameter is present.
            switch (sortOrder)
            {
                case "grade_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.Grade).ToList();
                    break;
                case "jobName_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.JobName).ToList();
                    break;
                case "JobName":
                    vm.Jobs = vm.Jobs.OrderBy(x => x.JobName).ToList();
                    break;
                case "rank_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.Rank).ToList();
                    break;
                case "Rank":
                    vm.Jobs = vm.Jobs.OrderBy(x => x.Rank).ToList();
                    break;
                default:
                    vm.Jobs = vm.Jobs.OrderBy(x => x.Grade).ToList();
                    break;
            }
            // Set the PagingInfo to facilitate page control
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = totalItems,
                ItemsPerPage = PageSize
            };
            // Populate the VM's lists to populate the search form drop downs
            vm.HydrateLists(_repository.Jobs.Select(x => x.JobDataXml.Element("Rank").Value).Distinct().ToList(), _repository.Jobs.Select(x => x.JobDataXml.Element("Grade").Value).Distinct().ToList());
            ViewData["Title"] = "Job Descriptions List";
            ViewData["ActiveNavBarMenuLink"] = "Job Descriptions";
            return View(vm);
        }

        /// <summary>
        /// Returns an Index list of all Job Descriptions in the database.
        /// </summary>
        /// <remarks>
        /// This view includes a link to view the details of a Job Description.
        /// </remarks>
        /// <param name="sortOrder">An optional string to sort the result list.</param>
        /// <param name="searchString">An optional string to search and filter the Job Description Names in the result list.</param>
        /// <param name="SelectedRank">An optional string parameter that correspondes to a rank as it appears in the XMLData of a Job Description.</param>
        /// <param name="SelectedGrade">An optional string parameter that correspondes to a grade as it appears in the XMLData of a Job Description.</param>
        /// <param name="page">An optional paging index. Defaults to 1.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [AllowAnonymous]
        public IActionResult UserIndex(string sortOrder, string searchString, string SelectedRank, string SelectedGrade, int page = 1)
        {
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel();
            vm.CurrentFilter = searchString;
            vm.CurrentSort = sortOrder;
            vm.SelectedGrade = SelectedGrade;
            vm.SelectedRank = SelectedRank;
            vm.GradeSort = String.IsNullOrEmpty(sortOrder) ? "grade_desc" : "";
            vm.NameSort = sortOrder == "JobName" ? "jobName_desc" : "JobName";
            vm.RankSort = sortOrder == "Rank" ? "rank_desc" : "Rank";
            string lowerSearchString = "";
            if (!String.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                lowerSearchString = new string(arr);
                lowerSearchString = lowerSearchString.ToLower();
            }
            vm.Jobs = _repository.Jobs.Where(x => (String.IsNullOrEmpty(SelectedRank) || x.JobDataXml.Element("Rank").Value == SelectedRank)
                && (String.IsNullOrEmpty(SelectedGrade) || x.JobDataXml.Element("Grade").Value == SelectedGrade)
                && (String.IsNullOrEmpty(searchString) || x.JobName.ToLower().Contains(lowerSearchString)))
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList().ConvertAll(x => new JobDescriptionListViewModeltem(x));
            int totalItems = _repository.Jobs.Where(x => (String.IsNullOrEmpty(SelectedRank) || x.JobDataXml.Element("Rank").Value == SelectedRank)
                && (String.IsNullOrEmpty(SelectedGrade) || x.JobDataXml.Element("Grade").Value == SelectedGrade)
                && (String.IsNullOrEmpty(searchString) || x.JobName.ToLower().Contains(lowerSearchString)))
                .Count();
            switch (sortOrder)
            {
                case "grade_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.Grade).ToList();
                    break;
                case "jobName_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.JobName).ToList();
                    break;
                case "JobName":
                    vm.Jobs = vm.Jobs.OrderBy(x => x.JobName).ToList();
                    break;
                case "rank_desc":
                    vm.Jobs = vm.Jobs.OrderByDescending(x => x.Rank).ToList();
                    break;
                case "Rank":
                    vm.Jobs = vm.Jobs.OrderBy(x => x.Rank).ToList();
                    break;
                default:
                    vm.Jobs = vm.Jobs.OrderBy(x => x.Grade).ToList();
                    break;
            }
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = totalItems,
                ItemsPerPage = PageSize
            };
            vm.HydrateLists(_repository.Jobs.Select(x => x.JobDataXml.Element("Rank").Value).Distinct().ToList(), _repository.Jobs.Select(x => x.JobDataXml.Element("Grade").Value).Distinct().ToList());
            ViewData["Title"] = "Job Descriptions List";
            ViewData["ActiveNavBarMenuLink"] = "Job Descriptions";
            return View(vm);
        }
        /// <summary>
        /// Returns a view that allows a User to edit an existing Job Description
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the Job Description being edited.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Edit(int id)
        {
            SmartJob job = _repository.Jobs.FirstOrDefault(j => j.JobId == id);
            JobDescriptionViewModel vm = new JobDescriptionViewModel(job);
            ViewData["Title"] = "Edit Job Description";
            return View(vm);
        }

        /// <summary>
        /// Handles the POSTed form data from the GET/Edit view.
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the Job Description being edited.</param>
        /// <param name="form">The user-provided form data, bound to a <see cref="JobDescriptionViewModel"/></param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsGlobalAdmin")]
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
                ViewData["Title"] = "Edit Job Description: Error";
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
        /// Returns a view that allows the User to create a new Job Description.
        /// </summary>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpGet]
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Create()
        {
            JobDescriptionViewModel vm = new JobDescriptionViewModel();
            ViewData["Title"] = "Create a Job Description";
            return View(vm);
        }

        /// <summary>
        /// Handles the POSTed form data and creates a new Job Description.
        /// </summary>
        /// <param name="form">The user-provided form data, bound to a <see cref="JobDescriptionViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Create([Bind("WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (!ModelState.IsValid)
            {
                // model state validation failed, return VM to user with validation error messages
                ViewData["Title"] = "Create a Job Description: Error";
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
        /// Returns a view that shows details for a specific Job Description
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the desired Job Description.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "IsUser")]
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
            ViewData["Title"] = "Job Description Details";
            return View(vmJob);
        }

        /// <summary>
        /// Returns a view that shows details for a specific Job Description
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the desired Job Description.</param>
        /// <param name="returnUrl">An optional return Url String to aid in navigation.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "IsUser")]
        public IActionResult Details(int? id, string returnUrl)
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
            ViewData["Title"] = "Job Description Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vmJob);
        }

        /// <summary>
        /// Returns a view to prompt the user to confirm that a specific Job Description will be deleted.
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the Job Description to be deleted.</param>
        /// <returns></returns>
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Delete(int id)
        {
            // TODO: Delete/JobDescription should probably be soft delete, or documents already generated will be borked.
            var SmartJob = _repository.Jobs
                .FirstOrDefault(m => m.JobId == id);
            if (SmartJob == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Job Description";
            return View(SmartJob);
        }

        /// <summary>
        /// Deletes a Job Description and redirects the User.
        /// </summary>
        /// <param name="id">The <see cref="SmartJob.JobId"/> of the Job Description to delete.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsGlobalAdmin")]
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
        /// Determines if a <see cref="SmartJob"/> with the provided <see cref="SmartJob.JobId"/> exists in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if a <see cref="SmartJob"/> with the provided <see cref="SmartJob.JobId"/> exists, otherwise false.</returns>
        private bool SmartJobExists(int id)
        {
            return _repository.Jobs.Any(e => e.JobId == id);
        }
    }
}
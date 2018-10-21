using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SmartPPA.Models;
using SmartPPA.Models.Types;
using SmartPPA.Models.ViewModels;

namespace SmartPPA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDocumentRepository _repository;

        public HomeController( IDocumentRepository repo)
        {
            _repository = repo;
        }
        public ActionResult Choices()
        {
            SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
            generator.WriteTemplate();
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                DocumentListViewModel vm = new DocumentListViewModel();
                vm.Documents = _repository.PPAs.Select(x => new DocumentListViewModelItem(x)).ToList();
                return View(vm);
            }
            else
            {
                return RedirectToAction(nameof(AccessDenied));
            }
            
        }
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }

        // GET: Home/Details/5
       
        public ActionResult Download(int id)
        {
            SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
            generator.ReDownloadPPA(id);
            string resultDocName = generator.dbPPA.DocumentName;
            return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
        }

        // GET: Home/Create
        public ActionResult Create()
        {            
            PPAFormViewModel vm = new PPAFormViewModel {
                JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                Users = _repository.Users.Select(x => new UserListItem(x)).ToList(),
                AuthorUserId = _repository.GetCurrentUser().UserId
            };
            return View(vm);
        }

        // POST: Home/Create
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
                // Model Validation failed, so recreate the joblist and push back the VM
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                return View(form);
            }
            else
            {   // TODO: Pass User's Info here             
                SmartPPAGenerator generator = new SmartPPAGenerator(_repository);                
                generator.SeedFormInfo(form);
                string resultDocName = generator.dbPPA.DocumentName;
                return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
            }
    }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            PPAFormViewModel vm = new PPAFormViewModel(_repository.PPAs.FirstOrDefault(x => x.PPAId == id));
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            return View(vm);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(
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
            if (id != form.PPAId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    SmartPPAGenerator generator = new SmartPPAGenerator(_repository);                
                    generator.SeedFormInfo(form);
                    string resultDocName = generator.dbPPA.DocumentName;
                    return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
                
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                // Model Validation failed, so recreate the joblist and push back the VM
                form.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
                return View(form);
            }

        }

        // GET: SmartUsers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartPPA = _repository.PPAs.FirstOrDefault(m => m.PPAId == id);
            if (smartPPA == null)
            {
                return NotFound();
            }

            return View(smartPPA);
        }

        // POST: SmartUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var smartPPA = _repository.PPAs.FirstOrDefault(x => x.PPAId == id);
            _repository.RemoveSmartPPA(smartPPA);
            return RedirectToAction(nameof(Index));
        }

        private bool SmartPPAExists(int id)
        {
            return _repository.PPAs.Any(e => e.PPAId == id);
        }

        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            return ViewComponent("JobDescriptionCategoryList", job);
        }

    }
}
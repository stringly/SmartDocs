using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    [Authorize]
    public class SmartPPAController : Controller
    {
        private IDocumentRepository _repository;

        public SmartPPAController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                DocumentListViewModel vm = new DocumentListViewModel();
                vm.Documents = _repository.PPAs.Select(x => new DocumentListViewModelItem(x)).ToList();
                return View(vm);
            }
            else
            {
                return RedirectToAction("Access Denied", "Home");
            }
        }
        public ActionResult Download(int id)
        {
            SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
            generator.ReDownloadPPA(id);
            string resultDocName = generator.dbPPA.DocumentName;
            return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
        }

        
        public ActionResult Create()
        {
            PPAFormViewModel vm = new PPAFormViewModel
            {
                JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                Users = _repository.Users.Select(x => new UserListItem(x)).ToList(),
                AuthorUserId = _repository.GetCurrentUser().UserId
            };
            return View(vm);
        }

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
            {
                SmartPPAGenerator generator = new SmartPPAGenerator(_repository);
                generator.SeedFormInfo(form);
                string resultDocName = generator.dbPPA.DocumentName;
                return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
            }
        }

        public ActionResult Edit(int id)
        {
            PPAFormViewModel vm = new PPAFormViewModel(_repository.PPAs.FirstOrDefault(x => x.PPAId == id));
            vm.JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList();
            vm.Users = _repository.Users.Select(x => new UserListItem(x)).ToList();
            return View(vm);
        }

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
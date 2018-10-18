using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SmartPPA.Models;
using SmartPPA.Models.Types;
using SmartPPA.Models.ViewModels;

namespace SmartPPA.Controllers
{
    public class HomeController : Controller
    {
        // TODO: I think the repo eliminates the need for the Hosting Environment?
        private readonly IHostingEnvironment _hostingEnvironment;
        private IDocumentRepository _repository;

        public HomeController(IHostingEnvironment hostingEnvironment, IDocumentRepository repo)
        {
            _hostingEnvironment = hostingEnvironment;
            _repository = repo;
        }
        // GET: Home
        public ActionResult Index()
        {
            //ViewBag.Message = this.User.Identity.Name;
            return View();
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            // TODO: Set the "vm.AuthorUserId" to the current application user
            PPAFormViewModel vm = new PPAFormViewModel {
                JobList = _repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(),
                Users = _repository.Users.Select(x => new UserListItem(x)).ToList()
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
            "SupervisedByEmployeeName," +
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
                string resultDocName = $"{form.LastName}, {form.FirstName} {form.DepartmentIdNumber} {form.EndDate.ToString("yyyy")} Performance Appraisal.docx";
                return File(generator.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", resultDocName);
            }
    }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Home/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        public IActionResult GetJobDescriptionViewComponent(int jobId)
        {
            JobDescription job = new JobDescription(_repository.Jobs.FirstOrDefault(x => x.JobId == jobId));
            return ViewComponent("JobDescriptionCategoryList", job);
        }
    }
}
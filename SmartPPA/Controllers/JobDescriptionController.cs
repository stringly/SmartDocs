using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartPPA.Models;
using SmartPPA.Models.Types;
using SmartPPA.Models.ViewModels;

namespace SmartPPA.Controllers
{
    // TODO: Add Delete View/Method
    public class JobDescriptionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IDocumentRepository _repository;

        public JobDescriptionController(IHostingEnvironment hostingEnvironment, IDocumentRepository repo)
        {
            _hostingEnvironment = hostingEnvironment;
            _repository = repo;
        }
        public IActionResult Index()
        {
            //var list = Directory.EnumerateFiles(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions");
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel
            {
                Jobs = _repository.Jobs.Select(x => new JobDescriptionListViewModeltem(x)).ToList()
            };

            return View(vm);
        }

        // GET: JobDescription/Edit
        public IActionResult Edit(int id)
        {
            SmartJob job = _repository.Jobs.FirstOrDefault(j => j.JobId == id);
            JobDescriptionViewModel vm = new JobDescriptionViewModel(job);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (id != form.JobId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            else
            {
                JobDescription job = new JobDescription(form);
                SmartJob DbJob = new SmartJob
                {
                    JobName = $"{job.Rank}-{job.WorkingTitle}",
                    JobDataXml = job.JobDescriptionToXml()
                    
                };
                _repository.SaveJob(DbJob);
                // job.WriteJobDescriptionToXml(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions\");

                return RedirectToAction(nameof(Index));
            }
        }
        // GET: JobDescription/Create
        public IActionResult Create()
        {
            JobDescriptionViewModel vm = new JobDescriptionViewModel();
            return View(vm);
        }

        // POST: JobDescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            else
            {
                JobDescription job = new JobDescription(form);
                //job.WriteJobDescriptionToXml(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions\");
                SmartJob DbJob = new SmartJob
                {
                    JobName = $"{job.Rank}-{job.WorkingTitle}",
                    JobDataXml = job.JobDescriptionToXml()
                    
                };
                _repository.SaveJob(DbJob);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
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
    public class JobDescriptionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public JobDescriptionController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            var list = Directory.EnumerateFiles(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions");
            return View(list);
        }

        // GET: JobDescription/Edit
        public IActionResult Edit(string filePath)
        {
            JobDescriptionViewModel job = new JobDescriptionViewModel(filePath);
            return View(job);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("WorkingTitle,Grade,WorkingHours,Rank,Categories")] JobDescriptionViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            else
            {
                JobDescription job = new JobDescription(form);
                job.WriteJobDescriptionToXml(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions\");

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
                job.WriteJobDescriptionToXml(_hostingEnvironment.ContentRootPath + @"\Resources\JobDescriptions\");
                
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
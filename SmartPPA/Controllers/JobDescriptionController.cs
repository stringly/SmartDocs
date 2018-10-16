using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Edit(string filePath)
        {
            JobDescriptionViewModel job = new JobDescriptionViewModel(filePath);
            return View(job);
        }
        public IActionResult Create()
        {
            JobDescriptionViewModel vm = new JobDescriptionViewModel();
            return View(vm);
        }
    }
}
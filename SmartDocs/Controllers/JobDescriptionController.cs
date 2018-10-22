using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
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
            JobDescriptionListViewModel vm = new JobDescriptionListViewModel
            {
                Jobs = _repository.Jobs.Select(x => new JobDescriptionListViewModeltem(x)).ToList()
            };

            return View(vm);
        }

        public IActionResult UserIndex()
        {
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
        public IActionResult ShowJobDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SmartJob = _repository.Jobs.FirstOrDefault(j => j.JobId == id);
            if (SmartJob == null)
            {
                return NotFound();
            }
            JobDescription vmJob = new JobDescription(SmartJob);
            return View(vmJob);
        }


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

                // GET: SmartUsers/Delete/5
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

            return View(SmartJob);
        }

        // POST: SmartUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var smartJob = _repository.Jobs.FirstOrDefault(x => x.JobId == id);
            _repository.RemoveJob(smartJob);
            return RedirectToAction(nameof(Index));
        }

        private bool SmartUserExists(int id)
        {
            return _repository.Jobs.Any(e => e.JobId == id);
        }
    }
}
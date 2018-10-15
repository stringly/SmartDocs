using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartPPA.Models;
using SmartPPA.Models.ViewModels;

namespace SmartPPA.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
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
            JobDescription job = new JobDescription("PoliceOfficer_Patrol");
            PPAFormViewModel vm = new PPAFormViewModel(job);
            return View(vm);
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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

        // Testing HttpResponse
        // Holy shit, it works.
        public ActionResult GenerateDocument()
        {
            Dictionary<string, string> formData = new Dictionary<string, string>();
            formData.Add("EmployeeName", "Smith, Evan");
            formData.Add("PayrollId", "123456");
            formData.Add("PositionNumber", "55555");
            formData.Add("Job", "PoliceOfficer_Patrol");
            formData.Add("StartDate", "01/01/2018");
            formData.Add("EndDate", "01/01/2018");
            formData.Add("DistrictDivision", "District I");
            formData.Add("Assessment", "<p><strong>In the merry month of June</strong></p><ul><li><strong><em>From me home I started</em></strong></li><li><strong><em>Left the girls of Tume</em></strong></li><li><strong><em>Merely brok-en hearted</em></strong></li></ul><p>Test.</p>");
            formData.Add("Recommendations", "RECOMMEND RECOMMEND");
            formData.Add("AgencyActivity", "5025");
            formData.Add("PlaceOfWork", "5000 Rhode Island Ave");
            formData.Add("Supervisor", "Sgt. T. Test #1234");
            formData.Add("Supervises", "Squad #Test");
            return File(new DocumentGenerator().PopulateDocumentViaMappedList(formData), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Test.docx");

        }
    }
}
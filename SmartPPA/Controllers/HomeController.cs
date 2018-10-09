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
            return View(new MainFormViewModel());
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
        public FileStreamResult GenerateDocument()
        {
            var mem = new MemoryStream();
            //try
            //{           
                // Create Document
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    new Document(new Body()).Save(mainPart);

                    Body body = mainPart.Document.Body;
                    body.Append(new Paragraph(new Run(new Text("Hello World!"))));

                    // If I'm not serializing it to the HDD, do I need to call .Save() ?
                    // mainPart.Document.Save();
                    // wordDocument.Save("");
                    // Stream it down to the browser
                    FileStream fileStream = new FileStream("Test.docx", System.IO.FileMode.CreateNew);
                    mem.Position = 0;
                    mem.WriteTo(fileStream);
                    return File(fileStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Test.docx");
                }             
                
            //}
            //catch
            //{
            //    mem.Dispose();
            //    throw;
            //}
            
        }
    }
}
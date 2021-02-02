﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;


namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for "Home" interactions
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Roles = "User, Administrator")]
    public class HomeController : Controller
    {
        private IDocumentRepository _repository;
        public int PageSize = 15;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <remarks>This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="Startup.ConfigureServices"/></remarks>
        /// <param name="repo">An <see cref="IDocumentRepository"/></param>
        public HomeController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index(string sortOrder, string searchString, string selectedDocumentType, int page = 1)
        {
            if (User.HasClaim(x => x.Type == "UserId"))
            {
                int UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                // create a new view model
                DocumentListViewModel vm = new DocumentListViewModel();
                vm.CurrentSort = sortOrder;
                vm.CurrentFilter = searchString;
                vm.SelectedDocumentType = selectedDocumentType;
                vm.CreatedDateSort = String.IsNullOrEmpty(sortOrder) ? "createdDate_desc" : "";
                vm.DocumentTypeSort = sortOrder == "DocumentTypeName" ? "documentTypeName_desc" : "DocumentTypeName";
                vm.FileNameSort = sortOrder == "FileName" ? "fileName_desc" : "FileName";
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
                
                // assign the Documents property of the viewmodel to the a list of DocumentListViewModelItems
                // that is created by passing each of the repository's PPAs to the DocumentListViewModelItem
                // constructor that takes a SmartPPA parameter
                vm.Documents = _repository.Documents.Where(x => (x.AuthorUserId == UserId)
                    && (String.IsNullOrEmpty(selectedDocumentType) || x.Type.ToString() == selectedDocumentType)
                    && (String.IsNullOrEmpty(searchString) || x.FileName.ToLower().Contains(lowerSearchString)
                ))
                .Skip((page-1) * PageSize)
                .Take(PageSize)                    
                .ToList().ConvertAll(x => new DocumentListViewModelItem(x));
                int totalItems = _repository.Documents.Where(x => (x.AuthorUserId == UserId)
                    && (String.IsNullOrEmpty(selectedDocumentType) || x.Type.ToString() == selectedDocumentType)
                    && (String.IsNullOrEmpty(searchString) || x.FileName.ToLower().Contains(lowerSearchString)
                ))
                    .Count();
                vm.PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = totalItems
                };
                switch (sortOrder)
                {
                    case "DocumentTypeName":
                        vm.Documents = vm.Documents.OrderBy(x => x.DocumentTypeDisplayName).ToList();
                        break;
                    case "documentTypeName_desc":
                        vm.Documents = vm.Documents.OrderByDescending(x => x.DocumentTypeDisplayName).ToList();
                        break;
                    case "FileName":
                        vm.Documents = vm.Documents.OrderBy(x => x.DocumentName).ToList();
                        break;
                    case "fileName_desc":
                        vm.Documents = vm.Documents.OrderByDescending(x => x.DocumentName).ToList();
                        break;
                    case "createdDate_desc":
                        vm.Documents = vm.Documents.OrderByDescending(x => x.CreatedDate).ToList();
                        break;
                    default:

                        break;
                }
                ViewData["Title"] = "My Documents";
                ViewData["ActiveNavBarMenuLink"] = "My Documents";
                return View(vm);
            }
            else
            {
                // TODO: if windows auth fails, the application will return a 401 long before they get here? Shouldn't this check against a null _repo.GetCurrentUser()?
                return RedirectToAction("Access Denied", "Home");
            }
        }

        
        /// <summary>
        /// Shows the "Choices" view.
        /// </summary>
        /// <remarks>
        /// This method shows the "Choices" landing page. It first checks the <see cref="M:SmartDocs.Models.IDocumentRepository.GetCurrentUser"/>
        /// method to ensure the user is known to the application. If the user's LDAP name is not in the "Users" table, the repo method will
        /// return "null" and the user will be redirected to the "Access Denied" view.
        /// </remarks>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public ActionResult Choices()
        {
            ViewData["Title"] = "Document Menu";
            ViewData["ActiveNavBarMenuLink"] = "Choices";
            ViewBag.UniqueUsersCount = _repository.Documents.Select(x => x.AuthorUserId).Distinct().Count();
            ViewBag.DocumentsCreatedCount = _repository.Documents.Count();
            return View();
        }
        /// <summary>
        /// Shows the "About" view.
        /// </summary>
        /// <remarks>This view shows the FAQ, help file link, and admin contact info.</remarks>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewData["Title"] = "About SmartDocs";
            return View();
        }

        /// <summary>
        /// Shows the "Access Denied" view.
        /// </summary>
        /// <remarks>This view shows the user a message generic "not authorized" message.</remarks>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            ViewData["Title"] = "Access Denied";
            return View();
        }
        /// <summary>
        /// View that is returned when a user attempts an action for which they are not authorized.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult NotAuthorized()
        {
            ViewBag.Message = "Unauthorized Action Attempted.";
            return View();
        }
        public ActionResult Download(int id)
        {
            // create a generator, passing the repository as a parameter
            SmartDocument document = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            if(document != null)
            {                
                switch (document.Type)
                {
                    case SmartDocument.SmartDocumentType.PPA:
                        var ppaFactory = new SmartPPAFactory(_repository, document);
                        return File(ppaFactory.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", document.FileName);                        
                    case SmartDocument.SmartDocumentType.JobDescription:
                        var jobFactory = new SmartJobDescriptionFactory(_repository, document);
                        return File(jobFactory.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", document.FileName);                        
                    case SmartDocument.SmartDocumentType.AwardForm:
                        var awardFactory = new SmartAwardFactory(_repository, document);
                        return File(awardFactory.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", document.FileName);                        
                    case SmartDocument.SmartDocumentType.PAF:
                        var pafFactory = new SmartPAFFactory(_repository, document);
                        return File(pafFactory.GenerateDocument(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", document.FileName);
                    default:
                        return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
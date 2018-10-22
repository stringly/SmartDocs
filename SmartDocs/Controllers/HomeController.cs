using System;
using System.Diagnostics;
using System.Linq;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IDocumentRepository _repository;

        public HomeController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public ActionResult Choices()
        {
            
            if (_repository.GetCurrentUser() == null)
            {
                // This is a LDAP-authenticated account without an associated SmartDocs account
                return RedirectToAction(nameof(AccessDenied));
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
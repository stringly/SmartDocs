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
    /// <summary>
    /// Controller for "Home" interactions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize(Roles = "User, Administrator")]
    public class HomeController : Controller
    {
        private IDocumentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Controllers.HomeController"/> class.
        /// </summary>
        /// <remarks>This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="M:SmartDocs.Startup.ConfigureServices"/></remarks>
        /// <param name="repo">An <see cref="T:SmartDocs.Models.IDocumentRepository"/></param>
        public HomeController(IDocumentRepository repo)
        {
            _repository = repo;
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
    }
}
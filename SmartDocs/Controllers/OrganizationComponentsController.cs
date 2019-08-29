using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartDocs.Models;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="T:SmartDocs.Models.OrganizationComponent"/> interactions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class OrganizationComponentsController : Controller
    {        
        private IDocumentRepository _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.OrganizationComponentsController"/> class.
        /// </summary>
        /// <remarks>This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="M:SmartDocs.Startup.ConfigureServices"/></remarks>
        /// <param name="repo">An <see cref="T:SmartDocs.Models.IDocumentRepository"/></param>
        public OrganizationComponentsController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Shows a view with a list of all <see cref="T:SmartDocs.Models.OrganizationComponent"/> in the DB
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Index()
        {
            ViewData["Title"] = "Component List";
            ViewData["ActiveNavBarMenuLink"] = "Components";
            return View(_repository.Components.ToList());
        }

        /// <summary>
        /// GET: OrganizationComponents/Details?id=""
        /// </summary>
        /// <remarks>Returns a view that shows the details for the <see cref="T:SmartDocs.Models.OrganizationComponent"/> with the provided id
        /// </remarks>
        /// <param name="id">The identifier for the <see cref="T:SmartDocs.Models.OrganizationComponent"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                // querystring is empty
                return NotFound();
            }
            // find the Component with the provided id
            var organizationComponent = _repository.Components.FirstOrDefault(m => m.ComponentId == id);
            if (organizationComponent == null)
            {
                // no Component with the provided id exists in the DB
                return NotFound();
            }
            ViewData["Title"] = "Component Details";
            return View(organizationComponent);
        }

        /// <summary>
        /// Shows the view to create a new <see cref="T:SmartDocs.Models.OrganizationComponent"/>.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Create()
        {
            ViewData["Title"] = "Create a Component";
            return View();
        }

        /// <summary>
        /// POST: OrganizationComponents/Create
        /// </summary>
        /// <remarks>Creates the specified organization component.</remarks>
        /// <param name="organizationComponent">The POSTed form data, bound to a <see cref="T:SmartDocs.Models.OrganizationComponent"/> object.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ComponentId,Name,Address,DepartmentCode")] OrganizationComponent organizationComponent)
        {
            if (ModelState.IsValid)
            {
                // model validation passed, save the component
                // context updates are encapsulated in the repository
                _repository.SaveComponent(organizationComponent);
                // redirect to Index view
                return RedirectToAction(nameof(Index));
            }
            // model validation failed, return the object to the view with validation error messages
            ViewData["Title"] = "Create a Component: Error";
            return View(organizationComponent);
        }

        /// <summary>
        /// GET: OrganizationComponent/Edit?id=""
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.OrganizationComponent"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                // querystring is null
                return NotFound();
            }
            // find an OrganizationComponent in the repo with the given id
            var organizationComponent = _repository.Components.FirstOrDefault(x => x.ComponentId == id);

            if (organizationComponent == null)
            {
                // no OrganizationComponent with the given id could be found
                return NotFound();
            }
            ViewData["Title"] = "Edit Component";
            return View(organizationComponent);
        }

        /// <summary>
        /// POST: OrganizationComponent/Edit
        /// </summary>
        /// <param name="id">The identifier for the <see cref="T:SmartDocs.Models.OrganizationComponent"/></param>
        /// <param name="organizationComponent">The POSTed form data, bound to a <see cref="T:SmartDocs.Models.OrganizationComponent"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ComponentId,Name,Address,DepartmentCode")] OrganizationComponent organizationComponent)
        {
            if (id != organizationComponent.ComponentId)
            {
                // form ComponentId doesn't match id parameeter
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.SaveComponent(organizationComponent);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationComponentExists(organizationComponent.ComponentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Edit Component: Error";
            return View(organizationComponent);
        }

        /// <summary>
        /// GET: OrganizationComponents/Delete
        /// </summary>
        /// <remarks>Displays the Delete confirmation page</remarks>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.OrganizationComponent"/> to be deleted.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.ActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationComponent = _repository.Components
                .FirstOrDefault(m => m.ComponentId == id);
            if (organizationComponent == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Component";
            return View(organizationComponent);
        }

        /// <summary>
        /// POST: OrganizationComponents/Delete
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.OrganizationComponent"/> to be deleted</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // find the Component with the provided id
            var organizationComponent = _repository.Components.FirstOrDefault(x => x.ComponentId == id);
            // context actions are encapsulated in the repository
            _repository.RemoveComponent(organizationComponent);
            // redirect to OrganizationComponents/Index
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines whether a component exists with the provided id
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.OrganizationComponent"/></param>
        /// <returns></returns>
        private bool OrganizationComponentExists(int id)
        {
            return _repository.Components.Any(e => e.ComponentId == id);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;
using System;
using System.Linq;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="OrganizationUnit"/> interactions
    /// </summary>
    /// <seealso cref="Controller" />
    [Authorize(Policy = "IsGlobalAdmin")]
    public class OrganizationUnitsController : Controller
    {        
        private IDocumentRepository _repository;
        private int PageSize = 15;
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnitsController"/> class.
        /// </summary>
        /// <remarks>This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="Startup.ConfigureServices"/></remarks>
        /// <param name="repo">An <see cref="IDocumentRepository"/></param>
        public OrganizationUnitsController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Shows a view with a list of all <see cref="OrganizationUnit"/> in the DB
        /// </summary>
        /// <returns>An <see cref="ActionResult"/></returns>
        public IActionResult Index(string sortOrder, string searchString, string SelectedDivision, string SelectedBureau, int page = 1)
        {
            OrganizationUnitIndexViewModel vm = new OrganizationUnitIndexViewModel
            {
                CurrentFilter = searchString,
                CurrentSort = sortOrder,
                SelectedBureau = SelectedBureau,
                SelectedDivision = SelectedDivision,
                TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "",
                AddressSort = sortOrder == "Address" ? "address_desc" : "Address",
                CodeSort = sortOrder == "Code" ? "code_desc" : "Code",
                DivisionSort = sortOrder == "Division" ? "division_desc" : "Division",
                BureauSort = sortOrder == "Bureau" ? "bureau_desc" : "Bureau"
            };
            // lower any search string text to facilitate comparison.
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
            // Query for documents that match the provided parameters, if any, and convert them to List Items
            vm.Units = _repository.Units.Where(x => (String.IsNullOrEmpty(SelectedDivision) || x.Division == SelectedDivision)
                && (String.IsNullOrEmpty(SelectedBureau) || x.Bureau == SelectedBureau)
                && (String.IsNullOrEmpty(searchString) || x.Title.ToLower().Contains(lowerSearchString)))
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            // Separate Query to determine total number of matching documents, as the list above is Skip().Take()
            int totalItems = _repository.Units.Where(x => (String.IsNullOrEmpty(SelectedDivision) || x.Division == SelectedDivision)
                && (String.IsNullOrEmpty(SelectedBureau) || x.Bureau == SelectedBureau)
                && (String.IsNullOrEmpty(searchString) || x.Title.ToLower().Contains(lowerSearchString)))
                .Count();
            // Sort the list according to the provided sorting parameter, if any. Defaults to order by ascending Grade if no sorting parameter is present.
            switch (sortOrder)
            {
                case "Address":
                    vm.Units = vm.Units.OrderBy(x => x.Address).ToList();
                    break;                
                case "address_desc":
                    vm.Units = vm.Units.OrderByDescending(x => x.Address).ToList();
                    break;
                case "Code":
                    vm.Units = vm.Units.OrderBy(x => x.Code).ToList();
                    break;
                case "code_desc":
                    vm.Units = vm.Units.OrderByDescending(x => x.Code).ToList();
                    break;
                case "Division":
                    vm.Units = vm.Units.OrderBy(x => x.Division).ToList();
                    break;
                case "division_desc":
                    vm.Units = vm.Units.OrderByDescending(x => x.Division).ToList();
                    break;
                case "Bureau":
                    vm.Units = vm.Units.OrderBy(x => x.Bureau).ToList();
                    break;
                case "bureau_desc":
                    vm.Units = vm.Units.OrderByDescending(x => x.Bureau).ToList();
                    break;
                case "title_desc":
                    vm.Units = vm.Units.OrderByDescending(x => x.Title).ToList();
                    break;
                default:
                    vm.Units = vm.Units.OrderBy(x => x.Title).ToList();
                    break;
            }
            // Set the PagingInfo to facilitate page control
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = totalItems,
                ItemsPerPage = PageSize
            };
            vm.HydrateLists(_repository.Units.Select(x => x.Division).Distinct().ToList(), _repository.Units.Select(x => x.Bureau).Distinct().ToList());
            ViewData["Title"] = "Org Unit List";
            ViewData["ActiveNavBarMenuLink"] = "Units";
            return View(vm);
        }

        /// <summary>
        /// GET: OrganizationComponents/Details?id=""
        /// </summary>
        /// <remarks>Returns a view that shows the details for the <see cref="OrganizationUnit"/> with the provided id
        /// </remarks>
        /// <param name="id">The <see cref="OrganizationUnit.Id"/> of the <see cref="OrganizationUnit"/></param>
        /// <returns>An <see cref="ActionResult"/></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                // querystring is empty
                return NotFound();
            }
            // find the Component with the provided id
            var unit = _repository.Units.FirstOrDefault(m => m.Id == id);
            if (unit == null)
            {
                // no Component with the provided id exists in the DB
                return NotFound();
            }
            ViewData["Title"] = "Organization Unit Details";
            return View(unit);
        }

        /// <summary>
        /// Shows the view to create a new <see cref="OrganizationUnit"/>.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/></returns>
        public IActionResult Create()
        {
            ViewData["Title"] = "Create an Organization Unit";
            return View();
        }

        /// <summary>
        /// POST: OrganizationComponents/Create
        /// </summary>
        /// <remarks>Creates the specified organization component.</remarks>
        /// <param name="form">The POSTed form data, bound to a <see cref="OrganizationUnit"/> object.</param>
        /// <returns>An <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Address,Code,Division,Bureau")] OrganizationUnit form)
        {
            if (ModelState.IsValid)
            {
                // model validation passed, save the component
                // context updates are encapsulated in the repository
                _repository.SaveUnit(form);
                // redirect to Index view
                return RedirectToAction(nameof(Index));
            }
            // model validation failed, return the object to the view with validation error messages
            ViewData["Title"] = "Create an Organization Unit: Error";
            return View(form);
        }

        /// <summary>
        /// GET: OrganizationComponent/Edit?id=""
        /// </summary>
        /// <param name="id">The <see cref="OrganizationUnit.Id"/> of the <see cref="OrganizationUnit"/></param>
        /// <returns>An <see cref="ActionResult"/></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                // querystring is null
                return NotFound();
            }
            // find an OrganizationComponent in the repo with the given id
            var unit = _repository.Units.FirstOrDefault(x => x.Id == id);

            if (unit == null)
            {
                // no OrganizationComponent with the given id could be found
                return NotFound();
            }
            ViewData["Title"] = "Edit Organization Unit";
            return View(unit);
        }

        /// <summary>
        /// POST: OrganizationComponent/Edit
        /// </summary>
        /// <param name="id">The <see cref="OrganizationUnit.Id"/> of the <see cref="OrganizationUnit"/></param>
        /// <param name="unit">The POSTed form data, bound to a <see cref="OrganizationUnit"/></param>
        /// <returns>An <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Address,Code,Division,Bureau")] OrganizationUnit unit)
        {
            if (id != unit.Id)
            {
                // form ComponentId doesn't match id parameeter
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.SaveUnit(unit);                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationComponentExists(unit.Id))
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
            ViewData["Title"] = "Edit Organization Unit: Error";
            return View(unit);
        }

        /// <summary>
        /// GET: OrganizationComponents/Delete
        /// </summary>
        /// <remarks>Displays the Delete confirmation page</remarks>
        /// <param name="id">The <see cref="OrganizationUnit.Id"/> of the <see cref="OrganizationUnit"/> to be deleted.</param>
        /// <returns>An <see cref="ActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = _repository.Units
                .FirstOrDefault(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Delete Component";
            return View(unit);
        }

        /// <summary>
        /// POST: OrganizationComponents/Delete
        /// </summary>
        /// <param name="id">The <see cref="OrganizationUnit.Id"/> of the <see cref="OrganizationUnit"/> to be deleted</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // find the Component with the provided id
            var unit = _repository.Units.FirstOrDefault(x => x.Id == id);
            // context actions are encapsulated in the repository
            _repository.RemoveUnit(unit);
            // redirect to OrganizationComponents/Index
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines whether a <see cref="OrganizationUnit"/> with the provided <see cref="OrganizationUnit.Id"/> exists
        /// </summary>
        /// <param name="id">The identifier of the <see cref="OrganizationUnit"/></param>
        /// <returns>True if the <see cref="OrganizationUnit"/> with the provided <see cref="OrganizationUnit.Id"/> exists, otherwise false.</returns>
        private bool OrganizationComponentExists(int id)
        {
            return _repository.Units.Any(e => e.Id == id);
        }
    }
}

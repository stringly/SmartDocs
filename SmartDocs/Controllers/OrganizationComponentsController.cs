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
    public class OrganizationComponentsController : Controller
    {        
        private IDocumentRepository _repository;

        public OrganizationComponentsController(IDocumentRepository repo)
        {
            _repository = repo;
        }

        // GET: OrganizationComponents
        public IActionResult Index()
        {
            return View(_repository.Components.ToList());
        }

        // GET: OrganizationComponents/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationComponent = _repository.Components.FirstOrDefault(m => m.ComponentId == id);
            if (organizationComponent == null)
            {
                return NotFound();
            }

            return View(organizationComponent);
        }

        // GET: OrganizationComponents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrganizationComponents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ComponentId,Name,Address,DepartmentCode")] OrganizationComponent organizationComponent)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveComponent(organizationComponent);                
                return RedirectToAction(nameof(Index));
            }
            return View(organizationComponent);
        }

        // GET: OrganizationComponents/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationComponent = _repository.Components.FirstOrDefault(x => x.ComponentId == id);
            if (organizationComponent == null)
            {
                return NotFound();
            }
            return View(organizationComponent);
        }

        // POST: OrganizationComponents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ComponentId,Name,Address,DepartmentCode")] OrganizationComponent organizationComponent)
        {
            if (id != organizationComponent.ComponentId)
            {
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
            return View(organizationComponent);
        }

        // GET: OrganizationComponents/Delete/5
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

            return View(organizationComponent);
        }

        // POST: OrganizationComponents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var organizationComponent = _repository.Components.FirstOrDefault(x => x.ComponentId == id);
            _repository.RemoveComponent(organizationComponent);
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationComponentExists(int id)
        {
            return _repository.Components.Any(e => e.ComponentId == id);
        }
    }
}

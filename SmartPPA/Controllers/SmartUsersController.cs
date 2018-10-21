using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartPPA.Models;

namespace SmartPPA.Controllers
{
    public class SmartUsersController : Controller
    {
        private readonly SmartDocContext _context;

        public SmartUsersController(SmartDocContext context)
        {
            _context = context;
        }

        // GET: SmartUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: SmartUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartUser = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (smartUser == null)
            {
                return NotFound();
            }

            return View(smartUser);
        }

        // GET: SmartUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SmartUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,BlueDeckId,LogonName,DisplayName")] SmartUser smartUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(smartUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(smartUser);
        }

        // GET: SmartUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartUser = await _context.Users.FindAsync(id);
            if (smartUser == null)
            {
                return NotFound();
            }
            return View(smartUser);
        }

        // POST: SmartUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,BlueDeckId,LogonName,DisplayName")] SmartUser smartUser)
        {
            if (id != smartUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(smartUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SmartUserExists(smartUser.UserId))
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
            return View(smartUser);
        }

        // GET: SmartUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartUser = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (smartUser == null)
            {
                return NotFound();
            }

            return View(smartUser);
        }

        // POST: SmartUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smartUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(smartUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SmartUserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}

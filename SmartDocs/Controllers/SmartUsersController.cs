using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="T:SmartDocs.Models.SmartUser"/> interactions
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize(Roles = "Administrator")]
    public class SmartUsersController : Controller
    {
        private IDocumentRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Controllers.SmartUsersController"/> class.
        /// </summary>
        /// <param name="repo">A <see cref="T:SmartDocs.Models.IDocumentRepository"/></param>
        public SmartUsersController(IDocumentRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// GET: SmartUsers/Index
        /// </summary>
        /// <remarks>
        /// This is an admin only view.
        /// </remarks>
        /// <param name="searchString">The search string that limits the list.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/> list of users in the repo.</returns>
        public IActionResult Index(string searchString)
        {
            UserIndexListViewModel vm = new UserIndexListViewModel { CurrentFilter = searchString };
            
            // if the searchstring isn't empty, sort the user list against the string
            if (!string.IsNullOrEmpty(searchString))
            {
                vm.Users = _repo.Users.Where(x => x.DisplayName.Contains(searchString));
            }
            else
            {
                vm.Users = _repo.Users.ToList();
            }
            // return the view
            ViewData["Title"] = "Current User List";
            ViewData["ActiveNavBarMenuLink"] = "Users";
            return View(vm);
        }


        /// <summary>
        /// GET: SmartUsers/Create
        /// </summary>
        /// <remarks>
        /// View that allows Admin to create a new SmartUser
        /// </remarks>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Create()
        {
            ViewData["Title"] = "Create User";
            return View();
        }

        /// <summary>
        /// POST: SmartUsers/Create
        /// </summary>
        /// <param name="smartUser">The POSTed form data, bound to a <see cref="T:SmartDocs.Models.SmartUser"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserId,BlueDeckId,LogonName,DisplayName")] SmartUser smartUser)
        {
            // check if POSTed data is valid
            if (ModelState.IsValid)
            {
                _repo.SaveUser(smartUser);
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["Title"] = "Create User: Error";
            return View(smartUser);
        }

        /// <summary>
        /// GET: SmartUsers/Edit?id=""
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartUser"/></param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [Authorize(Roles = "User, Administrator")]
        public IActionResult Edit(int? id)
        {            
            SmartUser smartUser = null;
            if (id == null)
            {
                // querystring is null, return 404
                return NotFound();
            }
            if (User.IsInRole("Administrator"))
            {
                // retrieve the SmartUser from the repo
                smartUser = _repo.Users.FirstOrDefault(x => x.UserId == id);
                if (smartUser == null)
                {
                    // no SmartUser with the given id exists in the repo
                    return NotFound();
                }
            }
            else
            {                
                if (User.HasClaim(x => x.Type == "UserId"))
                {
                    int UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                    smartUser = _repo.Users.FirstOrDefault(x => x.UserId == UserId);
                }
            }
            if(smartUser == null)
            {
                return RedirectToAction("Not Authorized", "Home");
            }
            // return the view
            ViewData["Title"] = "Edit User";
            ViewData["ActiveNavBarMenuLink"] = "Edit User";
            return View(smartUser);
        }

        /// <summary>
        /// POST: SmartUsers/Edit?id=""
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartUser"/> to edit.</param>
        /// <param name="smartUser">The POSTed form data, bound to a <see cref="SmartDocs.Models.SmartUser"/> object.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User, Administrator")]
        public IActionResult Edit(int id, [Bind("UserId,DisplayName")] SmartUser smartUser)
        {
            // if querystring id doesn't match the POSTed form UserId
            if (id != smartUser.UserId)
            {
                return NotFound();
            }

            // validate model state
            if (ModelState.IsValid)
            {
                // invoke the repo method to save the user
                _repo.SaveUser(smartUser);                    
                return RedirectToAction("Choices", "Home");
            }
            // modelstate invalid, return the view with the VM with validation errors showing
            ViewData["Title"] = "Edit User: Error";
            ViewData["ActiveNavBarMenuLink"] = "Edit User";
            return View(smartUser);
        }

        /// <summary>
        /// GET: SmartUsers/Delete?id=""
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartUser"/>.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                // querystring is null, return 404
                return NotFound();
            }            
            // retrieve the SmartUser from the repo
            var smartUser = _repo.Users.FirstOrDefault(m => m.UserId == id);            
            if (smartUser == null)
            {
                // no SmartUser with the given id could be found in the repo
                return NotFound();
            }
            // return the view
            ViewData["Title"] = "Delete User";
            return View(smartUser);
        }

        /// <summary>
        /// POST: SmartUsers/Delete?id=""
        /// </summary>
        /// <param name="id">The identifier of the <see cref="SmartDocs.Models.SmartUser"/> to be deleted.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartUser from the repo
            var smartUser = _repo.Users.FirstOrDefault(x => x.UserId == id);
            // invoke the repo method to remove the SmartUser
            _repo.RemoveUser(smartUser);
            // redirect to the index
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Determines if a <see cref="T:SmartDocs.Models.SmartUser"/> exists in the repo.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="T:SmartDocs.Models.SmartUser"/>.</param>
        /// <returns></returns>
        private bool SmartUserExists(int id)
        {
            return _repo.Users.Any(e => e.UserId == id);
        }
    }
}

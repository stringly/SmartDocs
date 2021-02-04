using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.ViewModels;
using System;
using System.Linq;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="SmartUser"/> interactions
    /// </summary>
    /// <seealso cref="Controller" />    
    public class SmartUsersController : Controller
    {
        private IDocumentRepository _repo;
        private int PageSize = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartUsersController"/> class.
        /// </summary>
        /// <param name="repo">An implementation of <see cref="IDocumentRepository"/></param>
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
        /// <returns>An <see cref="IActionResult"/> list of users in the repo.</returns>      
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Index(string searchString, string sortOrder, int page = 1)
        {
            // TODO: Make this a proper sorting index
            UserIndexListViewModel vm = new UserIndexListViewModel();
            vm.CurrentFilter = searchString;
            vm.CurrentSort = sortOrder;
            vm.LDAPNameSort = String.IsNullOrEmpty(sortOrder) ? "LDAPName_desc" : "";
            vm.BlueDeckIdSort = sortOrder == "BlueDeckId" ? "BlueDeckId" : "blueDeckId_Desc";
            vm.UserIdSort = sortOrder == "" // TODO: HERE
            
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
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize
            };
            // if the searchstring isn't empty, sort the user list against the string
            vm.Users = _repo.Users
                .Where(x => (String.IsNullOrEmpty(searchString) || x.DisplayName.Contains(lowerSearchString)))
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
            vm.PagingInfo.TotalItems = _repo.Users
                .Where(x => (String.IsNullOrEmpty(searchString) || x.DisplayName.Contains(lowerSearchString)))                
                .Count();            
            switch (sortOrder)
            {
                
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
        /// <returns>An <see cref="IActionResult"/></returns>  
        [Authorize(Policy = "IsGlobalAdmin")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Create User";
            return View();
        }

        /// <summary>
        /// POST: SmartUsers/Create
        /// </summary>
        /// <param name="smartUser">The POSTed form data, bound to a <see cref="SmartUser"/></param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsGlobalAdmin")]
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
        /// GET: SmartUsers/Edit?id={0}
        /// </summary>
        /// <param name="id">The <see cref="SmartUser.UserId"/> of the <see cref="SmartUser"/> to edit.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "CanEditUser")]
        public IActionResult Edit(int id)
        {
            // retrieve the SmartUser from the repo
            SmartUser smartUser = _repo.Users.FirstOrDefault(x => x.UserId == id);
            if (smartUser == null)
            {
                // no SmartUser with the given id exists in the repo
                return NotFound();
            }
            // return the view
            ViewData["Title"] = "Edit User";
            ViewData["ActiveNavBarMenuLink"] = "Edit User";
            return View(smartUser);
        }

        /// <summary>
        /// POST: SmartUsers/Edit?id={0}
        /// </summary>
        /// <param name="id">The <see cref="SmartUser.UserId"/> of the <see cref="SmartUser"/> to edit.</param>
        /// <param name="smartUser">The POSTed form data, bound to a <see cref="SmartUser"/> object.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "CanEditUser")]
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
        /// <param name="id">The <see cref="SmartUser.UserId"/> of the <see cref="SmartUser"/> to delete.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [Authorize(Policy = "IsGlobalAdmin")]
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
        /// <param name="id">The <see cref="SmartUser.UserId"/> of the <see cref="SmartUser"/> to delete.</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsGlobalAdmin")]
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
        /// Determines if a <see cref="SmartUser"/> exists in the repo.
        /// </summary>
        /// <param name="id">The <see cref="SmartUser.UserId"/> of the <see cref="SmartUser"/> to delete.</param>
        /// <returns></returns>
        private bool SmartUserExists(int id)
        {
            return _repo.Users.Any(e => e.UserId == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.ViewComponents
{
    /// <summary>
    /// Class that generates the Navigation Bar  ViewComponent
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class UserNavBarViewComponent : ViewComponent
    {
        private readonly IDocumentRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.ViewComponents.UserNavBarViewComponent"/> class.
        /// </summary>
        /// <param name="repo">An injected <see cref="T:SmartDocs.Models.IDocumentRepository"/> repository</param>
        public UserNavBarViewComponent(IDocumentRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Invokes the Default Component
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> task with the ViewComponent.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {            
            return View(_repository.GetCurrentUser());
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.ViewComponents
{
    public class UserNavBarViewComponent : ViewComponent
    {
        private readonly IDocumentRepository _repository;
        
        public UserNavBarViewComponent(IDocumentRepository repo)
        {
            _repository = repo;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {            
            return View(_repository.GetCurrentUser());
        }
    }
}

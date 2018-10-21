using Microsoft.AspNetCore.Mvc;
using SmartPPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.ViewComponents
{
    public class UserNavBarViewComponent : ViewComponent
    {
        private readonly IDocumentRepository _repository;
        
        public UserNavBarViewComponent(IDocumentRepository repo)
        {
            _repository = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {            
            return View(_repository.GetCurrentUser());
        }
    }
}

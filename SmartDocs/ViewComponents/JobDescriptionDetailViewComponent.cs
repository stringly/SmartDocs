using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.ViewComponents
{
    public class JobDescriptionDetailViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(JobDescription job)
        {
            return View(job);
        }
    }
}

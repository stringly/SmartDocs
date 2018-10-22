using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartDocs.Models;
using SmartDocs.Models.Types;

namespace SmartDocs.ViewComponents
{
    public class JobDescriptionCategoryListViewComponent : ViewComponent
    {

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(JobDescription job)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {            
            return View(job);
        }
    }
}

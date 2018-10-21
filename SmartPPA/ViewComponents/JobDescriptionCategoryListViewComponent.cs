using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartPPA.Models;
using SmartPPA.Models.Types;

namespace SmartPPA.ViewComponents
{
    public class JobDescriptionCategoryListViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(JobDescription job)
        {            
            return View(job);
        }
    }
}

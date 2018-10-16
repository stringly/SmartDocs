using Microsoft.AspNetCore.Mvc;
using SmartPPA.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.ViewComponents
{
    public class JobDescriptionCategoryEditorViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<JobDescriptionCategory> list)
        {            
            return View(list);
        }
    }
}

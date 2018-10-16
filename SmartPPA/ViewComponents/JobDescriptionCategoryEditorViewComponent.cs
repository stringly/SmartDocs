using Microsoft.AspNetCore.Mvc;
using SmartPPA.Models;
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
            JobDescription job = new JobDescription();
            job.Categories = list;
            int categoriesCount = list.Count();
            if (categoriesCount < 6)
            {
                for (var i = categoriesCount; i < 6; i++)
                {
                    JobDescriptionCategory c = new JobDescriptionCategory();
                    list.Add(c);
                }
            }

            foreach (JobDescriptionCategory j in list)
            {
                int descriptionItemsCount = j.PositionDescriptionItems.Count();
                if (descriptionItemsCount < 7)
                {
                    for (var i = descriptionItemsCount; i < 7; i++)
                    {
                        PositionDescriptionItem p = new PositionDescriptionItem();
                        j.PositionDescriptionItems.Add(p);
                    }
                }
                int performanceItemsCount = j.PerformanceStandardItems.Count();
                if (performanceItemsCount < 7)
                {
                    for (var i = performanceItemsCount; i < 7; i++)
                    {
                        PerformanceStandardItem p = new PerformanceStandardItem();
                        j.PerformanceStandardItems.Add(p);
                    }
                }
            }
            return View(job);
        }
    }
}

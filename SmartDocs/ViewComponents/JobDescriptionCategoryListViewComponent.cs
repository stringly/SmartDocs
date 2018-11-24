using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartDocs.Models;
using SmartDocs.Models.Types;

namespace SmartDocs.ViewComponents
{
    /// <summary>
    /// ViewComponent that allows a user to assign ratings to a Job Description's categories
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class JobDescriptionCategoryListViewComponent : ViewComponent
    {
        /// <summary>
        /// Generates the ViewComponent from a <see cref="T:SmartDocs.Models.JobDescription"/> job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> task with the ViewComponent.</returns>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(JobDescription job)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {            
            return View(job);
        }
    }
}

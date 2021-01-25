using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models.ViewModels;
using System.Threading.Tasks;

namespace SmartDocs.ViewComponents
{
    /// <summary>
    /// Class that generates the Navigation Bar ViewComponent
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class UserNavBarViewComponent : ViewComponent
    {   
        /// <summary>
        /// Invokes the Default Component
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> task with the ViewComponent.</returns>
        public async Task<IViewComponentResult> InvokeAsync(string activeLink)
        {            
            NavBarViewModel vm = new NavBarViewModel { ActiveLink = activeLink };
            return View(vm);
        }
    }


}

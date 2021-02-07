using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller that displays error views.
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Returns a generic error view and message.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>An <see cref="IActionResult"/></returns>
        [HttpGet("/error")]
        public IActionResult Error(int? statusCode = null)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewData["ErrorUrl"] = feature?.OriginalPath;
            if (statusCode.HasValue)
            {
                switch (statusCode.Value)
                {
                    case 403:
                        ViewBag.Title = "Not Authorized";
                        ViewBag.Message = "You are not authorized to perform this action.";
                        break;
                    case 404:
                        ViewBag.Title = "Not Found";
                        ViewBag.Message = "The resource you requested was not found.";
                        break;
                }
            }
            else
            {
                ViewBag.Title = "Error";
                ViewBag.Message = "The application could not process your request.";
            }
            return View();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth.CanEditUser
{
    /// <summary>
    /// Handler that determines if a User is attempting to Edit their own information.
    /// </summary>
    public class IsEditingSelfHandler : AuthorizationHandler<CanEditUserRequirement>
    {
        /// <summary>
        /// Handles the requirement
        /// </summary>
        /// <param name="context">The <see cref="AuthorizationHandlerContext"/></param>
        /// <param name="requirement">The <see cref="CanEditUserRequirement"/></param>
        /// <returns>Success if the User is editing their own info, otherwise fails.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUserRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "UserId"))
            {
                // retrieve the User's UserId Claim converted to Int
                int UserId = Convert.ToInt32(((ClaimsIdentity)context.User.Identity).FindFirst("UserId").Value);
                // retrieve the Id of the requested document from the route
                var authContext = (AuthorizationFilterContext)context.Resource;
                var routeUserId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id")?.ToString() ?? null);
                if (routeUserId == UserId)
                {
                    context.Succeed(requirement);
                }
                
            }
            return Task.FromResult(0);
        }
    }
}

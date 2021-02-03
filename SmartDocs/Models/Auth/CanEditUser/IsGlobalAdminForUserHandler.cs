using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth.CanEditUser
{
    /// <summary>
    /// Policy Handler that determines if the application user attempting to edit a user is a Global Administrator.
    /// </summary>
    public class IsGlobalAdminForUserHandler : AuthorizationHandler<CanEditUserRequirement>
    {
        /// <summary>
        /// Handles the Requirement
        /// </summary>
        /// <param name="context">An <see cref="AuthorizationHandlerContext"/></param>
        /// <param name="requirement">The <see cref="CanEditDocumentRequirement"/></param>
        /// <returns>Success if the user is an admin, otherwise fail.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditUserRequirement requirement)
        {
            if (context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}

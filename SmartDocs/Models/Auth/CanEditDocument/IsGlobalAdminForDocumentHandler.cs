using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth.CanEditDocument
{
    /// <summary>
    /// Policy Handler that determines if the application user attempting to edit a document is a Global Administrator.
    /// </summary>
    public class IsGlobalAdminForDocumentHandler : AuthorizationHandler<CanEditDocumentRequirement>
    {
        /// <summary>
        /// Handles the Requirement
        /// </summary>
        /// <param name="context">An <see cref="AuthorizationHandlerContext"/></param>
        /// <param name="requirement">The <see cref="CanEditDocumentRequirement"/></param>
        /// <returns>Success if the user is an admin, otherwise fail.</returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditDocumentRequirement requirement)
        {
            if (context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}

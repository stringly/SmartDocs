using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth.CanEditDocument
{
    /// <summary>
    /// Policy Handler that determines if an application user is the author of a given document.
    /// </summary>
    public class IsDocumentAuthorHandler : AuthorizationHandler<CanEditDocumentRequirement>
    {
        private IDocumentRepository _repository;
        /// <summary>
        /// Creates a new instance of the handler
        /// </summary>
        /// <param name="repository">A <see cref="IDocumentRepository"/></param>
        public IsDocumentAuthorHandler(IDocumentRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Handles the Requirement
        /// </summary>
        /// <param name="context">An <see cref="AuthorizationHandlerContext"/></param>
        /// <param name="requirement">The <see cref="CanEditDocumentRequirement"/></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanEditDocumentRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "UserId"))
            {
                // retrieve the User's UserId Claim converted to Int
                int UserId = Convert.ToInt32(((ClaimsIdentity)context.User.Identity).FindFirst("UserId").Value);
                // retrieve the Id of the requested document from the route
                var authContext = (AuthorizationFilterContext)context.Resource;
                var documentId = Convert.ToInt32(authContext.HttpContext.GetRouteValue("id")?.ToString() ?? null);
                var doc = _repository.Documents.FirstOrDefault(x => x.DocumentId == documentId);
                if (doc != null)
                {
                    if (doc.AuthorUserId == UserId)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            return Task.FromResult(0);
        }
    }
}

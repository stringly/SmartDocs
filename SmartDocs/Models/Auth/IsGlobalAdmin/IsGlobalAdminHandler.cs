using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth
{
    public class IsGlobalAdminHandler : AuthorizationHandler<IsGlobalAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGlobalAdminRequirement requirement)
        {
            if (context.User.IsInRole("Administrator"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}

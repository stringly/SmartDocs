using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace SmartDocs.Models.Auth.IsUser
{
    public class IsUserHandler : AuthorizationHandler<IsUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsUserRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "UserId"))
            {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}

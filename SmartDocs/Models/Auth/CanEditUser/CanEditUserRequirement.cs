using Microsoft.AspNetCore.Authorization;

namespace SmartDocs.Models.Auth.CanEditUser
{
    /// <summary>
    /// Requirement that sets policy to determine if an application User can edit User information.
    /// </summary>
    public class CanEditUserRequirement : IAuthorizationRequirement
    {
    }
}

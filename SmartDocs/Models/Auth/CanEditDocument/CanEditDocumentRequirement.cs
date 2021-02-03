using Microsoft.AspNetCore.Authorization;

namespace SmartDocs.Models.Auth.CanEditDocument
{
    /// <summary>
    /// Authorization Policy that determines if an application user can edit a document.
    /// </summary>
    public class CanEditDocumentRequirement : IAuthorizationRequirement
    {
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDocs.Models
{
    /// <summary>
    /// Class that extracts the User's LDAP name from the HttpContext
    /// </summary>
    public class UserResolverService  
    {
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SmartDocs.Models.UserResolverService"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public UserResolverService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;            
        }

        /// <summary>
        /// Extracts the User's LDAP name from the context User.Identity.Name 
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            string fullName = _httpContext.HttpContext.User?.Identity?.Name;
            return fullName.Substring(fullName.LastIndexOf(@"\") +1 );            
        }
    }
}

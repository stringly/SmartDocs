using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartPPA.Models
{
    public class UserResolverService  
    {
        private readonly IHttpContextAccessor _httpContext;
        public UserResolverService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;            
        }

        public string GetUserName()
        {
            string fullName = _httpContext.HttpContext.User?.Identity?.Name;
            return fullName.Substring(fullName.LastIndexOf(@"\") +1 );            
        }
    }
}

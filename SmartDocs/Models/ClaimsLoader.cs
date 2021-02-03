using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace SmartDocs.Models
{
    public class ClaimsLoader : IClaimsTransformation
    {
        private readonly IDocumentRepository _repository;

        public ClaimsLoader(IDocumentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if (identity == null) return principal;

            var user = identity.Name;
            // Is this the Windows Logon name?
            if (user == null) return principal;

            if (principal.Identity is ClaimsIdentity)
            {
                string logonName = user.Split('\\')[1];
                logonName = logonName.ToLower();
                // pull user roles 
                SmartUser dbUser = _repository.GetUserByLogonName(logonName);
                if (dbUser != null)
                {
                    var ci = (ClaimsIdentity)principal.Identity;
                    if(logonName == "jcs30" || logonName == "jcsmith1")
                    {
                        ci.AddClaim(new Claim(ci.RoleClaimType, "Administrator"));
                    }
                    ci.AddClaim(new Claim(ci.RoleClaimType, "User"));
                    ci.AddClaim(new Claim("DisplayName", dbUser.DisplayName));                    
                    ci.AddClaim(new Claim("UserId", dbUser.UserId.ToString(), ClaimValueTypes.Integer32));
                    ci.AddClaim(new Claim("BlueDeckId", dbUser.BlueDeckId.ToString(), ClaimValueTypes.Integer32));
                    ci.AddClaim(new Claim("LDAPName", logonName));
                }
                else
                {
                    var ci = (ClaimsIdentity)principal.Identity;
                    ci.AddClaim(new Claim("DisplayName", "Guest"));
                    ci.AddClaim(new Claim("LDAPName", logonName));
                }

            }

            return principal;
        }
    }
}

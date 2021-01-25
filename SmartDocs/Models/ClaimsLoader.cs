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
                    //foreach (Role ur in dbUser.CurrentRoles)
                    //{
                    //    var c = new Claim(ci.RoleClaimType, ur.RoleType.RoleTypeName);
                    //    ci.AddClaim(c);
                    //    if (ur.RoleType.RoleTypeName == "ComponentAdmin")
                    //    {
                    //        int memberParentComponentId = dbUser.Position.ParentComponent.ComponentId;
                    //        // TODO: Repo method to get tree of componentIds for the user's parent component
                    //        List<ComponentSelectListItem> canEditComponents = _unitOfWork.Components.GetChildComponentsForComponentId(memberParentComponentId);
                    //        var d = new Claim("CanEditComponents", JsonConvert.SerializeObject(canEditComponents));
                    //        ci.AddClaim(d);
                    //        List<MemberSelectListItem> canEditMembers = _unitOfWork.Members.GetMembersUserCanEdit(memberParentComponentId);
                    //        var e = new Claim("CanEditUsers", JsonConvert.SerializeObject(canEditMembers));
                    //        ci.AddClaim(e);
                    //        List<PositionSelectListItem> canEditPositions = _unitOfWork.Positions.GetPositionsUserCanEdit(memberParentComponentId);
                    //        var f = new Claim("CanEditPositions", JsonConvert.SerializeObject(canEditPositions));
                    //        ci.AddClaim(f);
                    //    }
                    //}
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

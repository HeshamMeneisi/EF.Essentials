using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EF.Essentials.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermission : Attribute, IAuthorizeData, IAuthorizationFilter
    {
        public RequirePermission(string permission)
        {
            Permission = permission;
        }

        private string Permission { get; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissionClaims = context.HttpContext.User.Claims.Where(c => c.Type == CustomClaims.Permission);
            var hasClaim = permissionClaims.Any(x => x.Value == Permission || x.Value == "sudo");
            if (!hasClaim) context.Result = new ForbidResult();
        }

        public string AuthenticationSchemes { get; set; }
        public string Policy { get; set; }
        public string Roles { get; set; }
    }
}

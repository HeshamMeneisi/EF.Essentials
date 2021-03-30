using System.Collections.Generic;
using System.Security.Claims;
using GenericCompany.Common.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GenericCompany.Common.UnitTests
{
    [TestClass]
    public class RequirePermissionTest
    {
        private HttpContext context;

        [TestInitialize]
        public void Initialize()
        {
            context = new DefaultHttpContext();
            IEnumerable<Claim> customClaimsPermissionClaims = new List<Claim>() { new Claim(CustomClaims.Permission, CustomClaims.Permission) };
            IEnumerable<Claim> sudoClaims = new List<Claim>() { new Claim(CustomClaims.Permission, "sudo") };
            IEnumerable<ClaimsIdentity> identities = new List<ClaimsIdentity>() {new ClaimsIdentity(customClaimsPermissionClaims),
                                                                                 new ClaimsIdentity(sudoClaims)};
            context.User = new ClaimsPrincipal(identities);
        }

        /// <summary>
        /// Null Permission still has Sudo claim for default action.
        /// </summary>
        [TestMethod]
        public void OnAuthorization_NullPermission_HasClaimResult()
        {
            RequirePermission permission = new RequirePermission(null);
            ActionContext actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            filterContext.HttpContext = context;
            permission.OnAuthorization(filterContext);
            Assert.AreEqual(filterContext.Result, null);
        }

        [TestMethod]
        public void OnAuthorization_CustomClaimsPermission_HasClaimResult()
        {
            RequirePermission permission = new RequirePermission(CustomClaims.Permission);
            ActionContext actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            filterContext.HttpContext = context;
            permission.OnAuthorization(filterContext);
            Assert.AreEqual(filterContext.Result, null);
        }

        [TestMethod]
        public void OnAuthorization_SudoPermission_HasClaimResult()
        {
            RequirePermission permission = new RequirePermission("sudo");
            ActionContext actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());
            AuthorizationFilterContext filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            filterContext.HttpContext = context;
            permission.OnAuthorization(filterContext);
            Assert.AreEqual(filterContext.Result, null);
        }
    }
}

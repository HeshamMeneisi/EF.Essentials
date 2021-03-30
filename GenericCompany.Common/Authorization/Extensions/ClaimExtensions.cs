#nullable enable
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace GenericCompany.Common.Authorization.Extensions
{
    public static class ClaimExtension
        {
            public static bool HasClaim(this ControllerBase controller, Claim claim)
            {
                var claims = controller.HttpContext.User.Claims;
                return claims.Any(c => c.Type == claim.Type && c.Value == claim.Value);
            }

            public static bool HasClaim(this ControllerBase controller,string type, string value)
            {
                return controller.HasClaim(new Claim(type, value));
            }

            public static void VerifyHasClaim(this ControllerBase controller,Claim claim)
            {
                if (!controller.HasClaim(claim))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            public static void VerifyHasClaim(this ControllerBase controller,string? type, string? value)
            {
                if (!controller.HttpContext.User.Claims.Any(c => (type == null || c.Type == type) &&
                    value == null || c.Value == value))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            public static void VerifyHasClaim(this ControllerBase controller,string[] anyOfTypes, string? value)
            {
                if (!controller.HttpContext.User.Claims.Any(c => anyOfTypes.Contains(c.Type) &&
                    value == null || c.Value == value))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            public static void VerifyHasClaim(this ControllerBase controller, string? type, string[] anyOfValues)
            {
                if (!controller.HttpContext.User.Claims.Any(c => (type == null || c.Type == type) &&
                                                                 anyOfValues.Contains(c.Value)))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }
}

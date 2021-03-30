#nullable enable
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace GenericCompany.Common.Authentication.Extensions
{
    public static class UserExtensions
    {
        public static string? GetClientToken(this ControllerBase controller)
        {
            var header = controller.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(header)) return null;
            return header.Trim().Split(" ").Last();
        }
        public static string? GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                   controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public static IPAddress? GetIpAddress(this ControllerBase controller)
        {
            // TODO: this might not be accurate behind a load balancer, so we might have to get X-Forwarded-For
            return controller.HttpContext.Connection.RemoteIpAddress;
        }
        public static string GetRoughLocation(this ControllerBase controller)
        {
            //TODO: Implement this
            return "Unknown";
        }
        public static string? GetUserAgent(this ControllerBase controller)
        {
            var containsData = controller.HttpContext.Request.Headers.TryGetValue("User-Agent", out var userAgent);
            return containsData ? userAgent.ToString() : null;
        }
    }
}

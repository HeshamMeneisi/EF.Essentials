using System;
using System.Linq;
using EF.Essentials.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EF.Essentials.Communication
{
    public class InternalApi : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.HttpContext.User.Claims.All(c => c.Type != CustomClaims.IsService))
            {
                throw new UnauthorizedAccessException();
            };
        }
    }
}

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace GenericCompany.Common.Logging
{
    public class LogActions : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
            var controllerName = ((ControllerActionDescriptor) context.ActionDescriptor).ControllerName;
            var actionName = ((ControllerActionDescriptor) context.ActionDescriptor).ActionName;

            if (context.Exception == null)
                Log
                    .ForContext("IPAddress", ipAddress)
                    .ForContext("ControllerName", controllerName)
                    .ForContext("ActionName", actionName)
                    .Information("Action Executed");
            else
                Log
                    .ForContext("IPAddress", ipAddress)
                    .ForContext("ControllerName", controllerName)
                    .ForContext("ActionName", actionName)
                    .ForContext("ErrorMessage", context.Exception.Message)
                    .Error("Action Errored");

            base.OnActionExecuted(context);
        }
    }
}

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GenericCompany.Common.StartupExtensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Fatal(contextFeature.Error, "GlobalError");

                        await context.Response.WriteAsJsonAsync(new ProblemDetails
                        {
                            Detail = contextFeature.Error.ToString(),
                            Title = "Unexpected Error",
                            Status = 500,
                            Instance = context.TraceIdentifier
                        });
                    }
                });
            });
        }
    }
}

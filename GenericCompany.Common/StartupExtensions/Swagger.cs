using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GenericCompany.Common.StartupExtensions
{
    public static class Swagger
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescription => apiDescription.Last());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Assembly.GetEntryAssembly()?.GetName()
                        .Name.Replace(".", ""),
                    Version = "v1"
                });
                c.CustomOperationIds(e =>
                {
                    var route = e.ActionDescriptor.RouteValues;
                    return route["action"];
                });
            });
            Console.WriteLine("Swagger JSON: http://localhost:5000/swagger/v1/swagger.json");
            Console.WriteLine("Swagger UI: http://localhost:5000/swagger/index.html");
        }

        public static void AddSwaggerWithUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "swagger";
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });
        }
    }
}

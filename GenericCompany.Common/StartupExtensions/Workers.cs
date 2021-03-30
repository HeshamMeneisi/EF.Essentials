using GenericCompany.Common.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace GenericCompany.Common.StartupExtensions
{
    public static class Workers
    {
        public static void RegisterBaseWorkers(this IServiceCollection services)
        {
            services.AddHostedService<SignatureKeyGenerationWorker>();
        }
    }
}

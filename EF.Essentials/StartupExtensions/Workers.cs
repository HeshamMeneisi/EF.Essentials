using EF.Essentials.Workers;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Essentials.StartupExtensions
{
    public static class Workers
    {
        public static void RegisterBaseWorkers(this IServiceCollection services)
        {
            services.AddHostedService<SignatureKeyGenerationWorker>();
        }
    }
}

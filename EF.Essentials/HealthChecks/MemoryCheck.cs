using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EF.Essentials.HealthChecks
{
    public class MemoryCheck : IHealthCheck
    {
        public static long Threshold = 50;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var allocated = GC.GetTotalMemory(true) / (1024.0 * 1024.0);
            var data = new Dictionary<string, object>
            {
                {"AllocatedMegabytes", allocated},
                {"Gen0Collections", GC.CollectionCount(0)},
                {"Gen1Collections", GC.CollectionCount(1)},
                {"Gen2Collections", GC.CollectionCount(2)}
            };
            var status = allocated <= Threshold ? HealthStatus.Healthy : HealthStatus.Degraded;
            var result = new HealthCheckResult(status, $"Memory used {allocated}/{Threshold}MB", null, data);
            return Task.FromResult(result);
        }
    }
}

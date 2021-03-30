using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GenericCompany.Common.HealthChecks
{
    public class ConnectionToDbCheck<T> : IHealthCheck where T : DbContext
    {
        private readonly T _db;

        public ConnectionToDbCheck(T db)
        {
            _db = db;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync(cancellationToken);
                return canConnect
                    ? new HealthCheckResult(HealthStatus.Healthy, "Database can connect")
                    : new HealthCheckResult(HealthStatus.Unhealthy, "database can't connect");
            }
            catch
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, "Database isn't reachable");
            }
        }
    }
}

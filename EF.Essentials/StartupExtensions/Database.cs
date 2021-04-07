using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EF.Essentials.StartupExtensions
{
    public static class Database
    {
        private static Exception _lastException;
        public static Exception LastException { get => _lastException; }

        public static async Task MigrateOrFail(this DatabaseFacade db, ILogger<DbContext> logger)
        {
            foreach(var waitTime in new[] {0, 1, 3, 8, 10})
            {
                logger.LogInformation($"Db connection attempt in {waitTime} seconds");
                await Task.Delay(TimeSpan.FromSeconds(waitTime));

                if (await TryMigrate(db, logger)) return;
                logger.LogWarning($"Connection failed...\nReason: {_lastException}");
            }

            throw new RetryLimitExceededException("Couldn't connect to database", _lastException);
        }

        private static async Task<bool> TryMigrate(DatabaseFacade db, ILogger<DbContext> logger)
        {
            try
            {
                var canConnect = await db.CanConnectAsync();
                if (!canConnect) return false;
                await db.MigrateAsync();
                return true;
            }
            catch(Exception ex)
            {
                _lastException = ex;
                logger.LogError(ex, "db connection failed");
                return false;
            }
        }
    }
}

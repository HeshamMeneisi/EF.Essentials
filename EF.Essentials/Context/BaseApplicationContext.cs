using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EF.Essentials.Config;
using EF.Essentials.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EF.Essentials.Context
{
    public class BaseApplicationContext : DbContext
    {
        private readonly DatabaseConfig _dbConfig;

        protected BaseApplicationContext(DbContextOptions options, IOptions<DatabaseConfig> dbOption)
        {
            _dbConfig = dbOption.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbHelper.ConfigureDb(optionsBuilder, _dbConfig);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbHelper.SetDatetimeFormat(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            DbHelper.CheckTimestamps(ChangeTracker.Entries());
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            DbHelper.CheckTimestamps(ChangeTracker.Entries());
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            DbHelper.CheckTimestamps(ChangeTracker.Entries());
            return base.SaveChangesAsync(cancellationToken);
        }

        public bool IsEntityAttached<T>(T entity) where T : class
        {
            return Set<T>().Local.Any(e => e == entity);
        }

        public void DetachEntity<T>(T attached) where T : class
        {
            Entry(attached).State = EntityState.Detached;
        }

        public object GetEntityKey<T>(T entity)
        {
            return DbHelper.GetEntityKey(entity, Model);
        }

        public void EnsureEntityIsAttached<T>(T entity) where T : class
        {
            var entityKey = GetEntityKey(entity);
            var attached = Set<T>().Local.FirstOrDefault(e => GetEntityKey(e).Equals(entityKey));
            if (attached == entity) return;
            if (attached != null) DetachEntity(attached);
            Entry(entity).State = EntityState.Modified;
            Set<T>().Attach(entity);
        }
    }
}

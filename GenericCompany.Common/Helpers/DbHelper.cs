using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GenericCompany.Common.Config;
using GenericCompany.Common.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql;

namespace GenericCompany.Common.Helpers
{
    public static class DbHelper
    {
        public static void ConfigureDb(DbContextOptionsBuilder optionsBuilder, DatabaseConfig dbConfig)
        {
            var connection = new NpgsqlConnectionStringBuilder
            {
                Host = dbConfig.Host,
                Port = dbConfig.Port,
                Database = dbConfig.Name,
                Username = dbConfig.User,
                Password = dbConfig.Password,
                SslMode = SslMode.Disable
            };
            optionsBuilder.UseNpgsql(connection.ConnectionString);
        }

        public static void SetDatetimeFormat(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            foreach (var property in entityType.GetProperties())
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(
                        new ValueConverter<DateTime, DateTime>(
                            v => v,
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    );
        }

        public static object GetEntityKey<T>(T entity, IModel model)
        {
            var keyName = model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
            var keyProp = entity.GetType().GetProperty(keyName);
            return keyProp != null ? keyProp.GetValue(entity, null) : null;
        }

        public static void CheckTimestamps(IEnumerable<EntityEntry> entityEntries)
        {
            var timestamp = DateTime.UtcNow;

            foreach (var entry in entityEntries)
            {
                if (!(entry.Entity is IWithTimestamps entityWithTimestamps)) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entityWithTimestamps.CreatedAt = entityWithTimestamps.UpdatedAt = timestamp;
                        break;
                    case EntityState.Modified:
                        entityWithTimestamps.UpdatedAt = timestamp;
                        break;
                }
            }
        }

        public static IQueryable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return outer
                .AsExpandable()// Tell LinqKit to convert everything into an expression tree.
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (outerItem, innerItems) => new { outerItem, innerItems })
                .SelectMany(
                    joinResult => joinResult.innerItems.DefaultIfEmpty(),
                    (joinResult, innerItem) =>
                        resultSelector.Invoke(joinResult.outerItem, innerItem));
        }
    }
}

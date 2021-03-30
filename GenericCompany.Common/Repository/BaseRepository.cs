using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GenericCompany.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GenericCompany.Common.Repository
{
    public class BaseRepository<T, TCtx> : IBaseRepository<T, TCtx> where T : class, IHasId where TCtx : DbContext
    {
        protected readonly DbSet<T> DbSet;

        public BaseRepository(TCtx context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public TCtx Context { get; }


        public virtual async Task<T> Find(string entityId)
        {
            return await DbSet.FindAsync(entityId);
        }

        public virtual async Task<T> Create(T entity)
        {
            entity.Id = DataHelper.GenerateUuid();
            if (entity is IHasSlug entityWithSlug) entityWithSlug.Slug = await GetValidSlug(entityWithSlug);
            var newEntity = (await DbSet.AddAsync(entity)).Entity;
            await Context.SaveChangesAsync();
            return newEntity;
        }

        public async Task<string> GetValidSlug(IHasSlug entityWithSlug)
        {
            var slug = StringHelper.UrlFriendly(entityWithSlug.GetSlugSource());
            if (await FindBySlug(slug) != null) slug += "-" + DateTimeHelper.DateTimeToBase64(DateTime.Now);

            return slug;
        }

        public virtual async Task<T> FindBySlug(string slug)
        {
            try
            {
                return (T) await DbSet.Cast<IHasSlug>()
                    .FirstOrDefaultAsync(e => e.Slug == slug);
            }
            catch (InvalidOperationException)
            {
                throw new EntityHasNoSlugException(typeof(T));
            }
        }

        public virtual async Task Delete(T entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<T> Update(T entity)
        {
            if (entity is IHasSlug entityWithSlug)
                if (entityWithSlug.Slug == null)
                    throw new NoNullAllowedException("Slug cannot be null");
            var newEntity = DbSet.Update(entity).Entity;
            await Context.SaveChangesAsync();
            return newEntity;
        }

        public virtual async Task<IEnumerable<T>> ListAll()
        {
            return await DbSet.ToArrayAsync();
        }
    }
}

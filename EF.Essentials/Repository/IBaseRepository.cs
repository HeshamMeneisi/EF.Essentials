using System.Collections.Generic;
using System.Threading.Tasks;
using EF.Essentials.Model;
using Microsoft.EntityFrameworkCore;

namespace EF.Essentials.Repository
{
    public interface IBaseRepository<T, TCtx> : IEntityCRUD<T> where TCtx: DbContext
    {
        TCtx Context { get; }
        Task<IEnumerable<T>> ListAll();
        Task<T> FindBySlug(string slug);
        Task<string> GetValidSlug(IHasSlug entity);
    }
}

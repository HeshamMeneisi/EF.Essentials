using System.Collections.Generic;
using System.Threading.Tasks;
using GenericCompany.Common.Model;
using Microsoft.EntityFrameworkCore;

namespace GenericCompany.Common.Repository
{
    public interface IBaseRepository<T, TCtx> : IEntityCRUD<T> where TCtx: DbContext
    {
        TCtx Context { get; }
        Task<IEnumerable<T>> ListAll();
        Task<T> FindBySlug(string slug);
        Task<string> GetValidSlug(IHasSlug entity);
    }
}

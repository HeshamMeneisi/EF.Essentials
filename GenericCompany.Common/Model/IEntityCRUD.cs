using System.Threading.Tasks;

namespace GenericCompany.Common.Model
{
    public interface IEntityCRUD<T>
    {
        public Task<T> Find(string entityId);
        public Task<T> Create(T entity);
        public Task Delete(T entity);
        public Task<T> Update(T entity);
    }
}

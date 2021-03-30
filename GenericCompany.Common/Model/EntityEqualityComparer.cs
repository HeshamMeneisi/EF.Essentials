using System.Collections.Generic;
using GenericCompany.Common.Repository;

namespace GenericCompany.Common.Model
{
    public class EntityEqualityComparer<T> : IEqualityComparer<T> where T : class, IHasId
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null) return x == y;
            return x.Id == y.Id;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}

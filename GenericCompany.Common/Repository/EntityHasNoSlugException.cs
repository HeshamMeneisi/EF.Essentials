using System;

namespace GenericCompany.Common.Repository
{
    public class EntityHasNoSlugException : Exception
    {
        public EntityHasNoSlugException(Type type) : base($"{type.FullName} does not implements IHasSlug")
        {
        }
    }
}

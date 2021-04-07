using System;

namespace EF.Essentials.Repository
{
    public class EntityHasNoSlugException : Exception
    {
        public EntityHasNoSlugException(Type type) : base($"{type.FullName} does not implements IHasSlug")
        {
        }
    }
}

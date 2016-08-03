using ECS.Interfaces;
using System;

namespace ECS
{
    public static class EcsUtils
    {
        static public IComponentContainer AddToPool(this IEntityPool pool, IEntity entity)
        {
            if (pool == null)
                throw new ArgumentNullException("pool");
            if (entity == null)
                throw new ArgumentNullException("entity");
            pool.Add(entity);
            return pool.GetComponents(entity);
        }
    }
}

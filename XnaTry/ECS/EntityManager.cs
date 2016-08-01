using System;
using System.Collections.Generic;

namespace ECS
{
    public class EntityManager
    {
        public EntityPool EntityPool { get; }

        public EntityManager()
        {
            EntityPool = new EntityPool();
        }

        public IEntity CreateEntity()
        {
            IEntity entity = new Entity(Guid.NewGuid());
            EntityPool.Add(entity);
            return entity;
        }
    }
}

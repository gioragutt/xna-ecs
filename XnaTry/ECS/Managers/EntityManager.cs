using System;
using ECS.BaseTypes;
using ECS.Interfaces;

namespace ECS.Managers
{
    public class EntityManager
    {
        public IEntityPool EntityPool { get; }

        internal EntityManager(IEntityPool pool = null)
        {
            EntityPool = pool ?? new EntityPool();
        }

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
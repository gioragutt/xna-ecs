using System;
using ECS.BaseTypes;
using ECS.Interfaces;

namespace ECS.Managers
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

    // TODO: Create a system manager class, a test system, and test whether update was called with CounterComponent
}
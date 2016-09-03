using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using System.Collections.Concurrent;

namespace ECS.BaseTypes
{
    public class EntityPool : IEntityPool
    {
        protected readonly ConcurrentDictionary<IEntity, IComponentContainer> entities;

        public EntityPool()
        {
            entities = new ConcurrentDictionary<IEntity, IComponentContainer>();
        }

        public EntityPool(EntityPool other)
        {
            entities = other.entities;
        }

        static void AssertParameterNotNull(object entity, string name)
        {
            if (entity == null)
                throw new ArgumentNullException(name);
        }

        public IList<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent
        {
            return
                entities.Values.Where(components => components.Has<TComponent>()).SelectMany(
                    components => components.GetAllOf<TComponent>()).ToList();
        }

        public IList<IComponentContainer> AllThat(Predicate<IComponentContainer> predicate)
        {
            return predicate == null
                ? default(IList<IComponentContainer>)
                : entities.Values.Where(c => predicate(c)).ToList();
        }

        public bool Exists(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");
            return entities.ContainsKey(entity);
        }

        public IComponentContainer GetComponents(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");
            IComponentContainer componentContainer;
            return entities.TryGetValue(entity, out componentContainer) ? componentContainer : null;
        }

        public void Add(IEntity entity, IComponentContainer container)
        {
            AssertParameterNotNull(entity, "entity");
            AssertParameterNotNull(container, "container");

            entity.Parent = this;
            entities.TryAdd(entity, container);
        }

        public void Add(IEntity entity)
        {
            AssertParameterNotNull(entity, "entity");

            Add(entity, new ComponentContainer(entity));
        }

        public void Remove(IEntity entity)
        {
            if (!entities.ContainsKey(entity))
                return;

            IComponentContainer outVal;
            entities.TryRemove(entity, out outVal);
        }

        public int Count => entities.Count;
        public IList<IEntity> AllEntities => entities.Keys.ToList();
    }
}

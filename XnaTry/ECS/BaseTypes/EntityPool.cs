using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class EntityPool : Dictionary<IEntity, IComponentContainer>, IEntityPool
    {
        public EntityPool() { }

        public EntityPool(EntityPool pool)
        {
            foreach (var entity in pool)
            {
                Add(entity.Key, entity.Value);
            }
        }

        public EntityPool(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);   
            }
        }

        static void AssertEntityNotNull(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
        }

        public ICollection<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent
        {
            return Values.Where(components => components.Has<TComponent>()).
                SelectMany(components => components.GetAllOf<TComponent>()).ToList();
        }

        public bool Exists(IEntity entity)
        {
            AssertEntityNotNull(entity);
            return ContainsKey(entity);
        }

        public IComponentContainer GetComponents(IEntity entity)
        {
            AssertEntityNotNull(entity);
            IComponentContainer componentContainer;
            return TryGetValue(entity, out componentContainer) ? componentContainer : null;
        }

        public void Add(IEntity entity)
        {
            AssertEntityNotNull(entity);

            if (Exists(entity))
                return;

            Add(entity, new ComponentContainer());
        }
    }
}

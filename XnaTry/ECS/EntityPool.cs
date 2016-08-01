using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
    public class EntityPool : Dictionary<IEntity, IComponentContainer>, IEntityPool
    {
        static void AssertEntityNotNull(IEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
        }
        public ICollection<IEntity> GetAllOf<TComponent>() where TComponent : class, IComponent
        {
            return Keys.Where(entity => this[entity].HasComponent<TComponent>()).ToList();
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

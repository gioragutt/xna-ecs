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

        public ICollection<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent
        {
            ICollection<TComponent> components = new List<TComponent>();

            foreach (var container in Values)
            {
                if (container.HasComponent<TComponent>())
                    components.Add(container.GetComponent<TComponent>());
            }

            return components;
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

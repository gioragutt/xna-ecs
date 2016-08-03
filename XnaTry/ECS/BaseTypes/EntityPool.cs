using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECS.BaseTypes
{
    public class EntityPool : Dictionary<IEntity, IComponentContainer>, IEntityPool
    {
        public EntityPool() { }

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

        public IEnumerable<IComponentContainer> AllThat(Predicate<IComponentContainer> predicate)
        {
            return predicate == null ? default(IEnumerable<IComponentContainer>) : Values.Where(c => predicate(c));
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

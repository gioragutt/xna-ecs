using System;
using System.Collections.Generic;

namespace ECS
{
    public class ComponentContainer : Dictionary<Type, IComponent>, IComponentContainer
    {
        public void AddComponent<TComponent>(TComponent component) where TComponent : class, IComponent
        {
            if (HasComponent<TComponent>())
                return;

            Add(typeof(TComponent), component);
        }

        public TComponent GetComponent<TComponent>() where TComponent : class, IComponent
        {
            IComponent component;
            return TryGetValue(typeof (TComponent), out component) ? (TComponent) component : null;
        }

        public bool HasComponent<TComponent>() where TComponent : class, IComponent
        {
            return ContainsKey(typeof(TComponent));
        }
    }
}

using System.Collections.Generic;

namespace ECS
{
    public interface IEntityPool
    {
        ICollection<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent;
        bool Exists(IEntity entity);
        IComponentContainer GetComponents(IEntity entity);
        void Add(IEntity entity);
        int Count { get; }
    }
}
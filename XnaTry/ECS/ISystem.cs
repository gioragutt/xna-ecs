using System.Collections.Generic;

namespace ECS
{
    public interface ISystem
    {
        /// <summary>
        /// Update a set of entities based on shared logic
        /// </summary>
        /// <param name="entities">The components that this system is in charge of</param>
        /// <param name="delta">Milliseconds since last update</param>
        void Update(ICollection<IComponent> entities, long delta);
    }

    public interface ISystem<TComponent> : ISystem where TComponent : class, IComponent
    {
        /// <summary>
        /// Update a set of entities based on shared logic
        /// </summary>
        /// <param name="entities">The components that this system is in charge of</param>
        /// <param name="delta">Milliseconds since last update</param>
        void Update(ICollection<TComponent> entities, long delta);
    }
}

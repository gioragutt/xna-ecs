using ECS.Interfaces;
using System.Collections.Generic;

namespace XnaTryLib.ECS
{
    public abstract class BaseSystem<T> : ISystem<T> where T : class, IComponent
    {
        public bool Enabled { get; set; }

        protected BaseSystem(bool enabled = true) { Enabled = enabled; } 

        /// <summary>
        /// By default, Calls update on all entities deemed relevant by GetRelevant
        /// </summary>
        /// <param name="pool">Pool of entities available in the game</param>
        /// <param name="delta">Milliseconds since last update</param>
        public virtual void Update(IEntityPool pool, long delta)
        {
            Update(GetRelevant(pool), delta);
        }

        public abstract void Update(ICollection<IComponentContainer> entities, long delta);
        public abstract ICollection<IComponentContainer> GetRelevant(IEntityPool pool);
    }
}

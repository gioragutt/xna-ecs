using System.Collections.Generic;
using ECS.Interfaces;

namespace XnaTryLib.ECS.Systems
{
    public abstract class System : ISystem
    {
        public bool Enabled { get; set; }

        protected System(bool enabled = true) { Enabled = enabled; } 

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

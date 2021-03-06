﻿using System.Collections.Generic;

namespace ECS.Interfaces
{
    public interface ISystem
    {
        bool Enabled { get; set; }

        /// <summary>
        /// Update a set of entities based on shared logic
        /// </summary>
        /// <param name="pool">The pool of available entities</param>
        /// <param name="delta">Milliseconds since last update</param>
        void Update(IEntityPool pool, long delta);

        /// <summary>
        /// Update a set of entities based on shared logic
        /// </summary>
        /// <param name="entities">The components that this system is in charge of</param>
        /// <param name="delta">Milliseconds since last update</param>
        void Update(IList<IComponentContainer> entities, long delta);

        /// <summary>
        /// Allows to get only the relevant components
        /// </summary>
        /// <param name="pool">The available pool of entities</param>
        /// <returns>An collection of all relevant components for the system</returns>
        IList<IComponentContainer> GetRelevant(IEntityPool pool);
    }
}

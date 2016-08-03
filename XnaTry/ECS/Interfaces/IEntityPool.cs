using System;
using System.Collections.Generic;

namespace ECS.Interfaces
{
    public interface IEntityPool
    {
        /// <summary>
        /// Gets all components (from all entities) of type TComponent
        /// </summary>
        /// <typeparam name="TComponent">The type of component to insert</typeparam>
        /// <returns>A collection of all components in the pool</returns>
        ICollection<TComponent> GetAllOf<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Checks if the entity exists in the pool
        /// </summary>
        /// <param name="entity">Entity to check</param>
        /// <returns>true if entity is in the pool; otherwise false</returns>
        /// <exception cref="System.ArgumentNullException">If entity is null</exception>
        bool Exists(IEntity entity);

        /// <summary>
        /// Gets the component container of a given container
        /// </summary>
        /// <param name="entity">Entity to get</param>
        /// <returns>IComponentContainer of the entity</returns>
        /// <exception cref="System.ArgumentNullException">If entity is null</exception>
        IComponentContainer GetComponents(IEntity entity);

        /// <summary>
        /// Gets all component containers that fall under the given predicate
        /// </summary>
        /// <param name="predicate">The predicate with which to filter containers</param>
        /// <returns>An enumeration of all containers that fall under the predicate</returns>
        IEnumerable<IComponentContainer> AllThat(Predicate<IComponentContainer> predicate); 

        /// <summary>
        /// Add an entity with an empty component container
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        void Add(IEntity entity);

        /// <summary>
        /// Adds an entity with an existing component container
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <param name="container">The component container of the entity</param>
        void Add(IEntity entity, IComponentContainer container);

        /// <summary>
        /// Count of entities in the pool
        /// </summary>
        int Count { get; }
    }
}
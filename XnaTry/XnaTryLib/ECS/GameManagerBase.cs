using System;
using ECS.Interfaces;
using ECS.Managers;

namespace XnaCommonLib.ECS
{
    public abstract class GameManagerBase
    {
        protected EntityManager entityManager;
        protected SystemManager nonDrawingSystems;
        public IEntityPool EntityPool { get; }

        protected GameManagerBase()
        {
            entityManager = new EntityManager();   
            nonDrawingSystems = new SystemManager(entityManager.EntityPool);
            EntityPool = entityManager.EntityPool;
        }

        public int EntitiesCount => entityManager.EntityPool.Count;

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject CreateGameObject()
        {
            var newGameObject = new GameObject(Guid.NewGuid());
            EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject CreateGameObject(Guid id)
        {
            var newGameObject = new GameObject(id);
            EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            nonDrawingSystems.AddSystem(system);
        }

        protected abstract void Update(long delta);
    }
}
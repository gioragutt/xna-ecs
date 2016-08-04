using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Interfaces;
using ECS.Managers;
using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS
{
    public class GameManager
    {
        private readonly EntityManager entityManager;
        private readonly SystemManager nonDrawingSystems;
        private readonly SystemManager drawingSystems;

        public GameManager()
        {
            entityManager = new EntityManager();    
            nonDrawingSystems = new SystemManager(entityManager.EntityPool);
            drawingSystems = new SystemManager(entityManager.EntityPool);
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            nonDrawingSystems.AddSystem(system);
        }

        public void RegisterDrawingSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            drawingSystems.AddSystem(system);
        }

        public GameObject CreateGameObject()
        {
            var newGameObject = new GameObject();
            entityManager.EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        public void Update(GameTime gameTime) { Update(gameTime.ElapsedGameTime.Milliseconds); }
        private void Update(long delta) { nonDrawingSystems.Update(delta); }

        public void Draw(GameTime gameTime) { Draw(gameTime.ElapsedGameTime.Milliseconds); }
        private void Draw(long delta) { drawingSystems.Update(delta); }
    }
}

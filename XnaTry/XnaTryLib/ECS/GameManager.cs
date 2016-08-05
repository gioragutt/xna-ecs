using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Interfaces;
using ECS.Managers;
using Microsoft.Xna.Framework;
using XnaTryLib.ECS.Components;

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

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject CreateGameObject()
        {
            var newGameObject = new GameObject();
            entityManager.EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        #region Systems API

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            nonDrawingSystems.AddSystem(system);
        }

        public void RegisterDrawingSystem<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            drawingSystems.AddSystem(system);
        }

        #endregion

        #region Debug Prints API

        /// <summary>
        /// Adds a debug print component with a reference to an object to print
        /// </summary>
        /// <param name="obj">GameObject to add component to</param>
        /// <param name="value">object reference to print</param>
        /// <param name="color">Color of test; defaults to Blue if color is null</param>
        public void AddDebugPrint(GameObject obj, object value, Color? color = null)
        {
            obj.Components.Add(new DebugPrintText
            {
                PrintValue = value,
                Color = color ?? Color.Blue
            });
        }

        /// <summary>
        /// Adds a debug print component with a reference to an object to print
        /// </summary>
        /// <param name="obj">GameObject to add component to</param>
        /// <param name="valueGetter">Function to get the string to print</param>
        /// <param name="color">Color of test; defaults to Blue if color is null</param>
        public void AddDebugPrint(GameObject obj, Func<string> valueGetter, Color? color = null)
        {
            obj.Components.Add(new DebugPrintText
            {
                PrintFunc = valueGetter,
                Color = color ?? Color.Blue
            });
        }

        /// <summary>
        /// Creates a new GameObject with a debug print attached to it, printing the references object
        /// </summary>
        /// <param name="value">object reference to print</param>
        /// <param name="color">Color of test; defaults to Blue if color is null</param>
        /// <returns>The newly created game object</returns>
        public GameObject CreateDebugPrint(object value, Color? color = null)
        {
            var newGameObject = CreateGameObject();
            AddDebugPrint(newGameObject, value, color);
            return newGameObject;
        }

        /// <summary>
        /// Creates a new GameObject with a debug print attached to it, printing the string receives by the valueGetter
        /// </summary>
        /// <param name="valueGetter">Function to get the string to print</param>
        /// <param name="color">Color of test; defaults to Blue if color is null</param>
        /// <returns>The newly created game object</returns>
        public GameObject CreateDebugPrint(Func<string> valueGetter, Color? color = null)
        {
            var newGameObject = CreateGameObject();
            AddDebugPrint(newGameObject, valueGetter, color);
            return newGameObject;
        }

        #endregion

        #region Game Loop API

        public void Update(GameTime gameTime)
        {
            Update(gameTime.ElapsedGameTime.Milliseconds);
        }
        private void Update(long delta)
        {
            nonDrawingSystems.Update(delta);
        }

        public void Draw(GameTime gameTime)
        {
            Draw(gameTime.ElapsedGameTime.Milliseconds);
        }
        private void Draw(long delta)
        {
            drawingSystems.Update(delta);
        }

        #endregion
    }
}

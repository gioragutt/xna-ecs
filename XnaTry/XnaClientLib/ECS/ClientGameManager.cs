using ECS.Interfaces;
using ECS.Managers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaClientLib.ECS.Compnents;
using XnaClientLib.ECS.Linkers;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS
{
    public class ClientGameManager : GameManagerBase
    {
        #region Properties and Variables

        private readonly SystemManager drawingSystems;
        private ResourcesManager ResourceManager { get; }
        public Dictionary<string, TeamData> Teams { get; set; }
        public Dictionary<string, string> TeamFrameTextures { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new ClientGameManager
        /// </summary>
        /// <param name="resourceManager">The games ResouceManager</param>
        public ClientGameManager(ResourcesManager resourceManager)
        {
            ResourceManager = resourceManager;
            drawingSystems = new SystemManager(EntityPool);
        }

        #endregion

        #region Helping Mehthods

        public override string ToString()
        {
            var builder = new StringBuilder();
            var allplayers = entityManager.EntityPool.AllThat(c => c.Has<PlayerAttributes>()).ToList();
            allplayers.Sort(ComparePlayerHealth);
            builder.AppendFormat("Total entities: {0} of which {1} are players{2}", EntityPool.Count, allplayers.Count, Environment.NewLine);

            foreach (var entity in allplayers)
            {
                var attributes = entity.Get<PlayerAttributes>();

                builder.AppendFormat("{0} - ( {1} ) {2}% HP{3}", attributes.Name, entity.Count, attributes.HealthPercentage * 100f, Environment.NewLine);
            }

            return builder.ToString();
        }

        private static int ComparePlayerHealth(IComponentContainer first,
                                               IComponentContainer second)
        {
            if (first == null)
                return 1;
            if (second == null)
                return 0;
            var firstHealth = first.Get<PlayerAttributes>().HealthPercentage;
            var secondHealth = second.Get<PlayerAttributes>().HealthPercentage;

            if (firstHealth > secondHealth)
                return 1;
            if (secondHealth < firstHealth)
                return -1;
            return 0;
        }

        #endregion

        #region Systems API

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
        public void AppendDebugPrint(GameObject obj, object value, Color? color = null)
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
        public void AppendDebugPrint(GameObject obj, Func<string> valueGetter, Color? color = null)
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
            AppendDebugPrint(newGameObject, value, color);
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
            AppendDebugPrint(newGameObject, valueGetter, color);
            return newGameObject;
        }

        #endregion

        #region Game Loop API

        public void Update(GameTime gameTime)
        {
            Update(gameTime.ElapsedGameTime.Milliseconds);
        }

        protected override void Update(long delta)
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

        #region GameObject Methods

        /// <summary>
        /// Initializes a remote client with game components
        /// </summary>
        /// <param name="go">The local player's GameObject</param>
        public void InitializeLocalClient(GameObject go)
        {
            var components = go.Components;

            // Now add input
            components.Add(new Velocity(new Vector2(2)));
            components.Add(new KeyboardDirectionalInput());

            // Show a character
            var sprite = ResourceManager.Register(new Sprite("Player/Images/Down_001"));
            components.Add(sprite);

            // Change Transform
            go.Transform.Scale = 0.4f;

            // Add Animation
            AddAnimation(sprite, components);

            var attributes = go.Components.Get<PlayerAttributes>();
            attributes.Team = Teams[attributes.Team.Name];

            AddStatusBar(components, sprite, attributes);

            components.Add(new LocalPlayer(go));
        }

        /// <summary>
        /// Initializes a remote client with game components
        /// </summary>
        /// <param name="go">The GameObject of the remote game client</param>
        public void InitializeRemoteClient(GameObject go)
        {
            var components = go.Components;

            var sprite = ResourceManager.Register(new Sprite("Player/Images/Down_001"));
            components.Add(sprite);

            AddAnimation(sprite, components);
            AddStatusBar(components, sprite, components.Get<PlayerAttributes>());
        }

        /// <summary>
        /// Creates a new GameObject for a remote player
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject AllocateGameObjectForRemote(Guid id)
        {
            var newGameObject = new GameObject(id);
            newGameObject.Components.Add(new PlayerAttributes());
            newGameObject.Components.Add(new InputData());
            newGameObject.Components.Add(new RemotePlayer(newGameObject));
            EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        #endregion

        #region Component Initializing Methods

        /// <summary>
        /// Inserts the StatusBar gui component to a component container
        /// </summary>
        /// <param name="components">The component container to add the component to</param>
        /// <param name="sprite">The sprite of the game object</param>
        /// <param name="attributes">The player attributes of the object</param>
        private void AddStatusBar(IComponentContainer components, Sprite sprite, PlayerAttributes attributes)
        {
            components.Add(
                ResourceManager.Register(new PlayerStatusBar(attributes, sprite, components.Get<Transform>(),
                    Constants.Assets.PlayerHealthBarAsset, Constants.Assets.PlayerNameFontAsset,
                    TeamFrameTextures[attributes.Team.Name])));
        }

        /// <summary>
        /// Inserts animation related components into a component container
        /// </summary>
        /// <param name="sprite">The sprite of the game object</param>
        /// <param name="components">The component container the components are inserted into</param>
        private void AddAnimation(Sprite sprite, IComponentContainer components)
        {
            const long msPerFrame = 100;
            var stateAnimation = new StateAnimation<MovementDirection>(sprite, 0, MovementDirection.Down,
                new Dictionary<MovementDirection, Animation>
                {
                    [MovementDirection.Down] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Util.FormatRange("Player/Images/Down_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Up] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Util.FormatRange("Player/Images/Up_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Left] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Util.FormatRange("Player/Images/Left_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Right] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Util.FormatRange("Player/Images/Right_{0:D3}", 1, 4), msPerFrame))
                });

            components.Add(stateAnimation);

            // Link Input to Animation
            components.Add(new MovementToAnimationLinker(components.Get<DirectionalInput>(), stateAnimation));
        }

        #endregion Component Initializing Methods
    }
}

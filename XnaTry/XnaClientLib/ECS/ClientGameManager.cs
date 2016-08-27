using ECS.BaseTypes;
using ECS.Interfaces;
using ECS.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Utility;
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
        public ResourcesManager ResourceManager { get; }
        public Dictionary<string, TeamData> Teams { get; set; }
        public Camera Camera { get; }
        private GameObject LocalPlayer { get; set; }

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
            Camera = new Camera();

            Subscribe(Constants.Messages.ClientDisconnected, Callback_ClientDisconnected);
        }

        #endregion

        #region Callbacks

        private void Callback_ClientDisconnected(JObject message)
        {
            var guid = message.GetGuid(Constants.Fields.PlayerGuid);

            if (guid.HasValue)
                EntityPool.Remove(new Entity(guid.Value));
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

        public void Update(GameTime gameTime, Viewport viewport)
        {
            var delta = gameTime.ElapsedGameTime.Milliseconds;
            Update(delta);

            if (LocalPlayer == null)
                return;

            LocalPlayer.Components.Get<DirectionalInput>().Update(delta);
            Camera.UpdateCamera(LocalPlayer, viewport);
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
        /// Finishes creating a game object
        /// </summary>
        /// <param name="go">The GameObject to finish creating</param>
        public void EndAllocate(GameObject go)
        {
            AddCommonComponents(go.Components);
        }

        /// <summary>
        /// Initializes all components commion to local and remote players
        /// </summary>
        /// <param name="components">The component container to add the components to</param>
        private void AddCommonComponents(IComponentContainer components)
        {
            AddSprite(components);
            AddAnimation(components);
            AddStatusBar(components);
            AddPlayerEffect(components);
            AddInterpolation(components);
        }

        private static DirectionalInput InputForLocal()
        {
#if WINDOWS
            return new KeyboardDirectionalInput();
#else
            return new GamepadDirectionalInput();
#endif
        }

        /// <summary>
        /// Creates a new GameObject for a remote player
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject BeginAllocateRemote(Guid id)
        {
            var newGameObject = new GameObject(id);
            newGameObject.Components.Add(new PlayerAttributes());
            newGameObject.Components.Add(new InputData());
            newGameObject.Components.Add(new Velocity(Vector2.Zero));
            newGameObject.Components.Add(new RemotePlayer(newGameObject));
            EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        /// <summary>
        /// Creates a new GameObject for a local player
        /// </summary>
        /// <returns>The newly created game object</returns>
        public GameObject BeginAllocateLocal(Guid id)
        {
            var newGameObject = new GameObject(id);
            var components = newGameObject.Components;
            components.Add(new PlayerAttributes());
            components.Add(new Velocity(Vector2.Zero));
            components.Add(InputForLocal());
            components.Add(new LocalPlayer(newGameObject));
            LocalPlayer = newGameObject;
            EntityPool.Add(newGameObject.Entity, newGameObject.Components);
            return newGameObject;
        }

        #endregion

        #region Component Initializing Methods

        /// <summary>
        /// Initializes the Sprite component
        /// </summary>
        /// <param name="components">The component container to add the component to</param>
        public void AddSprite(IComponentContainer components)
        {
            components.Add(ResourceManager.Register(new Sprite("Player/Images/Down_001")));
        }

        /// <summary>
        /// Initializes the PlayerStatusBar component
        /// </summary>
        /// <param name="components">The component container to add the component to</param>
        private void AddStatusBar(IComponentContainer components)
        {
            var sprite = components.Get<Sprite>();
            var attributes = components.Get<PlayerAttributes>();
            attributes.Team = Teams[attributes.Team.Name];
            components.Add(
                ResourceManager.Register(new PlayerStatusBar(attributes, sprite, components.Get<Transform>(),
                    Constants.Assets.PlayerHealthBar, Constants.Assets.PlayerNameFont)));
        }

        /// <summary>
        /// Initializes the Animation component
        /// </summary>
        /// <param name="components">The component container the components are inserted into</param>
        private void AddAnimation(IComponentContainer components)
        {
            var sprite = components.Get<Sprite>();
            const long msPerFrame = 100;
            var stateAnimation = new StateAnimation<MovementDirection>(sprite, 0, MovementDirection.Down,
                new Dictionary<MovementDirection, Animation>
                {
                    [MovementDirection.Down] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Utils.FormatRange("Player/Images/Down_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Up] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Utils.FormatRange("Player/Images/Up_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Left] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Utils.FormatRange("Player/Images/Left_{0:D3}", 1, 4), msPerFrame)),
                    [MovementDirection.Right] =
                        ResourceManager.Register(new TextureCollectionAnimation(sprite,
                            Utils.FormatRange("Player/Images/Right_{0:D3}", 1, 4), msPerFrame))
                });

            components.Add(stateAnimation);

            // Link Input to Animation
            components.Add(new MovementToAnimationLinker(components, stateAnimation));
        }

        /// <summary>
        /// Initializes the PlayerEffects component
        /// </summary>
        /// <param name="components">The component container to add the component to</param>
        private void AddPlayerEffect(IComponentContainer components)
        {
            var effect = new SpriteEffect("Player/Effects/PlayerEffects");
            components.Add(ResourceManager.Register(effect));

            components.Add(new ActionLinker<IComponentContainer, SpriteEffect>(components, effect, (c, s) =>
            {
                var attr = c.Get<PlayerAttributes>();
                if (attr.IsDead)
                {
                    s.ApplyPass("Ghost");
                    return;
                }
                if (attr.PreviousHealth < attr.Health)
                {
                    s.ApplyPass("Healed");
                    return;
                }
                if (attr.PreviousHealth > attr.Health)
                {
                    s.ApplyPass("Hit");
                    return;
                }

                s.ResetPass();
            }));
        }

        /// <summary>
        /// Initializes the Interpolator component
        /// </summary>
        /// <param name="components">The component container to add the component to</param>
        public void AddInterpolation(IComponentContainer components)
        {
            components.Add(new Interpolator());
        }

        #endregion Component Initializing Methods
    }
}

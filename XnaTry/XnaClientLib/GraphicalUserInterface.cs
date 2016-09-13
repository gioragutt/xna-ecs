using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using XnaClientLib.ECS;
using XnaClientLib.ECS.Compnents.GUI;
using XnaClientLib.ECS.Compnents.GUI.TimedMessageBox;
using XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style;

namespace XnaClientLib
{
    public class GraphicalUserInterface
    {
        #region Fields

        private readonly GameMinimap minimap;
        private readonly Label pingLabel;
        private readonly TimedMessageBox messageBox;

        #endregion Fields

        #region Properties

        public GameMap Map { get; }

        #endregion

        #region Constructor

        public GraphicalUserInterface(ClientGameManager gameManager, string tmxMapName, Func<string> pingGetter)
        {
            var gameObject = gameManager.CreateGameObject();
            Map = new GameMap(tmxMapName);
            gameObject.Components.Add(gameManager.ResourceManager.Register(Map));
            minimap = new GameMinimap(Map, Constants.Assets.PlayerNameFont, gameManager);
            gameObject.Components.Add(gameManager.ResourceManager.Register(minimap));
            pingLabel = new Label(pingGetter, Color.LightGreen, Constants.Assets.DefaultFont, new Vector2(5, 5));
            gameObject.Components.Add(gameManager.ResourceManager.Register(pingLabel));
            messageBox = new TimedMessageBox(Constants.Assets.DefaultFont)
            {
                Position = new Vector2(5, 5),
                MaxTime = TimeSpan.FromSeconds(3),
                Style = TimedMessageBoxStyle.Fading,
                Color = Color.White
            };
            gameObject.Components.Add(gameManager.ResourceManager.Register(messageBox));
        }

        #endregion Constructor

        #region Initialization

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            var minimapHeight = (int)minimap.MinimapSize.Y;
            var upperViewportBounds = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height - minimapHeight);
            var upperViewport = new Viewport(upperViewportBounds);
            var lowerViewport = new Viewport(0, upperViewport.Height, upperViewport.Width, minimapHeight);
            graphicsDevice.Viewport = upperViewport;
            minimap.Viewport = lowerViewport;
            pingLabel.Viewport = lowerViewport;
            messageBox.Viewport = upperViewport;
        }

        #endregion
    }
}

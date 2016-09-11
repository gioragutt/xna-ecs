using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class GameMinimap : GameMap
    {
        #region Fields

        private readonly string mapFontAsset;
        private readonly ClientGameManager gameManager;
        private SpriteFont mapFont;

        #endregion

        #region Properties

        public Vector2 MinimapSize => new Vector2(map.MapWidth, map.MapHeight) * Constants.GUI.MinimapSize;

        #endregion Properties

        #region Constructor

        public GameMinimap(GameMap gameMap, string mapFontAsset, ClientGameManager gameManager)
            : base(gameMap.TmxMapName)
        {
            this.mapFontAsset = mapFontAsset;
            this.gameManager = gameManager;
        }

        #endregion

        #region GuiComponent Methods

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            mapFont = content.Load<SpriteFont>(mapFontAsset);
        }

        public override int DrawOrder => Constants.GUI.DrawOrder.Hud;

        public override bool IsHud => true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var minimapPosition = MinimapPosition(spriteBatch.GraphicsDevice.Viewport, MinimapSize);
            DrawMap(spriteBatch, Constants.GUI.MinimapSize, minimapPosition);
            DrawPlayersPosition(spriteBatch, Constants.GUI.MinimapSize, minimapPosition);
        }

        #endregion

        #region Helper Methods

        private void DrawPlayersPosition(SpriteBatch spriteBatch, float factor = 1.0f, Vector2? origin = null)
        {
            if (!Utils.NotNull(mapFont, gameManager))
                return;

            origin = origin ?? Vector2.Zero;

            const string playerOnMap = "*";
            var dotSize = mapFont.MeasureString(playerOnMap);
            var players = gameManager.EntityPool.AllThat(c => c.Has<Transform>() && c.Has<PlayerAttributes>());

            foreach (var player in players)
                DrawPlayerPositionOnMap(spriteBatch, factor, origin.Value, player, playerOnMap, dotSize);
        }

        private void DrawPlayerPositionOnMap(
            SpriteBatch spriteBatch,
            float factor,
            Vector2 origin,
            IComponentContainer player,
            string playerOnMap,
            Vector2 dotSize)
        {
            var position = player.Get<Transform>().Position;
            var teamColor = DecideTeamColor(player);
            spriteBatch.DrawString(mapFont, playerOnMap, origin + position * factor - dotSize / 2f, teamColor);
        }

        private Color DecideTeamColor(IComponentContainer player)
        {
            var attributes = player.Get<PlayerAttributes>();
            if (Equals(player.Parent, gameManager.LocalPlayer.Entity))
                return Color.LightGreen;
            if (attributes.IsDead)
                return Color.Gray;
            return attributes.Team.Color;
        }

        private static Vector2 MinimapPosition(Viewport viewport, Vector2 minimapSize)
        {
            return new Vector2(viewport.Width, viewport.Height) - minimapSize;
        }

        #endregion
    }
}
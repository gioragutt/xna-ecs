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
        private readonly string mapFontAsset;
        private readonly ClientGameManager gameManager;
        private SpriteFont mapFont;

        public GameMinimap(GameMap gameMap, string mapFontAsset, ClientGameManager gameManager) 
            : base(gameMap.TmxMapName)
        {
            this.mapFontAsset = mapFontAsset;
            this.gameManager = gameManager;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            mapFont = content.Load<SpriteFont>(mapFontAsset);
        }

        public override int DrawOrder()
        {
            return Constants.GUI.DrawOrder.Minimap;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            PrepareNonCameraSpriteBatch(spriteBatch);
            var minimapSize = new Vector2(map.MapWidth, map.MapHeight) * Constants.GUI.MinimapSize;
            var minimapPosition = MinimapPosition(spriteBatch.GraphicsDevice.Viewport, minimapSize);
            DrawMap(spriteBatch, Constants.GUI.MinimapSize, minimapPosition);
            DrawPlayersPosition(spriteBatch, Constants.GUI.MinimapSize, minimapPosition);
        }

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
            var teamColor = Equals(player.Parent, gameManager.LocalPlayer.Entity)
                ? Color.LightGreen
                : player.Get<PlayerAttributes>().Team.Color;
            spriteBatch.DrawString(mapFont, playerOnMap, origin + position * factor - dotSize / 2f, teamColor);
        }

        private static void PrepareNonCameraSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }

        public static Vector2 MinimapPosition(Viewport viewport, Vector2 minimapSize)
        {
            return new Vector2(viewport.Width, viewport.Height) - minimapSize;
        }
    }
}
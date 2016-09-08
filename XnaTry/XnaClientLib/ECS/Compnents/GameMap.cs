using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using XnaCommonLib;

namespace XnaClientLib.ECS.Compnents
{
    public class GameMap : GuiComponent
    {
        #region Fields

        public string TmxMapName { get; }
        protected TmxMapData map;
        private readonly Dictionary<int, Texture2D> tilesByCode;

        #endregion

        #region Properties

        public Rectangle Bounds => new Rectangle(0, 0, map.MapWidth, map.MapHeight);

        #endregion

        #region Constructor

        public GameMap(string mapName)
        {
            TmxMapName = mapName;
            map = null;
            tilesByCode = new Dictionary<int, Texture2D>();
        }

        #endregion Constructor

        #region GuiComponent Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(spriteBatch);
        }

        protected void DrawMap(SpriteBatch spriteBatch, double factor = 1.0, Vector2? origin = null)
        {
            origin = origin ?? Vector2.Zero;

            var width = (int) (map.TileWidth * factor);
            var height = (int)(map.TileHeight * factor);

            foreach (var tile in map.Map.Layers.SelectMany(layer => layer.Tiles.Where(t => t.Gid != 0)))
            {
                spriteBatch.Draw(tilesByCode[tile.Gid - 1],
                    new Rectangle((int)origin.Value.X + tile.X * width, (int)origin.Value.Y + tile.Y * height, width, height), Color.White);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            map = new TmxMapData(TmxMapName);

            var tiles = map.Map.Tilesets[0].Tiles;
            foreach (var t in tiles)
                tilesByCode.Add(t.Id, content.Load<Texture2D>(SourceToAsset(t.Image.Source)));
        }

        public override int DrawOrder()
        {
            return Constants.GUI.DrawOrder.Map;
        }

        #endregion GuiComponent Methods

        #region Helper Methods

        /// <summary>
        /// Gets the Asset name of the source file
        /// </summary>
        /// <param name="source">The path of the source file</param>
        /// <returns>An asset name in XnaTryContent</returns>
        static string SourceToAsset(string source)
        {
            var firstIndex = source.IndexOf("Content", StringComparison.Ordinal) + ("Conetent").Length;
            var lastIndex = source.LastIndexOf(".", StringComparison.Ordinal);
            return source.Substring(firstIndex, lastIndex - firstIndex);
        }

        #endregion
    }
}

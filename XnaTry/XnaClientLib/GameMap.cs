using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib;

namespace XnaClientLib
{
    public class GameMap : GuiComponent
    {
        #region Fields

        private readonly string tmxMapName;
        private TmxMapData map;
        public readonly Dictionary<int, Texture2D> tilesByCode;

        #endregion

        #region Properties

        public Rectangle Bounds => new Rectangle(0, 0, map.MapWidth, map.MapHeight);

        #endregion

        #region Constructor

        public GameMap(string mapName)
        {
            tmxMapName = mapName;
            map = null;
            tilesByCode = new Dictionary<int, Texture2D>();
        }

        #endregion Constructor

        #region GuiComponent Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in map.Map.Layers.SelectMany(layer => layer.Tiles.Where(t => t.Gid != 0)))
            {
                spriteBatch.Draw(tilesByCode[tile.Gid - 1],
                    new Rectangle(tile.X * map.TileWidth, tile.Y * map.TileHeight, map.TileWidth, map.TileHeight),
                    Color.White);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            map = new TmxMapData(tmxMapName);

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

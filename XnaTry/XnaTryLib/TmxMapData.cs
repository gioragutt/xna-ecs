using TiledSharp;
using UtilsLib.Consts;

namespace XnaCommonLib
{
    public class TmxMapData
    {
        public TmxMap Map { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int MapWidth { get;  }
        public int MapHeight { get; }
        public TmxMapData(string fileName)
        {
            Map = new TmxMap(fileName);
            TileWidth = (int)(Map.TileWidth * Constants.Game.TileScale);
            TileHeight = (int)(Map.TileHeight * Constants.Game.TileScale);
            MapWidth = Map.Width * TileWidth;
            MapHeight = Map.Height * TileHeight;
        }
    }
}
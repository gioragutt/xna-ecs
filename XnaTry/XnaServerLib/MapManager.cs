using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TiledSharp;
using UtilsLib.Consts;
using XnaCommonLib;

namespace XnaServerLib
{
    public class MapManager
    {
        #region Fields

        private TmxMapData map;

        #endregion

        #region Properties

        private readonly Dictionary<string, List<SpawningArea>> teamRespawnAreas;

        #endregion

        #region Constructor

        public MapManager(string mapName)
        {
            map = new TmxMapData(mapName);
            teamRespawnAreas = new Dictionary<string, List<SpawningArea>>();
            var spawningAreas = map.Map.ObjectGroups["SpawningAreas"].Objects;
            AddTeamSpawningAreas(spawningAreas, "Good");
            AddTeamSpawningAreas(spawningAreas, "Bad");
        }

        #endregion

        #region API

        readonly Random rnd = new Random();

        public Vector2 GetRandomSpawnPosition(string teamName)
        {
            var spawnIndex = rnd.Next(teamRespawnAreas[teamName].Count);
            return teamRespawnAreas[teamName][spawnIndex].RandomPositionInArea();
        }

        #endregion

        #region Helper Methods

        private void AddTeamSpawningAreas(IEnumerable<TmxObject> spawningAreas, string teamName)
        {
            teamRespawnAreas[teamName] = spawningAreas.Where(a => a.Name.StartsWith(teamName)).Select(GetSpawningArea).ToList();
        }

        private static SpawningArea GetSpawningArea(TmxObject tmxObject)
        {
            return new SpawningArea(
                new Rectangle(
                    (int) (tmxObject.X * Constants.Game.TileScale), 
                    (int) (tmxObject.Y * Constants.Game.TileScale),
                    (int) (tmxObject.Width * Constants.Game.TileScale),
                    (int) (tmxObject.Height * Constants.Game.TileScale)));
        }

        #endregion
    }
}

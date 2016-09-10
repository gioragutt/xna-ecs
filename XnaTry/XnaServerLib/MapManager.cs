using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SharedGameData;
using TiledSharp;
using UtilsLib.Consts;
using XnaCommonLib;

namespace XnaServerLib
{
    public class MapManager
    {
        #region Fields

        private readonly Dictionary<string, IList<SpawningArea>> teamRespawnAreas;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of MapManager
        /// </summary>
        /// <param name="tmxMapName">name of the tmx map</param>
        public MapManager(string tmxMapName)
        {
            var map = new TmxMapData(tmxMapName);
            teamRespawnAreas = new Dictionary<string, IList<SpawningArea>>();
            var spawningAreas = map.Map.ObjectGroups["SpawningAreas"].Objects;
            foreach (var team in TeamsData.Teams.Keys)
                AddTeamSpawningAreas(spawningAreas, team);
        }

        #endregion

        #region API

        readonly Random rnd = new Random();

        /// <summary>
        /// Gets a random spawn point based on the team name
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public Vector2 GetRandomSpawnPosition(string teamName)
        {
            try
            {
                var spawnIndex = rnd.Next(teamRespawnAreas[teamName].Count);
                return teamRespawnAreas[teamName][spawnIndex].RandomPositionInArea();
            }
            catch (KeyNotFoundException e)
            {
                throw new ArgumentOutOfRangeException(teamName, e);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Initializes the spawning areas for given team
        /// </summary>
        /// <param name="spawningAreas">All spawning areas</param>
        /// <param name="teamName">The name of the team</param>
        /// <remarks>The spawning area's team is decided by the object's "Team" property in the tmx map</remarks>
        private void AddTeamSpawningAreas(IEnumerable<TmxObject> spawningAreas, string teamName)
        {
            teamRespawnAreas[teamName] = spawningAreas.Where(a => IsSpawningAreaForTeam(a, teamName)).Select(ParseSpawningArea).ToList();
        }

        /// <summary>
        /// Checks if the tmx object is a spawning are representing a given team
        /// </summary>
        /// <param name="obj">The checked tmx object</param>
        /// <param name="team">name of the team</param>
        /// <returns></returns>
        private static bool IsSpawningAreaForTeam(TmxObject obj, string team)
        {
            return obj.Properties["Team"] == team;
        }

        /// <summary>
        /// Parses a TmxObject to a SpawningArea
        /// </summary>
        /// <param name="tmxObject">TmxObject representing a spawning area</param>
        /// <returns>Spawning are represented by the TmxObject</returns>
        private static SpawningArea ParseSpawningArea(TmxObject tmxObject)
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

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XnaCommonLib;

namespace SharedGameData
{
    public static class TeamsData
    {
        public static readonly TeamData GoodTeam = new TeamData
        {
            Color = Color.Blue,
            Name = "Good",
            Frame = "Player/GUI/GreenTeamV2"
        };

        public static readonly TeamData BadTeam = new TeamData
        {
            Color = Color.Red,
            Name = "Bad",
            Frame = "Player/GUI/RedTeamV2"
        };

        public static Dictionary<string, TeamData> Teams { get; } = new Dictionary<string, TeamData>
        {
            [GoodTeam.Name] = GoodTeam,
            [BadTeam.Name] = BadTeam
        };
    }
}

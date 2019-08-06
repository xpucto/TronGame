using System.Collections.Generic;
using Tron.Model.Models.Enums;

namespace Tron.Model.Models
{
    public class Game
    {
        public Game()
        {
            Players = new HashSet<Player>();
        }

        public string Name { get; set; }

        public HashSet<Player> Players { get; set; }

        public GameStatus Status { get; set; }

        public int NumberOfPlayers { get; set; }

        public int JoinedPlayersCount { get; set; }

        public int FieldWidth { get; set; }

        public int FieldHeight { get; set; }

        public int Speed { get; set; }
    }
}

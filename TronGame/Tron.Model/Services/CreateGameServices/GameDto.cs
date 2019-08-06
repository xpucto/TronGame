using System;
using System.Collections.Generic;
using System.Text;
using Tron.Model.Models.Enums;

namespace Tron.Model.Services.CreateGameServices
{
    public class GameDto
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public int NumberOfPlayers { get; set; }

        public int JoinedPlayersCount { get; set; }
    }
}

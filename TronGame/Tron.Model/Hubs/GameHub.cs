using Microsoft.AspNetCore.SignalR;
using System.Linq;
using Tron.Model.Models.Enums;
using Tron.Model.Services.CreateGameServices;

namespace Tron.Model.Hubs
{
    /// <summary>
    /// The game hub which serves the communication between server and client
    /// </summary>
    public class GameHub : Hub
    {
        private ICreateGameService _gameService;
        public GameHub(ICreateGameService gameService)
            :base()
        {
            _gameService = gameService;
        }

        public void AddUser(string user, string game)
        {
            _gameService.SetPlayerConnectionId(user, game, Context.ConnectionId);
        }

        public void MovePlayer(string gameName, int keyCode)
        {
            var game = _gameService.GetGame(gameName);
            game.Players.First(x => x.ConnectionId == Context.ConnectionId).ChangeDirection((Direction)keyCode);
        }
    }
}

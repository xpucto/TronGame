using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tron.Model.Models;
using Tron.Model.Models.Enums;
using Tron.Model.Services.CreateGameServices;
using Tron.Model.Services.GameMangerServices;

namespace Tron.Model.CreateGameServices.Services
{
    /// <summary>
    /// Represents Create game manager
    /// </summary>
    public class CreateGameService : ICreateGameService
    {
        private IDictionary<string, Game> _games;

        private readonly IServiceProvider _services;

        public CreateGameService(IServiceProvider services)
        {
            _services = services;
            _games = new Dictionary<string, Game>();
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="game" cref="GameDto">Create game data</param>
        /// <returns cref="bool">Returns true if succeeds, otherwise false.</returns>
        public bool CreateGame(GameDto game)
        {
            Game alreadyCreatedGame = null;
            if (_games.TryGetValue(game.Name, out alreadyCreatedGame))
            {
                return false;
            }

            _games.Add(game.Name, new Game
            {
                Name = game.Name,
                NumberOfPlayers = game.NumberOfPlayers,
                Status = GameStatus.NotStarted
            });

            return true;
        }

        /// <summary>
        /// Gets not started games
        /// </summary>
        /// <returns>Returns list of not started games</returns>
        public IList<GameDto> GetAvailableGames()
        {
            ClearFinishedGames();
            return _games.Where(x => x.Value.Status == GameStatus.NotStarted).Select(x => new GameDto
            {
                Name = x.Value.Name,
                NumberOfPlayers = x.Value.NumberOfPlayers,
                JoinedPlayersCount = x.Value.JoinedPlayersCount,
                Status = x.Value.Status.ToString(),
            }).ToList();
        }

        /// <summary>
        /// Joins player to a selected game.
        /// </summary>
        /// <param name="playerName" cref="string">Player name</param>
        /// <param name="gameName" cref="string">Game name</param>
        /// <returns>Returns true if succeeds, otherwise false</returns>
        public bool JoinGame(string playerName, string gameName)
        {
            Game game = null;
            if (!_games.TryGetValue(gameName, out game))
            {
                return false;
            }

            if (!game.Players.Add(new Player(playerName)))
            {
                return false;
            }

            game.JoinedPlayersCount++;

            return true;
        }

        /// <summary>
        /// Sets the player connectionId
        /// </summary>
        /// <param name="playerName">Player name</param>
        /// <param name="gameName">Game name</param>
        /// <param name="connectionId">Connection Id</param>
        public void SetPlayerConnectionId(string playerName, string gameName, string connectionId)
        {
            var game = _games[gameName];
            var player = game.Players.First(x => x.Name == playerName);
            player.ConnectionId = connectionId;
            if (game.NumberOfPlayers == game.JoinedPlayersCount && 
                !game.Players.Any(x => string.IsNullOrEmpty(x.ConnectionId)))
            {
                game.Speed = 1000;
                game.FieldHeight = 40;
                game.FieldWidth = 60;
                StartGame(game);
                game.Status = GameStatus.InProgress;
            }
        }

        /// <summary>
        /// Returns a game by its name
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns cref="Game">Game object</returns>
        public Game GetGame(string gameName)
        {
            return _games[gameName];
        }

        private void StartGame(Game game)
        {
            var gameManager = (IGameMangerService)_services.GetService(typeof(IGameMangerService));
            ThreadPool.QueueUserWorkItem(new WaitCallback(gameManager.Run), game);
        }

        private void ClearFinishedGames()
        {
            var finishedGamesNames = _games.Where(x => x.Value.Status == GameStatus.Finished).Select(x => x.Key).ToList();
            foreach (var name in finishedGamesNames)
            {
                _games.Remove(name);
            }
        }
    }
}

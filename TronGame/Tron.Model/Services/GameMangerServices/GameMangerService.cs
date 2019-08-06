using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tron.Model.Hubs;
using Tron.Model.Models;
using Tron.Model.Models.Enums;

namespace Tron.Model.Services.GameMangerServices
{
    /// <summary>
    /// The class manages a running game
    /// </summary>
    public class GameMangerService : IGameMangerService
    {
        private readonly IHubContext<GameHub> _hub;
        private bool[,] _gameField;
        private readonly IList<Player> _playersToKill;
        private bool _run;
        private Game _game;

        public GameMangerService(IHubContext<GameHub> hub)
        {
            _playersToKill = new List<Player>();
            _hub = hub;
        }

        /// <summary>
        /// Starts a game
        /// </summary>
        /// <param name="game">Game object</param>
        public void Run(object game)
        {
            _game = (Game)game;
            Intialize();
            _run = true;
            while (_run)
            {
                foreach (var player in _game.Players)
                {
                    if (player.IsAlive)
                    {
                        GeneratePlayer(player);
                    }
                }

                MovePlayersMessage().Wait();
                if (_playersToKill.Any())
                {
                    foreach (var player in _playersToKill)
                    {
                        KillPlayer(player);
                        KillPlayerMessage(player).Wait();
                    }

                    _playersToKill.Clear();
                }

                Thread.Sleep(_game.Speed);
            }

            _game.Status = GameStatus.Finished;
        }

        private void InitField()
        {
            _gameField = new bool[_game.FieldHeight, _game.FieldWidth];
            for (int i = 0; i < _game.FieldHeight; i++)
            {
                for (int j = 0; j < _game.FieldWidth; j++)
                {
                    if (i == 0 || j == 0 || i == _game.FieldHeight - 1 || j == _game.FieldWidth - 1)
                    {
                        _gameField[i, j] = false;
                    }
                    else
                    {
                        _gameField[i, j] = true;
                    }
                }
            }
        }

        private void Intialize()
        {
            this.InitField();
            this.InitPlayers();
        }

        private void InitPlayers()
        {
            var halfFieldWidth = _game.FieldWidth / 2;
            var halfFieldHeight = _game.FieldHeight / 2;

            int i = 0;
            foreach (var player in _game.Players)
            {
                if (i == 0)
                {
                    player.SetPosition(new PlayerPosition(1, halfFieldHeight, Direction.Right));
                }
                else if (i == 1)
                {
                    player.SetPosition(new PlayerPosition(_game.FieldWidth - 2, halfFieldHeight, Direction.Left));
                }
                else if (i == 2)
                {
                    player.SetPosition(new PlayerPosition(halfFieldWidth, 1, Direction.Down));
                }
                else if (i == 3)
                {
                    player.SetPosition(new PlayerPosition(halfFieldWidth, _game.FieldHeight - 2, Direction.Up));
                }

                i++;
            }
        }

        private void GeneratePlayer(Player player)
        {
            player.Move();
            if (!_gameField[player.Position.PositionY, player.Position.PositionX])
            {
                _playersToKill.Add(player);
                return;
            }

            _gameField[player.Position.PositionY, player.Position.PositionX] = false;
            player.Score++;
        }

        private async Task MovePlayersMessage()
        {
            await _hub.Clients.Clients(_game.Players.Select(x => x.ConnectionId).ToList())
                .SendAsync("movePayers", _game.Players.Where(x => x.IsAlive).Select(x => x.PlayerPath).ToList());
        }

        private async Task KillPlayerMessage(Player player)
        {
            await _hub.Clients.Clients(_game.Players.Select(x => x.ConnectionId).ToList())
                .SendAsync("killPayer", player.Position);
        }

        private void KillPlayer(Player player)
        {
            player.IsAlive = false;

            // clear the field
            for (int i = 0; i < player.PlayerPath.Count; i++)
            {
                _gameField[player.PlayerPath[i].PositionY, player.PlayerPath[i].PositionX] = true;
            }

            // check is there more alive players
            if (!_game.Players.Any(x => x.IsAlive))
            {
                _run = false;
            }
        }
    }
}

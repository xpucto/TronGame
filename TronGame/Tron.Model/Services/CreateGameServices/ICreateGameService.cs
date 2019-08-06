using System.Collections.Generic;
using Tron.Model.Models;

namespace Tron.Model.Services.CreateGameServices
{
    /// <summary>
    /// Represents Create game manager
    /// </summary>
    public interface ICreateGameService
    {
        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="game" cref="GameDto">Create game data</param>
        /// <returns cref="bool">Returns true if succeeds, otherwise false.</returns>
        bool CreateGame(GameDto game);

        /// <summary>
        /// Gets not started games
        /// </summary>
        /// <returns>Returns list of not started games</returns>
        IList<GameDto> GetAvailableGames();

        /// <summary>
        /// Joins player to a selected game.
        /// </summary>
        /// <param name="playerName" cref="string">Player name</param>
        /// <param name="gameName" cref="string">Game name</param>
        /// <returns>Returns true if succeeds, otherwise false</returns>
        bool JoinGame(string playerName, string gameName);

        /// <summary>
        /// Sets the player connectionId
        /// </summary>
        /// <param name="playerName">Player name</param>
        /// <param name="gameName">Game name</param>
        /// <param name="connectionId">Connection Id</param>
        void SetPlayerConnectionId(string playerName, string gameName, string connectionId);

        /// <summary>
        /// Returns a game by its name
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns cref="Game">Game object</returns>
        Game GetGame(string gameName);
    }
}

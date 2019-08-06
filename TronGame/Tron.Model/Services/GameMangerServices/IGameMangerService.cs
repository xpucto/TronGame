using System.Threading.Tasks;

namespace Tron.Model.Services.GameMangerServices
{
    /// <summary>
    /// The class manages a running game
    /// </summary>
    public interface IGameMangerService
    {
        /// <summary>
        /// Starts a game
        /// </summary>
        /// <param name="game">Game object</param>
        void Run(object callBack);
    }
}

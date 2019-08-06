using Microsoft.AspNetCore.Mvc;
using Tron.Model.Services.CreateGameServices;

namespace Tron.Web.Controllers
{
    /// <summary>
    /// This controller manages game creation and game join functionality.
    /// </summary>
    public class HomeController : Controller
    {
        private const string GameNameError = "Game name is already taken. Please change the name";

        private readonly ICreateGameService _createGameService;
        public HomeController(ICreateGameService createGameService)
        {
            _createGameService = createGameService;
        }

        /// <summary>
        /// Loads the home screen
        /// </summary>
        /// <returns>Home screen view</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets not started games
        /// </summary>
        /// <returns>Returns list of not started games</returns>
        [HttpGet]
        public JsonResult GetAvailableGames()
        {
            return Json(_createGameService.GetAvailableGames());
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="game" cref="GameDto">Create game data</param>
        /// <returns code="201">if operation is successful</returns>
        /// <returns code="400">if operation is fails</returns>
        [HttpPost]
        public ActionResult CreateGame(GameDto game)
        {
            if (_createGameService.CreateGame(game))
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created);
            };

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
                new { Error = GameNameError });
        }

        /// <summary>
        /// Joins player to a selected game.
        /// </summary>
        /// <param name="playerName" cref="string">Player name</param>
        /// <param name="gameName" cref="string">Game name</param>
        /// <returns>if succeeds returns the Game view.</returns>
        /// <returns>if fails reloads home view.</returns>
        [HttpGet]
        public ActionResult JoinGame(string playerName, string gameName)
        {
            if (_createGameService.JoinGame(playerName, gameName))
            {
                ViewBag.gameName = gameName;
                ViewBag.playerName = playerName;

                return View("Game");
            };

            return View("Index");
        }
    }
}

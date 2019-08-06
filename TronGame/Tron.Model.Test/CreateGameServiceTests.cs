using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tron.Model.CreateGameServices.Services;
using Tron.Model.Services.CreateGameServices;

namespace Tron.Model.Test
{
    [TestClass]
    public class CreateGameServiceTests
    {
        [TestMethod]
        public void CreateGame_Game_Created()
        {
            // Arrange
            var gameDto = new GameDto { Name = "TestGame", NumberOfPlayers = 2 };
            var gameService = new CreateGameService(null);

            // Act
            var result = gameService.CreateGame(gameDto);
            var createdGame = gameService.GetGame(gameDto.Name);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(gameDto.Name, createdGame.Name);
            Assert.AreEqual(gameDto.NumberOfPlayers, createdGame.NumberOfPlayers);
        }

        [TestMethod]
        public void CreateGame_Create_Game_With_Same_Name_Fails()
        {
            // Arrange
            var gameDto = new GameDto { Name = "TestGame", NumberOfPlayers = 2 };
            var gameService = new CreateGameService(null);

            // Act

            var result = gameService.CreateGame(gameDto);
            result = gameService.CreateGame(gameDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

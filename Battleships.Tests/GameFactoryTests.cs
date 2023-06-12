using System.Collections;
using System.Security.Cryptography;

namespace Battleships.Tests;

[TestFixture]
internal class GameFactoryTests
{
    [TestCaseSource(typeof(Cases))]
    public Task ShouldGenerateShips10x10(int seed, GameOptions options)
    {
        // Arrange
        var sut = new GameFactory(new Random(seed), options);

        // Act
        var result = sut.GenerateShips();

        // Assert
        return Verify(result);
    }

    private class Cases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[] { 1, new GameOptions(10, 10, new ShipType[] { new ShipType.Battleship(), new ShipType.Battleship(), new ShipType.Destroyer() }) };
            yield return new object[] { 2, new GameOptions(10, 10, new ShipType[] { new ShipType.Battleship(), new ShipType.Battleship(), new ShipType.Destroyer() }) };
            yield return new object[] { 3, new GameOptions(10, 10, new ShipType[] { new ShipType.Battleship(), new ShipType.Battleship(), new ShipType.Destroyer() }) };
        }
    }
}


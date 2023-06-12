using FluentAssertions;
using System.Collections.Immutable;
using static Game.ShotResult;

namespace Battleships.Tests;

[TestFixture]
internal class GameTests
{
    [Test]
    public void ShouldReturnHit()
    {
        // Arrange
        var sut = new Game(ImmutableList.Create(
            new Ship(ImmutableHashSet.Create(new Coord(1, 1), new Coord(2,1), new Coord(3,1)))
        ));

        // Act
        var result = sut.Shot(new Coord(1, 1));

        // Assert
        result.Should().Be(Hit);
    }

    [Test]
    public void ShouldReturnMiss()
    {
        // Arrange
        var sut = new Game(ImmutableList.Create(
            new Ship(ImmutableHashSet.Create(new Coord(1, 1), new Coord(2,1), new Coord(3,1)))
        ));

        // Act
        var result = sut.Shot(new Coord(1, 2));

        // Assert
        result.Should().Be(Miss);
    }

    [Test]
    public void ShouldReturnSink()
    {
        // Arrange
        var sut = new Game(ImmutableList.Create(
            new Ship(ImmutableHashSet.Create(new Coord(1, 1), new Coord(2, 1), new Coord(3, 1))),
            new Ship(ImmutableHashSet.Create(new Coord(1, 2), new Coord(1, 3))
        )));

        // Act & Assert
        sut.Shot(new Coord(1, 1)).Should().Be(Hit);
        sut.Shot(new Coord(2, 1)).Should().Be(Hit);
        sut.Shot(new Coord(3, 1)).Should().Be(Sink);
    }

    [Test]
    public void ShouldReturnDuplicatedShot()
    {
        // Arrange
        var sut = new Game(ImmutableList.Create(
            new Ship(ImmutableHashSet.Create(new Coord(1, 1), new Coord(2, 1), new Coord(3, 1))),
            new Ship(ImmutableHashSet.Create(new Coord(1, 2), new Coord(1, 3))
        )));

        // Act & Assert
        sut.Shot(new Coord(1, 1)).Should().Be(Hit);
        sut.Shot(new Coord(1, 1)).Should().Be(DuplicatedShot);
    }

    [Test]
    public void ShouldPlayUntilGameOver()
    {
        // Arrange
        var sut = new Game(ImmutableList.Create(
            new Ship(ImmutableHashSet.Create(new Coord(1, 1), new Coord(2, 1), new Coord(3, 1))),
            new Ship(ImmutableHashSet.Create(new Coord(1, 2), new Coord(1, 3))
        )));

        // Act & Assert
        sut.Shot(new Coord(1, 1)).Should().Be(Hit);
        sut.Shot(new Coord(2, 1)).Should().Be(Hit);
        sut.Shot(new Coord(3, 1)).Should().Be(Sink);

        sut.Shot(new Coord(1, 4)).Should().Be(Miss);

        sut.Shot(new Coord(1, 2)).Should().Be(Hit);
        sut.Shot(new Coord(1, 3)).Should().Be(GameOver);
    }
}


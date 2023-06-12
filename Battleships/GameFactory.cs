using System.Collections.Immutable;

internal class GameFactory
{
    private readonly Random _random;
    private readonly GameOptions options;

    /*
     * Injecting Random allows to perform repeatable tests
     */
    public GameFactory(Random random, GameOptions gameOptions)
    {
        options = gameOptions;
        _random = random;
    }

    public Game Generate() => new(GenerateShips());

    public ImmutableList<Ship> GenerateShips() => options
                   .Ships
                   .Aggregate(ImmutableList<Ship>.Empty, (ships, ship) =>
                   {
                       while (true)
                       {
                           var direction = RandNextDirection();

                           var shipStartCoord = new Coord(
                               _random.Next(options.XSize - (direction == Direction.X ? ship.Length : 0)),
                               _random.Next(options.YSize - (direction == Direction.Y ? ship.Length : 0)));

                           var allShipCoords = Enumerable
                               .Range(0, ship.Length)
                               .Select(index => direction switch
                               {
                                   Direction.X => new Coord(shipStartCoord.X + index, shipStartCoord.Y),
                                   Direction.Y => new Coord(shipStartCoord.X, shipStartCoord.Y + index),
                                   _ => throw new InvalidOperationException()
                               }).ToImmutableList();

                           if (ships.SelectMany(x => x.Coords).Intersect(allShipCoords).Any())
                               continue; // conflict with previosly created ship, handled this way to simplify solution

                           return ships.Add(new Ship(allShipCoords.ToImmutableHashSet()));
                       }
                   });


    private Direction RandNextDirection() => _random.Next(2) switch
    {
        0 => Direction.Y,
        1 => Direction.X,
        _ => throw new InvalidOperationException()
    };

    private enum Direction
    {
        X,
        Y
    }
}
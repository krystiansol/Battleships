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
                           var direction = NextDirection();

                           var shipStartField = new Coord(
                               _random.Next(0, options.XSize - (direction == Direction.X ? ship.Length : 0)),
                               _random.Next(0, options.YSize - (direction == Direction.Y ? ship.Length : 0)));

                           var allShipFields = Enumerable
                               .Range(0, ship.Length)
                               .Select(index => direction switch
                               {
                                   Direction.X => new Coord(shipStartField.X + index, shipStartField.Y),
                                   Direction.Y => new Coord(shipStartField.X, shipStartField.Y + index),
                               }).ToImmutableList();

                           if (ships.SelectMany(x => x.Fields).Intersect(allShipFields).Any())
                               continue; // conflict with previosly created ship, handled this way to simplify solution

                           return ships.Add(new Ship(allShipFields.ToImmutableHashSet()));
                       }
                   });


    private Direction NextDirection() => _random.Next(0, 2) switch
    {
        0 => Direction.Y,
        1 => Direction.X
    };

    private enum Direction
    {
        X,
        Y
    }
}
using System.Collections.Immutable;
using System.Text;
using static Game.ShotResult;

internal record GameOptions(int XSize, int YSize, ShipType[] Ships);

internal record ShipType(int Length)
{
    internal record Battleship() : ShipType(5);
    internal record Destroyer() : ShipType(4);
}

internal record Coord(int X, int Y);

internal record Ship(ImmutableHashSet<Coord> Coords);

internal class Game
{
    private readonly HashSet<Coord> ShotsHistory;
    private readonly ImmutableList<Ship> Ships;

    public Game(ImmutableList<Ship> ships)
    {
        Ships = ships;
        ShotsHistory = new HashSet<Coord>();
    }

    public ShotResult Shot(Coord coord)
    {
        if (ShotsHistory.Contains(coord))
            return DuplicatedShot;

        ShotsHistory.Add(coord);

        if (!Ships.Any(x => x.Coords.Contains(coord)))
            return Miss;

        if (!Ships.SelectMany(ship => ship.Coords).Except(ShotsHistory).Any())
            return GameOver;

        if (Ships.First(ship => ship.Coords.Contains(coord)).Coords.Except(ShotsHistory).Any())
            return Hit;

        return Sink;
    }

    public enum ShotResult
    {
        Miss,
        Hit,
        Sink,
        GameOver,
        DuplicatedShot,
    }

    public string ToString(bool showShips)
    {
        StringBuilder builder = new();

        foreach (var y in Enumerable.Range(0, 10))
        {
            foreach (var x in Enumerable.Range(0, 10))
            {
                var coord = new Coord(x, y);
                if (showShips)
                {
                    builder.Append(Ships.SelectMany(ship => ship.Coords).Contains(coord) switch
                    {
                        true => "S",
                        false => "_"
                    });
                }
                else if (ShotsHistory.Contains(coord) && Ships.SelectMany(ship => ship.Coords).Contains(coord))
                    builder.Append("H"); // Hit
                else if (ShotsHistory.Contains(coord))
                    builder.Append("M"); // Miss
                else
                    builder.Append("?"); // Unknown
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
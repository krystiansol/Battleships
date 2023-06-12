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

internal record Ship(ImmutableHashSet<Coord> Fields);

internal class Game
{
    private readonly HashSet<Coord> Log;
    private readonly ImmutableList<Ship> Ships;

    public Game(ImmutableList<Ship> ships)
    {
        Ships = ships;
        Log = new HashSet<Coord>();
    }

    public ShotResult Shot(Coord coord)
    {
        if (Log.Contains(coord))
            return DuplicatedShot;

        Log.Add(coord);

        if (!Ships.Any(x => x.Fields.Contains(coord)))
            return Miss;

        if (!Ships.SelectMany(field => field.Fields).Except(Log).Any())
            return GameOver;

        if (Ships.First(x => x.Fields.Contains(coord)).Fields.Except(Log).Any())
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
                    builder.Append(Ships.SelectMany(ship => ship.Fields).Contains(coord) switch
                    {
                        true => "S",
                        false => "_"
                    });
                }
                else if (Log.Contains(coord) && Ships.SelectMany(x => x.Fields).Contains(coord))
                    builder.Append("H"); // Hit
                else if (Log.Contains(coord))
                    builder.Append("M"); // Miss
                else
                    builder.Append("?"); // Unknown
            }
            builder.AppendLine();
        }

        return builder.ToString();
    }
}
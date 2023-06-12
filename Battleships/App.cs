using System.CommandLine;

internal class App
{
    private Game CurrentGame;
    private GameFactory Factory;

    public App(GameFactory factory) => Factory = factory;

    public async Task StartAsync()
    {
        CurrentGame = Factory.Generate();

        var rootCommand = new RootCommand(Logo)
        {
            Exit(),
            Shot(),
            StartNewGame(),
            Map()
        };

        await rootCommand.InvokeAsync("--help");
        do
        {
            Console.Write("Battleship>");
        } while (new[] { 0, 1 }.Contains(await rootCommand.InvokeAsync((Console.ReadLine() ?? string.Empty).Split(" "))));
    }

    private Command StartNewGame()
    {
        var startNewGame = new Command("start-new", "starts new game");
        startNewGame.SetHandler(() =>
        {
            CurrentGame = Factory.Generate();
            Console.WriteLine("New game started!");
        });
        return startNewGame;
    }

    private Command Exit()
    {
        var exit = new Command("exit", "exit");
        exit.SetHandler(() => Task.FromResult(2));
        return exit;
    }

    private Command Shot()
    {
        var shot = new Command("shot", "make a shot - \"shot A 1\"");

        var columnArg = new Argument<string>("column", "coordinate in form [A-J]");
        columnArg.AddValidator(result =>
        {
            var value = result.GetValueForArgument(columnArg);
            if (value.Length > 1 || (int)value[0] is < 'A' or > ('A' + 10))
                result.ErrorMessage = "Column must be in form [A-J]";
        });
        shot.AddArgument(columnArg);

        var rowArg = new Argument<int>("row", "coordinate in form 0-9{1,2}");
        rowArg.AddValidator(result =>
        {
            if (result.GetValueForArgument(rowArg) is < 1 or > 10)
                result.ErrorMessage = "Row must be between 1-10";
        }
        );
        shot.AddArgument(rowArg);

        shot.SetHandler((column, row) =>
        {
            var result = CurrentGame.Shot(new Coord(column[0] - 'A', row - 1));
            Console.WriteLine(result);
            Console.WriteLine(CurrentGame.ToString(false));
            if (result is Game.ShotResult.GameOver)
            {
                CurrentGame = Factory.Generate();
                Console.WriteLine("new game started!");
            }
        }, columnArg, rowArg);
        return shot;
    }

    private Command Map()
    {
        var map = new Command("map", "display map - \"map\", \"map show-ships\"");
        var showShipsArg = new Option<bool>("show-ships");
        map.AddOption(showShipsArg);
        map.SetHandler((showShips) => Console.WriteLine(CurrentGame.ToString(showShips)), showShipsArg);
        return map;
    }

    
    public static readonly String Logo =
    @"(  ___ \ (  ___  )\__   __/\__   __/( \      (  ____ \(  ____ \|\     /|\__   __/(  ____ )(  ____ \
| (   ) )| (   ) |   ) (      ) (   | (      | (    \/| (    \/| )   ( |   ) (   | (    )|| (    \/
| (__/ / | (___) |   | |      | |   | |      | (__    | (_____ | (___) |   | |   | (____)|| (_____ 
|  __ (  |  ___  |   | |      | |   | |      |  __)   (_____  )|  ___  |   | |   |  _____)(_____  )
| (  \ \ | (   ) |   | |      | |   | |      | (            ) || (   ) |   | |   | (            ) |
| )___) )| )   ( |   | |      | |   | (____/\| (____/\/\____) || )   ( |___) (___| )      /\____) |
|/ \___/ |/     \|   )_(      )_(   (_______/(_______/\_______)|/     \|\_______/|/       \_______)";
}

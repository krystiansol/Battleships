using static System.Console;

internal class UI
{
    internal record Coord(int x, int y);

    public UI()
    {
        _logo = new Logo(new Coord(0, 0));
        _prompt = new Prompt(new Coord(0, Logo.YSize + 2));
        _log = new Log(new Coord(0, Logo.YSize + 3));
        _map = new Map(new Coord(0, Logo.YSize + 5));
    }

    private readonly Logo _logo;
    private readonly Prompt _prompt;
    private readonly Log _log;
    private readonly Map _map;

    public void Start()
    {
        _logo.Print();
        _prompt.Print();
        _log.Print();
        _map.Print();

        while (true)
        {
            _prompt.Clear();
            _prompt.SetCursor();

            var line = ReadLine();
            if (line == "a1")
                _log.Print("test");
            else
                _log.Print(line);
        }
    }

    internal class Logo 
    {
        public static int XSize = 100;
        public static int YSize = 7;

        private readonly Coord _coord;

        public Logo(Coord coord) => _coord = coord;


        public void Print()
        {
            SetCursorPosition(_coord.x, _coord.y);
            Write(
    @"(  ___ \ (  ___  )\__   __/\__   __/( \      (  ____ \(  ____ \|\     /|\__   __/(  ____ )(  ____ \
| (   ) )| (   ) |   ) (      ) (   | (      | (    \/| (    \/| )   ( |   ) (   | (    )|| (    \/
| (__/ / | (___) |   | |      | |   | |      | (__    | (_____ | (___) |   | |   | (____)|| (_____ 
|  __ (  |  ___  |   | |      | |   | |      |  __)   (_____  )|  ___  |   | |   |  _____)(_____  )
| (  \ \ | (   ) |   | |      | |   | |      | (            ) || (   ) |   | |   | (            ) |
| )___) )| )   ( |   | |      | |   | (____/\| (____/\/\____) || )   ( |___) (___| )      /\____) |
|/ \___/ |/     \|   )_(      )_(   (_______/(_______/\_______)|/     \|\_______/|/       \_______)");
        }
    }

    internal class Map
    {
        public static int XSize = 10;
        public static int YSize = 10;

        private readonly Coord _coord;

        public Map(Coord coord) => _coord = coord;

        internal void Print()
        {
            foreach (var y in Enumerable.Range(0, 10))
            {
                foreach (var x in Enumerable.Range(0, 10))
                {
                    SetCursorPosition(_coord.x + x, _coord.y + y);
                    Write("x");
                }
            }
        }
    }

    internal class Prompt
    {
        private readonly Coord _coord;
        private static readonly int XSize = 15;

        public Prompt(Coord coord) => _coord = coord;

        public void Print()
        {
            SetCursorPosition(_coord.x, _coord.y);
            WriteLine("Input command:");
        }

        public void Clear()
        {
            SetCursorPosition(_coord.x+XSize, _coord.y);
            Write(new string(' ', WindowWidth - XSize));
        }

        public void SetCursor()
        {
            SetCursorPosition(_coord.x + XSize, _coord.y);
        }
    }
    
    internal class Log
    {
        private readonly Coord _coord;
        private static readonly int XSize = 5;

        public Log(Coord coord) => _coord = coord;

        public void Clear()
        {
            SetCursorPosition(_coord.x+XSize, _coord.y);
            Write(new string(' ', WindowWidth - XSize));
        }

        public void Print(string message = "")
        {
            SetCursorPosition(_coord.x, _coord.y);
            WriteLine($"Log: {message}");
        }
    }
}

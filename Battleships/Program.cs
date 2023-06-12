var options = new GameOptions(
    XSize: 10, 
    YSize: 10, 
    Ships: new ShipType[] { 
        new ShipType.Battleship(), 
        new ShipType.Destroyer(), 
        new ShipType.Destroyer() });

var app = new App(new GameFactory(new Random(), options));

await app.StartAsync();
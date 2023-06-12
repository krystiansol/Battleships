# Battleships
## Requirements
* .NET 6.0

## How to run project (dotnet CLI)
```powershell
dotnet run --project .\Battleships\Battleships.csproj
```
## Navigation
```
Commands:
  exit                 exit
  shot <column> <row>  make a shot - "shot A 1"
  start-new            starts new game
  map                  display map - "map", "map show-ships"
  --help
```

## How to run tests (dotnet CLI)
```powershell
dotnet test
```

## Debt
* hardcoded map size in CLI

## Dependencies
* System.CommandLine
* Verify.Nunit
* Nunit
* FluentAssertions
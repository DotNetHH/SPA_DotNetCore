# .net Meetup Demo

Hier der Code zur letzten Demo aus dem Vortrag mit einer kurzen Anleitung, wie man das ganze zum Laufen bekommt.

Weitere Fragen und Anregungen gerne an kuechler@ergonweb.de

## Voraussetzungen
- .net Core >= 2.x
- node >= 6.x (besser 8.x)
- npm >= 5.x (wird mit node installiert, Update: `npm i -g npm`)
- SQL Server Express >= 2014 (oder MS LocalDB, dann ConnectionString in ~/WebAPI/appsettings.json anpassen)
- VS Code (geht auch ohne, dafür ist der launch-Task aber bereits durch VS Code vorbereitet)
- im Order ~/WebClient `npm install` ausführen

VS Code sollte beim Öffnen des Ordners automatisch darauf hinweisen, dass "There are unresolved dependencies..." für WebAPI und WebAPI.Tests und eine Restore-Option anbieten. Sollte dies nicht der Fall sein, dann jeweils in den Ordnern ~/WebAPI und ~/WebAPI.Tests `dotnet restore` ausführen.

Die Datenbank wird beim Start automatisch angelegt. Siehe Startup.cs Zeile 76.

## VS Code Extensions
- C# - nötig für dieses Projekt
- Vetur - sehr hilfreich bei der Bearbeitung von .vue-Dateien

## Run
- F5

## Test
- Backend Unit-Tests in ~/WebAPI.Tests per `dotnet test` oder über die VS Code Oberfläche
- Frontend Unit-Tests in ~/WebClient per `npm test`

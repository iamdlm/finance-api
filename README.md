## Finance Api
This is a solution example for the "Backend challenge" with ASP.NET Core following the principles of Clean Architecture.

## Technologies
- ASP.NET Core 3.1
- Entity Framework Core 3.1
- xUnit & InMemory
- Swagger

## Getting started
1. Update the `DefaultConnection` connection string within `appsettings.json` and make sure it points to a valid SQL Server instance.  
When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.  
Alternatively, run the command `dotnet ef database update --project src\Fin.Infrastructure --startup-project src\Fin.Api` (assuming you're at source folder) to create the database.
2. Update information about token issuer, audience and secret within `appsettings.json`.
3. Navigate to src/Fin.Api and run `dotnet run` to launch the backend (ASP.NET Core Web API).

## Considerations about the implementation
- An user has one or more portfolios, each portfolio has a list of trades, and the sum of all trades gives the portfolio position.
- Portfolios with at least one trade cannot be deleted.
- All trades in a portfolio must have the same currency.
- Trades cannot be deleted [#13](https://github.com/iamdlm/finance-api/pull/13).


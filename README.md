## Finance Api
This is a solution example for the "Backend challenge" with ASP.NET Core following the principles of Clean Architecture.

## Technologies
- ASP.NET Core 3.1
- Entity Framework Core 3.1
- NUnit, FluentAssertions & Moq
- Swagger

## Getting started
1. Update the `DefaultConnection` connection string within `appsettings.json` and make sure it points to a valid SQL Server instance.  
When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.
2. Update information about token issuer, audience and secret within `appsettings.json`.
3. Navigate to src/Fin.Api and run `dotnet run` to launch the backend (ASP.NET Core Web API).

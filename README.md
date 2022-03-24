# Web API Project:<p/>Human Resource Management for IT Company

This project building on **.NET 6**, using for Human Resource Management for IT Company.
## Status: unfinished

## Technologies

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-6.0?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [AutoMapper](https://automapper.org/)
* [Swagger UI - Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Cronos](https://github.com/HangfireIO/Cronos)
* [Serilog](https://serilog.net/)
* [x Unit Test](https://xunit.net/)
* [Quasar](https://quasar.dev/)

## Features
* Human resources management with following information: personal, work history, project, certificate, education, skill,...
* Support: authentication-authorization with token-base (JWT).
* Archiving accounts into database, hashing password-salt with PBKDF2 algorithm.
* Storing and resizing image.
* Getting data with pagination.
* Logging information to console and file.

## Diagram

[Update diagram here](https://drawsql.app/kim-young-ken/diagrams/hr-management)

![](/Docs/diagram.png)

## Design Patterns

* Unit Of Work
* Generic Repository
* Request-Reply
* Inversion of Control / Dependency injection

## Getting Started

* Following CLI:

1. Install the latest [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)
2. Editing connection string in **appsettings.json**
3. Run `dotnet ef migrations add "Initial"` to add migration.
4. Run `dotnet ef database update` to update database.
5. Run `dotnet run` to runs source code without any explicit compile or launch commands.

**Note:** you must directive to root project before run CLI.

* Seeding data: 

1. First login: [http://yourhost/api/v1/token/login]()
 + Username: **admin**
 + Password: **c93ccd78b2076528346216b3b2f701e6** (plain-text: admin1234) (hash function: MD5)
2. Get your access token

## License

This project is licensed with the [MIT license](LICENSE).
# Web API Project:<p/>Human Resource Management for IT Company

This project building on **.NET 5.0**, using for Human Resource Management for IT Company. 

## Technologies

* [ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-5.0?view=aspnetcore-5.0)
* [Entity Framework Core 5](https://docs.microsoft.com/en-us/ef/core/)
* [AutoMapper](https://automapper.org/)
* [Swagger UI - Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Features
* Human resources management with following information: personal, work history, project, certificate, education, skill,...
* Support: authentication-authorization with token-base (JWT).
* Archiving accounts into database, hashing password-salt with PBKDF2 algorithm.
* Storing and resizing image: mobile-web format.
* Getting data with pagination.

## Diagram

[Update diagram here](https://drawsql.app/kim-young-ken/diagrams/hr-management)

![](/Docs/diagram.png)

## Design Patterns

* Unit Of Work
* Generic Repository
* Request-Reply
* Inversion of Control / Dependency injection
* ORM

## Getting Started

* Following CLI:

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Editing connection string in **appsettings.json**
3. Run `dotnet ef migrations add "Initial"` to add migration.
4. Run `dotnet ef database update` to update database.

**Note:** you must directive to root project before run CLI.

* Seeding data: 

1. First login: [http://yourhost/api/token/login]()
 + Username: **admin**
 + Password: **d7f32454b44d22182618d56e683f419a**
2. Get your access token

## License

This project is licensed with the [MIT license](LICENSE).
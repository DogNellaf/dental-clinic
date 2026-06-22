# Dental Clinic

> 🇬🇧 English | [🇷🇺 Русский](README.ru.md)

A web application for managing appointments, staff, services, and patient reviews at a dental clinic. Built with ASP.NET Core 8 MVC and Entity Framework Core.

## Features

- Public pages: home with reviews, services catalogue, schedule browser, FAQ, about, contacts
- Appointment booking with service and doctor selection
- Personal cabinet with upcoming and past appointments
- Role-based access: Client, Doctor, Manager, Administrator
- Doctor workflow: active appointment management, recommendations, patient records
- Manager moderation queue for patient reviews
- Admin panel: full control over profiles, appointments, and reviews
- Account banning and unbanning

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | C#, ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| Database | SQL Server / SQL Server Express |
| Auth | ASP.NET Core Identity (cookie-based) |
| Frontend | Bootstrap 5, jQuery, jQuery Validation |
| Tests | xUnit, Moq, EF Core In-Memory |

## Requirements

- .NET 8 SDK
- SQL Server or SQL Server Express

## Installation

```bash
# Clone the repository
git clone <repository-url>
cd dental-clinic

# Apply migrations (EF Core creates the database on first run)
dotnet run
```

After the first run, seed the roles by executing the following SQL against your database:

```sql
INSERT INTO [Role] (Id, Name, NormalizedName, ConcurrencyStamp, Title)
VALUES
  (1, N'Клиент',        N'КЛИЕНТ',        NEWID(), N'Клиент'),
  (2, N'Администратор', N'АДМИНИСТРАТОР',  NEWID(), N'Администратор'),
  (3, N'Менеджер',      N'МЕНЕДЖЕР',       NEWID(), N'Менеджер'),
  (4, N'Доктор',        N'ДОКТОР',         NEWID(), N'Доктор');
```

The application will be available at `http://localhost:5137`.

## Configuration

Edit `appsettings.json` to set your database connection:

| Key | Description | Default |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | `Server=.\SQLEXPRESS;Database=dental_clinic;...` |

## Running Tests

```bash
cd DentalClinic.Tests
dotnet test
```

Tests use an EF Core in-memory database — no SQL Server required.

## Project Structure

```
dental-clinic/
├── Controllers/          # MVC controllers (one per role + shared base)
│   └── BaseController.cs # Shared GetProfile() helper
├── Models/               # EF Core entities and DTOs
│   └── DTO/              # View models for forms
├── Views/                # Razor templates grouped by controller
├── wwwroot/              # Static assets (CSS, JS, Bootstrap)
├── DatabaseContext.cs    # EF Core DbContext
└── Program.cs            # App bootstrap and DI configuration

DentalClinic.Tests/
├── Controllers/          # Controller unit tests
└── Helpers/              # Test database factory and seeding helpers
```

## License

[MIT](LICENSE)

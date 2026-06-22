# DentalClinic — Dental Clinic Management System

> [Русская версия](README.ru.md)

A web application for managing appointments, staff, services, and patient reviews at a dental clinic network. Built with ASP.NET Core 8 MVC and Entity Framework Core.

---

## Features

| Role | Capabilities |
|------|-------------|
| **Client** | Book appointments, view medical record, write and hide reviews |
| **Doctor** | View current and scheduled appointments, add recommendations, extend or end appointments early, view patient records |
| **Manager** | Moderate and publish patient reviews |
| **Administrator** | Full management of profiles, appointments, and reviews |

**Public pages** (no login required): home page with reviews, services catalogue, schedule browser, FAQ, about, contacts.

---

## Tech Stack

- **Runtime**: .NET 8.0
- **Framework**: ASP.NET Core MVC
- **ORM**: Entity Framework Core 8 with SQL Server
- **Auth**: ASP.NET Core Identity (cookie-based)
- **Frontend**: Bootstrap 5, jQuery, jQuery Validation
- **Tests**: xUnit, Moq, EF Core In-Memory

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server Express

---

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd Программа
```

### 2. Configure the database connection

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=dental_clinic;Trusted_Connection=True;Encrypt=False;"
  }
}
```

### 3. Seed roles

The application expects four roles in the database with the following IDs and names:

| ID | Name |
|----|------|
| 1 | Клиент |
| 2 | Администратор |
| 3 | Менеджер |
| 4 | Доктор |

Run the following SQL after the app creates the database on first launch:

```sql
INSERT INTO [Role] (Id, Name, NormalizedName, ConcurrencyStamp, Title)
VALUES
  (1, N'Клиент',        N'КЛИЕНТ',        NEWID(), N'Клиент'),
  (2, N'Администратор', N'АДМИНИСТРАТОР',  NEWID(), N'Администратор'),
  (3, N'Менеджер',      N'МЕНЕДЖЕР',       NEWID(), N'Менеджер'),
  (4, N'Доктор',        N'ДОКТОР',         NEWID(), N'Доктор');
```

### 4. Run the application

```bash
dotnet run
```

The app will be available at `http://localhost:5137` (or `https://localhost:7xxx` depending on your launch profile).

---

## Running Tests

```bash
cd ..\DentalClinic.Tests
dotnet test
```

Tests use an in-memory EF Core database — no SQL Server required.

---

## Project Structure

```
Программа/
├── Controllers/          # MVC controllers (one per role + shared base)
│   └── BaseController.cs # Shared GetProfile() helper
├── Models/               # EF entities and DTOs
│   └── DTO/              # View models for forms
├── Views/                # Razor templates grouped by controller
├── wwwroot/              # Static assets (CSS, JS, Bootstrap)
├── DatabaseContext.cs    # EF Core DbContext
└── Program.cs            # App bootstrap and DI configuration

DentalClinic.Tests/
├── Controllers/          # Controller unit tests
└── Helpers/              # Test database factory and seeding helpers
```

---

## User Roles

### Client (`/client`)
- View upcoming and past appointments
- Access appointment details and doctor recommendations
- Submit or update a review (requires at least one past appointment)

### Doctor (`/doctor`)
- See the currently active appointment in real time
- Add recommendations to a patient's record
- Extend appointment duration or end it early
- Browse the full medical record of any patient

### Manager (`/manager`)
- Review the queue of pending patient reviews
- Publish or hide reviews

### Administrator (`/admin`)
- Create and manage user profiles with role assignment
- Ban or unban accounts
- Edit or delete any appointment
- Moderate reviews

---

## Appointment Booking Flow

1. Open **Schedule** and select a service.
2. Choose a doctor from the list of staff who provide that service.
3. Pick an available time slot.
4. The slot is immediately assigned to your account.
5. View your booked appointments in **Personal Cabinet**.

---

## License

MIT License

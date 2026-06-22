# Dental Clinic

> [🇬🇧 English](README.md) | 🇷🇺 Русский

Веб-приложение для управления записями на приёмы, персоналом, услугами и отзывами пациентов стоматологической клиники. Построено на ASP.NET Core 8 MVC и Entity Framework Core.

## Возможности

- Публичные страницы: главная с отзывами, каталог услуг, расписание, FAQ, о клинике, контакты
- Запись на приём с выбором услуги и врача
- Личный кабинет с предстоящими и прошедшими приёмами
- Ролевой доступ: Клиент, Врач, Менеджер, Администратор
- Врач: управление текущим приёмом, рекомендации, карта пациента
- Менеджер: очередь отзывов на модерацию
- Администратор: полное управление профилями, приёмами и отзывами
- Блокировка и разблокировка аккаунтов

## Технологии

| Слой | Технология |
|---|---|
| Backend | C#, ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 |
| База данных | SQL Server / SQL Server Express |
| Аутентификация | ASP.NET Core Identity (cookie) |
| Frontend | Bootstrap 5, jQuery, jQuery Validation |
| Тесты | xUnit, Moq, EF Core In-Memory |

## Требования

- .NET 8 SDK
- SQL Server или SQL Server Express

## Установка

```bash
# Клонировать репозиторий
git clone <repository-url>
cd dental-clinic

# EF Core создаёт базу данных при первом запуске
dotnet run
```

После первого запуска добавьте роли, выполнив следующий SQL:

```sql
INSERT INTO [Role] (Id, Name, NormalizedName, ConcurrencyStamp, Title)
VALUES
  (1, N'Клиент',        N'КЛИЕНТ',        NEWID(), N'Клиент'),
  (2, N'Администратор', N'АДМИНИСТРАТОР',  NEWID(), N'Администратор'),
  (3, N'Менеджер',      N'МЕНЕДЖЕР',       NEWID(), N'Менеджер'),
  (4, N'Доктор',        N'ДОКТОР',         NEWID(), N'Доктор');
```

Приложение будет доступно по адресу `http://localhost:5137`.

## Конфигурация

Отредактируйте `appsettings.json` для настройки подключения к базе данных:

| Ключ | Описание | По умолчанию |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Строка подключения SQL Server | `Server=.\SQLEXPRESS;Database=dental_clinic;...` |

## Запуск тестов

```bash
cd DentalClinic.Tests
dotnet test
```

Тесты используют базу данных в памяти — SQL Server не требуется.

## Структура проекта

```
dental-clinic/
├── Controllers/          # MVC-контроллеры (по одному на роль + общий базовый)
│   └── BaseController.cs # Общий метод GetProfile()
├── Models/               # EF-сущности и DTO
│   └── DTO/              # View-модели для форм
├── Views/                # Razor-шаблоны, сгруппированные по контроллерам
├── wwwroot/              # Статика (CSS, JS, Bootstrap)
├── DatabaseContext.cs    # EF Core DbContext
└── Program.cs            # Конфигурация приложения и DI

DentalClinic.Tests/
├── Controllers/          # Unit-тесты контроллеров
└── Helpers/              # Фабрика тестовой БД и вспомогательные методы
```

## Лицензия

[MIT](LICENSE)

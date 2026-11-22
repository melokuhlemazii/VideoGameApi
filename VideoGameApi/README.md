# Video Game API

A modern RESTful API for managing video games, built with **ASP.NET Core 9** and **Entity Framework Core 9** using SQL Server.

## ?? Project Overview

The Video Game API provides comprehensive endpoints for managing a video game catalog. It supports creating, reading, updating, and deleting video games along with their associated metadata including developers, publishers, genres, and detailed information.

**Tech Stack:**
- **.NET 9** (Latest LTS)
- **ASP.NET Core 9** (REST API)
- **Entity Framework Core 9** (ORM)
- **SQL Server** (Database)
- **Scalar** (Interactive API Documentation)
- **C# 13** (Language)

---

## ??? Architecture & Data Model

### Core Entities

#### **VideoGame**
Main entity representing a video game.
```csharp
- Id (int) - Primary Key
- Title (string) - Game title
- Platform (string) - Platform (e.g., PS5, PC, Nintendo Switch)
- DeveloperId (int?) - Foreign Key to Developer
- PublisherId (int?) - Foreign Key to Publisher
- VideoGameDetails (1:1) - Extended details
- Genres (M:N) - Many-to-many relationship
```

#### **VideoGameDetails**
One-to-one relationship with VideoGame containing extended information.
```csharp
- Id (int) - Primary Key
- Description (string) - Game description
- ReleaseDate (DateTime) - Release date
- VideoGameId (int) - Foreign Key to VideoGame (cascade delete)
```

#### **Developer**
Video game development studio.
```csharp
- Id (int) - Primary Key
- Name (string) - Developer name (required)
```

#### **Publisher**
Video game publishing company.
```csharp
- Id (int) - Primary Key
- Name (string) - Publisher name (required)
```

#### **Genre**
Game genre/category (Action, Adventure, RPG, etc.).
```csharp
- Id (int) - Primary Key
- Name (string) - Genre name (required)
- VideoGames (M:N) - Many-to-many relationship
```

### Relationships

```
Developer (1) ???????????????? (M) VideoGame
Publisher (1) ???????????????? (M) VideoGame
VideoGameDetails (1) ? (1) VideoGame (cascade delete)
Genre (M) ???????????????? (M) VideoGame
```

---

## ?? Getting Started

### Prerequisites

- **.NET 9 SDK** or later ([download](https://dotnet.microsoft.com/download/dotnet/9.0))
- **SQL Server** (Express or higher) on `localhost\SQLExpress`
- **Visual Studio 2022** (or any .NET IDE)

### Installation

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd VideoGameApi
   ```

2. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations:**
   ```bash
   dotnet ef database update
   ```
   Or via Visual Studio Package Manager Console:
   ```powershell
   Update-Database
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:7000` (or similar).

5. **Access the interactive API documentation:**
   - Navigate to `https://localhost:7000/scalar/v1` in your browser.
   - Explore and test all endpoints with the Scalar UI.

---

## ?? API Endpoints

### Video Games

#### **Get All Video Games**
```http
GET /api/videogame
```
**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Spider-Man 2",
    "platform": "PS5",
    "developerId": 1,
    "developer": { "id": 1, "name": "Insomniac Games" },
    "publisherId": 1,
    "publisher": { "id": 1, "name": "Sony Interactive Entertainment" },
    "videoGameDetails": {
      "id": 1,
      "description": "Open-world action-adventure...",
      "releaseDate": "2023-10-20T00:00:00"
    },
    "genres": [
      { "id": 1, "name": "Action" },
      { "id": 2, "name": "Adventure" }
    ]
  }
]
```

#### **Get Video Game by ID**
```http
GET /api/videogame/{id}
```
**Response:** `200 OK` or `404 Not Found`

#### **Create a New Video Game**
```http
POST /api/videogame
Content-Type: application/json

{
  "title": "New Game",
  "platform": "PC",
  "developerId": 1,
  "publisherId": 1
}
```
**Response:** `201 Created`

#### **Update a Video Game**
```http
PUT /api/videogame/{id}
Content-Type: application/json

{
  "title": "Updated Title",
  "platform": "PS5",
  "developerId": 1,
  "publisherId": 1,
  "genres": [
    { "id": 1 },
    { "id": 2 }
  ]
}
```
**Response:** `204 No Content` or `404 Not Found`

#### **Delete a Video Game**
```http
DELETE /api/videogame/{id}
```
**Response:** `204 No Content` or `404 Not Found`

---

## ??? Database Configuration

The application connects to SQL Server via the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLExpress;Database=VideoGameDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

**Default Database:** `VideoGameDb` on `localhost\SQLExpress`

### Modifying the Connection String

1. Edit `appsettings.json`:
   ```json
   "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=true;"
   ```

2. Create/update migrations if schema changes:
   ```bash
   dotnet ef migrations add YourMigrationName
   dotnet ef database update
   ```

---

## ?? Development

### Project Structure

```
VideoGameApi/
??? Controllers/           # API endpoints
?   ??? VideoGameController.cs
??? Models/               # Entity classes
?   ??? VideoGame.cs
?   ??? VideoGameDetails.cs
?   ??? Developer.cs
?   ??? Publisher.cs
?   ??? Genre.cs
??? Data/                 # EF Core DbContext
?   ??? VideoGameDbContext.cs
??? Migrations/           # EF Core migrations
??? Program.cs            # Application startup configuration
??? appsettings.json      # Configuration
??? VideoGameApi.csproj   # Project file
```

### Key Classes

#### **VideoGameController**
Handles all HTTP requests for video games with full CRUD operations and eager loading of related entities.

#### **VideoGameDbContext**
Entity Framework Core DbContext with:
- DbSet properties for all entities
- One-to-one relationship configuration (VideoGame ? VideoGameDetails)
- Many-to-many relationship configuration (VideoGame ? Genre)
- Required property constraints
- Sample seed data

### Adding Migrations

When modifying entity models, create a new migration:

```bash
# Using CLI
dotnet ef migrations add MigrationName
dotnet ef database update

# Or using Package Manager Console (Visual Studio)
Add-Migration MigrationName
Update-Database
```

---

## ?? Sample Data

The database is pre-seeded with:

**Video Games:**
- Spider-Man 2 (PS5)
- The Legend of Zelda: Breath of the Wild (Nintendo Switch)
- Cyberpunk 2077 (PC)

**Developers:**
- Insomniac Games
- Nintendo EPD
- CD Projekt Red

**Publishers:**
- Sony Interactive Entertainment
- Nintendo
- CD Projekt

**Genres:**
- Action
- Adventure
- RPG

---

## ?? Configuration

### Logging
Configured in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### OpenAPI / Swagger
- **Scalar UI:** `https://localhost:7000/scalar/v1` (Development only)
- **OpenAPI JSON:** `https://localhost:7000/openapi/v1.json`

---

## ?? Testing

### Manual Testing via Scalar UI
1. Run the application
2. Navigate to `https://localhost:7000/scalar/v1`
3. Use the interactive interface to test endpoints

### Using cURL
```bash
# Get all games
curl -X GET "https://localhost:7000/api/videogame" -H "accept: application/json"

# Get a specific game
curl -X GET "https://localhost:7000/api/videogame/1" -H "accept: application/json"

# Create a new game
curl -X POST "https://localhost:7000/api/videogame" \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Game","platform":"PC","developerId":1,"publisherId":1}'
```

### Using Postman
1. Import the OpenAPI spec from `https://localhost:7000/openapi/v1.json`
2. Create requests for each endpoint

---

## ?? Security Considerations

- **HTTPS:** Enforced in production
- **CORS:** Currently allows all hosts (`AllowedHosts: "*"`) — configure per environment
- **Authentication:** Not currently implemented — add JWT/OAuth as needed
- **Input Validation:** Basic null checks in place — enhance with FluentValidation or DataAnnotations

---

## ??? Common Tasks

### Add a New Entity

1. Create model in `Models/`
2. Add DbSet in `VideoGameDbContext.cs`
3. Configure relationships in `OnModelCreating()`
4. Create migration: `dotnet ef migrations add AddNewEntity`
5. Update database: `dotnet ef database update`
6. Create controller in `Controllers/`

### Update Database Schema
```bash
# Create migration
dotnet ef migrations add DescriptiveName

# Apply migration
dotnet ef database update

# Revert last migration
dotnet ef migrations remove
```

### Reset Database
```bash
# Remove all migrations and database
dotnet ef database drop
dotnet ef migrations remove

# Create fresh migrations
dotnet ef migrations add Initial
dotnet ef database update
```

---

## ?? Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.OpenApi | 9.0.0 | OpenAPI/Swagger support |
| Microsoft.EntityFrameworkCore | 9.0.0 | ORM |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.0 | SQL Server provider |
| Microsoft.EntityFrameworkCore.Tools | 9.0.0 | Migration tools |
| Scalar.AspNetCore | 1.2.39 | Interactive API documentation |

---

## ?? Troubleshooting

### Database Connection Error
- Verify SQL Server is running on `localhost\SQLExpress`
- Check connection string in `appsettings.json`
- Ensure Windows Authentication is enabled

### Migration Issues
```bash
# View current migrations
dotnet ef migrations list

# Check pending migrations
dotnet ef migrations pending

# See raw SQL for a migration
dotnet ef migrations script
```

### Port Already in Use
- Change the port in `Properties/launchSettings.json`
- Or kill the process using the port

---

## ?? Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Entity Framework Core Relationships](https://docs.microsoft.com/en-us/ef/core/modeling/relationships/)
- [Scalar Documentation](https://github.com/scalar/scalar)

---

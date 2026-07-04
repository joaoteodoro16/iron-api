# 🏋️ Iron

A **SaaS for gym management** (students, trainers, plans and memberships).
Study project built with **.NET 10** and **Clean Architecture**, focused on a
**scalable, high-performance API** able to handle many requests per second.

> 🚧 **Work in progress** — the API is still evolving. Features, optimizations
> and scalability strategies are being added continuously.

---

## ✨ Goals

This is primarily a **learning project**. The main objectives are:

- **Scalability** — design the application to grow and sustain traffic spikes.
- **High throughput / low latency** — handle many requests per second.
- **Clean Architecture** — clear separation of concerns across layers.
- **Rich domain** — entities and Value Objects (CPF, CNPJ, Email, Phone, CREF)
  with business rules and validations encapsulated in the domain.
- **Quality** — unit tests for entities and value objects.

## 🧱 Tech stack

| Area           | Technology                                              |
| -------------- | ------------------------------------------------------- |
| Runtime        | .NET 10 / C#                                             |
| API            | ASP.NET Core Web API + OpenAPI                           |
| Persistence    | Entity Framework Core 10 + Npgsql (PostgreSQL 17)       |
| Validation     | FluentValidation                                        |
| Security       | BCrypt.Net (password hashing)                           |
| Logging        | Serilog                                                 |
| Tests          | xUnit · NSubstitute · coverlet                          |
| Infrastructure | Docker Compose                                          |

## 🗂️ Project structure

The solution follows **Clean Architecture**, with dependencies pointing inward
to the domain:

```
Iron/
├── src/
│   ├── Iron.Domain/        # Entities, Value Objects, domain contracts (no dependencies)
│   ├── Iron.Aplication/    # Use cases, DTOs, validators
│   ├── Iron.Infra/         # EF Core, repositories, persistence, security
│   └── Iron.API/           # Controllers, HTTP pipeline, composition root
├── tests/
│   ├── Iron.Domain.Tests/
│   └── Iron.Aplication.Tests/
└── docker-compose.yml      # PostgreSQL for local development
```

---

## 🚀 Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (to run PostgreSQL locally)

### 1. Clone the repository

```bash
git clone https://github.com/<your-user>/Iron.git
cd Iron
```

### 2. Start the database

```bash
docker compose up -d
```

This spins up a PostgreSQL 17 instance (`iron-postgres`) on port `5432`, matching
the default connection string in `appsettings.json`.

### 3. Run the API

```bash
cd Iron/src/Iron.API
dotnet run
```

Pending EF Core migrations are applied automatically on startup, so the database
is created for you on the first run.

The API will be available at:

- HTTP  → `http://localhost:5144`
- HTTPS → `https://localhost:7116`

In **Development**, the OpenAPI document is exposed at
`http://localhost:5144/openapi/v1.json`.

### 4. Run the tests

From the repository root:

```bash
dotnet test
```

---

## 🔧 Configuration

The default connection string lives in
`Iron/src/Iron.API/appsettings.json` and points to the local Docker database:

```
Host=localhost;Port=5432;Database=iron;Username=postgres;Password=postgres
```

For environment-specific settings and secrets, use
`appsettings.Development.json`, [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets)
or environment variables — **never commit real credentials.**
`appsettings.Development.json` is intentionally git-ignored.

---

## 📌 API (current)

| Method | Route                | Description         | Auth      |
| ------ | -------------------- | ------------------- | --------- |
| POST   | `/api/auth/register` | Register a new user | Anonymous |

> The surface is intentionally minimal for now.

---

## 📄 License

This is a personal study project. Feel free to explore and learn from it.

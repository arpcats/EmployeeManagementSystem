# Employee Management System

A full-stack Employee Management System built for the Frontier Software Web
Developer coding challenge — view, search, add, and edit employee records.

- **Backend:** ASP.NET Core Web API (.NET 10), EF Core 10, SQL Server, FluentValidation, Swagger, Clean Architecture (Domain / Application / Infrastructure / API)
- **Frontend:** Angular 20 (standalone components), Reactive Forms, HttpClient
- **Testing:** xUnit + Moq (backend), Jasmine + Karma (frontend)

---

## Table of Contents

1. [Project Structure](#1-project-structure)
2. [Prerequisites](#2-prerequisites)
3. [Database Setup](#3-database-setup)
4. [Backend — Setup, Configuration, Migrations, Run](#4-backend--setup-configuration-migrations-run)
5. [Frontend — Setup, Configuration, Run](#5-frontend--setup-configuration-run)
6. [Running the Tests](#6-running-the-tests)
7. [Stopping Everything](#7-stopping-everything)
8. [Troubleshooting](#8-troubleshooting)
9. [Assumptions & Limitations](#9-assumptions--limitations)

---

## 1. Project Structure

```
EmployeeManagementSystem/
  backend/
      EmployeeManagement.Domain/          Entities, enums — no dependencies
      EmployeeManagement.Application/     DTOs, service interfaces/implementations, validators
      EmployeeManagement.Infrastructure/  EF Core DbContext, repository implementation
      EmployeeManagement.API/             Controllers, Program.cs, middleware
      EmployeeManagement.Tests/           xUnit tests
  frontend/
    src/app/
      core/
        models/employee.model.ts          TypeScript interfaces matching the API DTOs
        services/employee.ts              HttpClient wrapper for /api/employees
      features/
        employee-list/                    List + search screen
        employee-form/                    Add/edit screen (reactive form)
```

---

## 2. Prerequisites

Install these before you start:

| Tool | Version | Check with |
|---|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0+ | `dotnet --version` |
| [Node.js](https://nodejs.org/) | 20+ | `node --version` |
| npm | comes with Node | `npm --version` |
| SQL Server | 2020+ |

---

## 3. Database Setup

### Local SQL Server install

If you already have SQL Server running locally, just make sure you know:
- The server address (usually `localhost` or `localhost\SQLEXPRESS`)
- A login (SQL auth `sa`/password, or Windows auth)

You'll plug these into the connection string in the next section.

---

## 4. Backend — Setup, Configuration, Migrations, Run

### 4.1 Open a terminal in the backend folder

```bash
cd backend
```

### 4.2 Configure the connection string

Open `EmployeeManagement.API/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=EmployeeManagementDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
}
```

- ** local SQL Server ** → replace `Server`, `User Id`, and `Password` with your own values.

### 4.3 Restore NuGet packages

```bash
dotnet restore
```

### 4.4 Install the EF Core CLI tool (one-time)

```bash
dotnet tool install --global dotnet-ef
```

If it says the tool already exists, skip this step.

### 4.5 Create the database schema (migrations)

```bash
dotnet ef migrations add InitialCreate --project .\EmployeeManagement.Infrastructure --startup-project .\EmployeeManagement.API
dotnet ef database update --project src/EmployeeManagement.Infrastructure --startup-project src/EmployeeManagement.API
```

> This step is optional — `Program.cs` also calls `db.Database.Migrate()`
> automatically the first time the API starts, which creates the schema
> Running it manually here just lets you confirm it worked before
> moving on.

### 4.6 Run the API

```bash
dotnet run --project EmployeeManagement.API
```

Look for output ending with something like:

```
Now listening on: https://localhost:7148
```

### 4.7 Verify it's working

Open **https://localhost:7148/swagger**. You should see Swagger UI listing
the `/api/employees` endpoints, and be able to try `GET /api/employees` to
see the seeded sample employees (John, Tony, Jose).

Leave this terminal running — the frontend needs the API up.

---

## 5. Frontend — Setup, Configuration, Run

### 5.1 Open a **new** terminal in the frontend folder

```bash
cd frontend
```

### 5.2 Install npm packages

```bash
npm install
```

### 5.3 Configure the API URL

Open `src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiBaseUrl: 'https://localhost:7148/api'
};
```

Only change this if your API is running on a different port than `7148`
(the terminal output from Step 4.6 will tell you the actual port).

### 5.4 Run the Angular app

```bash
npm start
```

This runs `ng serve`. Once compiled, you'll see:

```
Local: http://localhost:4200/
```

### 5.5 Open the app

Go to **http://localhost:4200**. You should see the Employee Management
System with the seeded employees listed, and be able to search, add, and
edit employees.

---

## 6. Running the Tests

### Backend (xUnit)

```bash
cd backend
dotnet test
```

### Frontend (Jasmine/Karma)

```bash
cd frontend
npm test or ng test
```

This opens a Chrome window and runs the test suite. Press `Ctrl+C` in the
terminal to stop watching.

---

## 7. Stopping Everything

- Frontend: `Ctrl+C` in the `npm start` terminal.
- Backend: `Ctrl+C` in the `dotnet run` terminal.

---

## 8. Troubleshooting

| Problem | Likely fix |
|---|---|
| `dotnet ef` command not found | Run `dotnet tool install --global dotnet-ef`, then restart your terminal. |
| API can't connect to SQL Server and that the connection string in `appsettings.json` matches. |
| `Login failed for user 'sa'` | Password in `appsettings.json` doesn't match your SQL Server's SA password — update one to match the other. |
| Angular app loads but shows "Unable to load employees" | The API isn't running, or `apiBaseUrl` in `environment.ts` doesn't match the port the API printed on startup. |
| CORS error in the browser console | Confirm the API is running on the exact port the frontend expects, and that `AllowedOrigins` in `appsettings.json` includes `http://localhost:4200`. |
| `npm install` fails on Node version warnings | Angular 20 needs Node 20+; run `node --version` and upgrade if needed. |
| Port `4200` or `7148` already in use | Stop whatever else is using it, or run `ng serve --port 4300` / change `applicationUrl` in `EmployeeManagement.API/Properties/launchSettings.json`. |

---

Then open **http://localhost:4200**.

---

## 9. Assumptions & Limitations

- **No authentication** — out of scope for the challenge brief; every endpoint is open.
- **No delete endpoint** — the brief only lists GET/GET-by-id/POST/PUT, so delete was intentionally left out rather than guessed at.
- **Employment status** is a fixed 3-value enum (`Active`, `OnLeave`, `Terminated`) rather than a free-text field or a separate lookup table, since the brief doesn't specify allowed values.
- **Search** matches first name, last name, or full name, case-insensitively, and is server-side (`GET /api/employees?search=`) so it scales past an in-memory list.
- **Validation** is duplicated intentionally: Angular's Reactive Forms validators give immediate feedback, and FluentValidation re-validates server-side as the source of truth — the frontend never trusts itself alone.
- **Database migrations** aren't checked into the repo (`dotnet ef migrations add` needs to be run locally, per Section 4.5) since the generated migration is environment/EF-tooling-version specific.
- **Styling** is intentionally minimal, per the brief's "functionality over design" guidance.



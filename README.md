# MySolution

This repository contains a .NET solution based on Clean Architecture and DDD, including a **WebAPI**, a **WorkerService** (using Hangfire for scheduled upsert jobs), and a **Vue.js + TypeScript** frontend. The application performs an **upsert** of data every hour by consuming the [Fake Store API](https://fakestoreapi.com/) and displays these products in a filterable grid on the frontend.

## Project Structure

MySolution
├── src
│   ├── MySolution.Domain
│   ├── MySolution.Application
│   ├── MySolution.Infrastructure
│   ├── MySolution.BackgroundJobs
│   ├── MySolution.WebAPI
│   └── MySolution.WorkerService
├── frontend
│   └── mysolution-frontend (Vue.js + TypeScript)
└── MySolution.sln


- **MySolution.Domain**: Contains domain entities (e.g., `Product`), repository interfaces, and domain-specific logic.
- **MySolution.Application**: Application services (use cases), DTOs, validations, etc.
- **MySolution.Infrastructure**: EF Core (MSSQL) implementation, repositories, DbContext configuration, and migrations.
- **MySolution.BackgroundJobs**: Contains the Hangfire jobs (e.g., the upsert job).
- **MySolution.WebAPI**: ASP.NET Core Web API project for exposing endpoints.
- **MySolution.WorkerService**: Worker Service project that hosts the Hangfire server to run the upsert job on a schedule (every hour).
- **mysolution-frontend**: Vue.js (TypeScript) application that displays data in a filterable grid.

## How It Works

1. **WorkerService**:
   - Runs Hangfire and schedules a job every hour to fetch product data from the [Fake Store API](https://fakestoreapi.com/products).
   - Performs an **upsert** operation (inserting or updating) in the local MSSQL database.

2. **WebAPI**:
   - Exposes endpoints (e.g., `GET /api/products`) that return data from the database.

3. **Frontend (Vue.js + TypeScript)**:
   - Calls the WebAPI to retrieve products.
   - Displays the data in a table with basic filtering functionality.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet)
- [Node.js](https://nodejs.org/) (for the Vue.js frontend)
- A [MSSQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) server or instance
- [Git](https://git-scm.com/) for version control

## Getting Started

### 1. Clone the Repository

bash

git clone https://github.com/JulioBecker/HahnTest.git

### 2. Adjust the Connection String

Update the DefaultConnection string in both MySolution.WebAPI/appsettings.json and MySolution.WorkerService/appsettings.json to point to your MSSQL instance, for example:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=MySolutionDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}

### 3. Run Database Migrations

From the project root or from the WebAPI folder, run:

dotnet ef migrations add InitialCreate --project ./src/MySolution.Infrastructure/MySolution.Infrastructure.csproj --startup-project ./src/MySolution.WebAPI/MySolution.WebAPI.csproj -o Data/Migrations

dotnet ef database update --project ./src/MySolution.Infrastructure/MySolution.Infrastructure.csproj --startup-project ./src/MySolution.WebAPI/MySolution.WebAPI.csproj

This creates and applies the initial migration, generating the necessary tables (e.g., Products, Hangfire schemas, etc.).

### 4. Run the WebAPI

cd src/MySolution.WebAPI
dotnet run

By default, it might run on https://localhost:5001 or http://localhost:5000 (depending on your environment).

### 5. Run the WorkerService

Open another terminal:

cd src/MySolution.WorkerService


dotnet run

This service schedules the Hangfire job that fetches data from the Fake Store API every hour. The job will upsert those products into the MSSQL database.

### 6. Run the Frontend (Vue.js + TypeScript)

cd frontend/mysolution-frontend

npm install

npm run dev

The application will start, typically on http://localhost:5173 (or another port shown in the console). Access it in your browser to see the products grid.

## Additional Details
### Clean Architecture:
Domain (core entities and logic)
Application (use cases and DTOs)
Infrastructure (EF Core, repositories, DbContext)
BackgroundJobs (Hangfire jobs)
### DDD:
Each entity encapsulates domain rules and logic.
### SOLID Principles:
Applied where applicable.
### Hangfire:
Used for job scheduling (hourly upsert).
### Fake Store API:
Data consumed in the WorkerService job.
## License
Feel free to use this project as a reference or starting point for your own solutions.

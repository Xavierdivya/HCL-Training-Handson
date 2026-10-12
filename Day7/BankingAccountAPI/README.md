# Banking Account API

## Overview
The Banking Account API is a RESTful web application developed using ASP.NET Core Web API and C#. It allows users to manage bank accounts and perform basic banking transactions such as deposits and withdrawals.

This project was developed as part of HCL training to practice API development, database integration, CRUD operations, and request validation.

## Technologies Used
- C#
- ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- SQL Server LocalDB
- Swagger
- Postman
- Visual Studio 2026

## Features
- Create a new bank account.
- Retrieve all bank accounts.
- Retrieve account details using an account ID.
- Update account holder name and account type.
- Delete a bank account.
- Filter accounts by account type (Savings or Current).
- Deposit money into an account.
- Withdraw money from an account.
- Validate account details and transaction amounts.
- Prevent duplicate account numbers.
- Prevent withdrawals when the balance is insufficient.

## Project Structure

```text
BankingAccountAPI/
├── Controllers/
│   └── AccountsController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── TransactionRequest.cs
│   └── UpdateAccountRequest.cs
├── Migrations/
├── Models/
│   └── BankAccount.cs
├── Properties/
├── Program.cs
├── appsettings.json
├── BankingAccountAPI.csproj
└── README.md
```

## Database Configuration
The application uses SQL Server LocalDB as its database and Entity Framework Core for database operations.

The database connection string is configured in `appsettings.json`.

The database name is `BankingAccountDB`.

## API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| GET | `/api/accounts` | Retrieve all accounts |
| GET | `/api/accounts/{id}` | Retrieve an account by ID |
| POST | `/api/accounts` | Create a new account |
| PUT | `/api/accounts/{id}` | Update account details |
| DELETE | `/api/accounts/{id}` | Delete an account |
| POST | `/api/accounts/{id}/deposit` | Deposit money |
| POST | `/api/accounts/{id}/withdraw` | Withdraw money |

### Account Filtering
Accounts can be filtered by account type using the `accountType` query parameter.

Examples:

```http
GET /api/accounts?accountType=Savings
GET /api/accounts?accountType=Current
```

## How to Run the Project
1. Open `BankingAccountAPI.csproj` in Visual Studio.
2. Restore the required NuGet packages.
3. Ensure SQL Server LocalDB is running.
4. Apply database migrations if required.
5. Run the project using Visual Studio.
6. Open the Swagger URL displayed by the application.
7. Test the API endpoints using Swagger or Postman.

## Testing
The API endpoints were tested using Postman.

Successful tests included account creation, retrieving account details, updating account information, depositing money, withdrawing money, filtering accounts, and validating invalid requests.

## Learning Outcomes
- Developing RESTful APIs using ASP.NET Core.
- Implementing CRUD operations.
- Integrating SQL Server with Entity Framework Core.
- Using Data Transfer Objects (DTOs).
- Applying model validation.
- Handling HTTP status codes and errors.
- Testing APIs using Postman and Swagger.

## Author
HCL Training — Day 7 Project

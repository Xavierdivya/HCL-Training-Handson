# Leave Management API

A RESTful Web API built using ASP.NET Core, C#, Entity Framework Core, and SQL Server to manage employee leave requests.

## Technologies Used

- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- SQL Server LocalDB
- Swagger / OpenAPI

## Features

- Create a leave request
- Retrieve all leave requests
- Retrieve a leave request by ID
- Filter leave requests by status
- Update an existing leave request
- Delete a leave request
- Validate required fields and leave dates
- Restrict status values to Pending, Approved, or Rejected
- Store leave requests in SQL Server

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/LeaveRequests` | Retrieve all requests |
| GET | `/api/LeaveRequests/{id}` | Retrieve a request by ID |
| GET | `/api/LeaveRequests?status=Pending` | Filter by status |
| POST | `/api/LeaveRequests` | Create a request |
| PUT | `/api/LeaveRequests/{id}` | Update a request |
| DELETE | `/api/LeaveRequests/{id}` | Delete a request |

## Database

The application uses SQL Server LocalDB with a database named `LeaveManagementDB`.

Apply migrations using the Visual Studio Package Manager Console:

```powershell
Update-Database
```

## Run the Project

1. Open `LeaveManagementAPI.sln` in Visual Studio.
2. Restore NuGet packages if prompted.
3. Verify the SQL Server LocalDB connection string in `appsettings.json`.
4. Run the project using F5.
5. Open the Swagger UI page shown in the application output.

## Example Request

```json
{
  "employeeName": "Divya",
  "leaveType": "Casual Leave",
  "startDate": "2026-10-15T00:00:00",
  "endDate": "2026-10-16T00:00:00",
  "reason": "Personal work",
  "status": "Pending"
}
```

New leave requests are assigned the `Pending` status by the API.

## Validation

The API validates required fields, accepted status values, and date order. Invalid requests return HTTP 400 Bad Request.

## License

This project is intended for learning and practice.

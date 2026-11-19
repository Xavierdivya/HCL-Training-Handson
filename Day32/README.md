# Day 32 – Revision & Frontend–Backend Integration

## Overview
Day 32 focused on revising previously learned full-stack development concepts and understanding how to integrate a frontend application with a backend API.

The main objective was to understand how Angular communicates with ASP.NET Core Web API, exchanges data, and performs operations using HTTP requests.

## Topics Revised

### 1. Frontend Development – Angular
- Angular project structure
- Components and services
- TypeScript fundamentals
- Data binding
- Routing
- Dependency Injection
- Angular Forms
- HTTP Client
- Observables and RxJS

### 2. Backend Development – ASP.NET Core Web API
- C# and OOP concepts
- Controllers and API endpoints
- HTTP methods (GET, POST, PUT, DELETE)
- Dependency Injection
- Service and Repository concepts
- Entity Framework Core
- Database operations
- REST API principles

### 3. Frontend–Backend Integration
- Connecting Angular with ASP.NET Core Web API
- Configuring Angular HTTP Client
- Creating API services
- Sending HTTP requests from Angular
- Receiving JSON responses from backend
- Displaying backend data in Angular components
- Handling API errors
- Understanding asynchronous API communication
- Configuring CORS in ASP.NET Core

### 4. Database Integration
- SQL Server connectivity
- Entity Framework Core
- Performing CRUD operations
- Retrieving and storing data through APIs

## Integration Workflow

1. User performs an action in the Angular frontend.
2. Angular component calls the corresponding service.
3. Angular service sends an HTTP request using HttpClient.
4. ASP.NET Core Web API receives the request.
5. Controller processes the request through the backend logic.
6. Entity Framework Core communicates with SQL Server.
7. Backend returns a JSON response.
8. Angular receives the response and updates the UI.

## Technologies & Tools
- **Frontend:** Angular, TypeScript, HTML, CSS, Bootstrap
- **Backend:** C#, ASP.NET Core Web API
- **Database:** SQL Server, Entity Framework Core
- **Communication:** REST API, HTTP, JSON
- **Tools:** Visual Studio, VS Code, Postman
- **Version Control:** Git and GitHub

## Key Learning Outcomes
- Revised important frontend and backend concepts.
- Understood the communication between frontend and backend.
- Learned how to consume REST APIs using Angular HttpClient.
- Understood how data flows between Angular, ASP.NET Core and SQL Server.
- Practised CRUD operations through API integration.
- Understood CORS and asynchronous API communication.
- Improved understanding of real-world full-stack application architecture.

## Learning Outcome
Day 32 strengthened my understanding of full-stack application development, especially how independently developed frontend and backend applications work together through REST APIs.

**Next Step:** Apply these concepts in the Food Menu Management System by implementing features module by module.
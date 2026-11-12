# Angular Task Tracker Demo

A small standalone Angular project demonstrating components, TypeScript interfaces, services, dependency injection, routing, template-driven forms, data binding, `@if`, `@for`, and basic add/complete/delete operations.

## Run
1. Extract the ZIP and open this folder in VS Code.
2. Open a terminal in this folder.
3. Run `npm install`.
4. Run `npm start`.
5. Open the local URL shown by Angular CLI (normally http://localhost:4200).

Build with `npm run build`.

## File map
- `src/main.ts`: application entry point
- `src/app/app.ts`, `app.html`: root component and navigation
- `src/app/app.config.ts`: router provider configuration
- `src/app/app.routes.ts`: URL-to-component mapping
- `src/app/models/task.ts`: Task interface
- `src/app/services/task.service.ts`: shared task data and operations
- `src/app/pages/home/`: home page
- `src/app/pages/task-list/`: task form, list and actions

## Note
Tasks are held in an in-memory array in `TaskService`; refreshing the browser resets changes. No API or database is connected.

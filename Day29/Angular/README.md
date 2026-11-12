# ANGULAR LEARNING PROJECTS – README

## Overview

This repository contains two beginner-friendly Angular projects created to understand Angular fundamentals, project structure, component communication, services, dependency injection, routing, data binding, and TypeScript concepts.

### Projects Included

1. Student Details Angular Project
2. Angular Task Tracker Demo

---

# PROJECT 1: STUDENT DETAILS ANGULAR PROJECT

## 1. Project Description

The Student Details application demonstrates how to build a simple Angular application to manage student information.

It helps understand how Angular components, services, interfaces, data binding, and dependency injection work together.

### Main Features

- Display student details.
- Add new students.
- Store student information using a service.
- Display student records using reusable components.
- Navigate between application pages.

## 2. Important Concepts

| Concept | Purpose |
|---|---|
| Component | Controls a particular part of the UI |
| Template | Defines the HTML displayed by a component |
| TypeScript | Contains application logic |
| Interface | Defines the structure of student data |
| Service | Manages and shares student data |
| Dependency Injection | Provides service instances to components |
| Data Binding | Connects component data with HTML |
| Routing | Navigates between application pages |
| `@for` | Iterates over student records |
| `@if` | Conditionally displays content |

## 3. Project File Structure

```text
StudentDetails/
│
├── src/
│   ├── main.ts
│   │
│   └── app/
│       ├── app.ts
│       ├── app.html
│       ├── app.config.ts
│       ├── app.routes.ts
│       │
│       ├── models/
│       │   └── student.ts
│       │
│       ├── services/
│       │   └── student.service.ts
│       │
│       └── components/
│           ├── student-list/
│           ├── student-card/
│           └── student-form/
│
├── angular.json
├── package.json
└── tsconfig.json
```

*Note: This is a conceptual file mapping. Actual filenames and folders may differ depending on the supplied project version.*

## 4. File Responsibilities

| File | Responsibility |
|---|---|
| `main.ts` | Bootstraps the Angular application |
| `app.ts` | Root component logic |
| `app.html` | Root component template |
| `app.config.ts` | Application-level configuration and providers |
| `app.routes.ts` | Defines application routes |
| `student.ts` | Defines the Student interface |
| `student.service.ts` | Stores and manages student information |
| `student-list` | Displays multiple students |
| `student-card` | Displays an individual student's details |
| `student-form` | Handles student input |

## 5. Application Workflow

```text
Browser
   |
   v
main.ts
   |
   v
Bootstrap Angular Application
   |
   v
App Component
   |
   v
Router
   |
   v
Student Form Component
   |
   v
User Enters Student Details
   |
   v
Data Binding
   |
   v
Student Service
   |
   v
Student Data Stored
   |
   v
Student List Component
   |
   v
@for
   |
   v
Student Card Component
   |
   v
Student Details Displayed
```

## 6. Important Angular Concepts

### A. Component

A component controls a particular part of the user interface.

A typical component contains:

- TypeScript file – application logic.
- HTML file – UI structure.
- CSS file – styling.

### B. Interface

An interface defines the structure of an object.

Example:

```typescript
export interface Student {
    id: number;
    name: string;
    department: string;
    age: number;
    email?: string;
}
```

The `email` property is optional because it uses `?`.

### C. Service

A service contains reusable business logic and shared data.

Example:

```typescript
@Injectable({
    providedIn: 'root'
})
export class StudentService {

    students: Student[] = [];

    addStudent(student: Student): void {
        this.students.push(student);
    }

    getStudents(): Student[] {
        return this.students;
    }
}
```

### D. Dependency Injection

Dependency Injection allows Angular to provide a service instance to a component instead of requiring the component to create it manually.

Example:

```typescript
constructor(private studentService: StudentService) {}
```

Modern Angular also supports:

```typescript
private studentService = inject(StudentService);
```

### E. Data Binding

Data binding connects component data and the HTML template.

Common types:

1. Interpolation – `{{ student.name }}`
2. Property Binding – `[value]="studentName"`
3. Event Binding – `(click)="addStudent()"`
4. Two-way Binding – `[(ngModel)]="studentName"`

---

# PROJECT 2: ANGULAR TASK TRACKER DEMO

## 1. Project Description

The Angular Task Tracker is a simple task management application developed using Angular standalone components.

It demonstrates how to create, display, complete, and delete tasks.

### Main Features

- Add a new task.
- Display all tasks.
- Mark tasks as completed.
- Undo task completion.
- Delete tasks.
- Display completed task counts.
- Navigate between Home and Tasks pages.

## 2. Technologies Used

- Angular
- TypeScript
- HTML
- CSS
- Node.js
- npm
- Angular Router

## 3. Project File Structure

```text
Angular-Task-Tracker-Demo/
│
├── src/
│   ├── index.html
│   ├── main.ts
│   ├── styles.css
│   │
│   └── app/
│       ├── app.ts
│       ├── app.html
│       ├── app.css
│       ├── app.config.ts
│       ├── app.routes.ts
│       │
│       ├── models/
│       │   └── task.ts
│       │
│       ├── services/
│       │   └── task.service.ts
│       │
│       └── pages/
│           ├── home/
│           │   ├── home.ts
│           │   ├── home.html
│           │   └── home.css
│           │
│           └── task-list/
│               ├── task-list.ts
│               ├── task-list.html
│               └── task-list.css
│
├── angular.json
├── package.json
├── tsconfig.json
├── tsconfig.app.json
└── README.md
```

## 4. File Responsibilities

| File | Responsibility |
|---|---|
| `main.ts` | Starts the Angular application |
| `index.html` | Main browser HTML page |
| `app.ts` | Root component logic |
| `app.html` | Root component template |
| `app.config.ts` | Configures application providers |
| `app.routes.ts` | Defines Home and Tasks routes |
| `task.ts` | Defines the Task interface |
| `task.service.ts` | Manages task data and operations |
| `home.ts` | Handles Home page logic |
| `home.html` | Displays the Home page |
| `task-list.ts` | Handles task list operations |
| `task-list.html` | Displays tasks and action buttons |
| `styles.css
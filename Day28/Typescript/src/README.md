# TypeScript Practice – Student Management

## 1. Project Overview

This project is created as part of my Angular and TypeScript learning journey.

The main objective is to understand TypeScript fundamentals, including interfaces, data types, union types, optional properties, interface inheritance, arrays, and functions.

A simple Student Management program is implemented to practice these concepts.

## 2. Technologies Used

- TypeScript
- Node.js
- npm
- Visual Studio Code

## 3. Concepts Covered

### 3.1 Interface

An interface defines the structure of an object by specifying its properties and their data types.

Example:

```typescript
interface Student {
    id: number;
    name: string;
    department: string;
    age: number;
}
```

Any object implementing this interface must contain the specified properties with matching data types.

### 3.2 Union Type

A union type allows a variable or property to hold more than one specified data type.

Example:

```typescript
id: number | string;
```

Here, the student ID can be either a number or a string.

Examples:

```typescript
id: 101
id: "S102"
```

### 3.3 Optional Properties

An optional property is a property that does not need to be present in every object.

Example:

```typescript
email?: string;
```

The `?` symbol makes the email property optional.

Therefore, a student object can contain an email address or omit it.

### 3.4 Interface Inheritance

TypeScript supports interface inheritance using the `extends` keyword.

Example:

```typescript
interface Student {
    id: number | string;
    name: string;
    department: string;
    age: number;
    email?: string;
}

interface ContactDetails {
    phone: string;
    address: string;
}

interface StudentWithContact extends Student, ContactDetails {
}
```

The `StudentWithContact` interface inherits properties from both `Student` and `ContactDetails`.

Therefore, it contains:

- Student ID
- Name
- Department
- Age
- Email (optional)
- Phone
- Address

### 3.5 Array of Objects

An array can store multiple objects following the same interface structure.

Example:

```typescript
const students: Student[] = [
    {
        id: 101,
        name: "Divya",
        department: "Computer Science",
        age: 23
    }
];
```

`Student[]` specifies that the array can contain multiple objects following the Student interface.

### 3.6 Functions

Functions are used to perform reusable operations.

Example:

```typescript
function displayStudents(students: Student[]): void {
    students.forEach((student) => {
        console.log(student.name);
    });
}
```

Explanation:

- `displayStudents` is the function name.
- `students: Student[]` defines the parameter type.
- `void` indicates that the function does not return a value.
- `forEach()` iterates through each student object.

### 3.7 Nullish Coalescing Operator

The `??` operator provides a default value when an expression is either `null` or `undefined`.

Example:

```typescript
console.log(student.email ?? "Not provided");
```

If the student has an email, it displays the email.

Otherwise, it displays `"Not provided"`.

## 4. Program Implementation

The program includes:

1. Creating a Student interface.
2. Defining a union type for the student ID.
3. Making the email property optional.
4. Creating a ContactDetails interface.
5. Combining interfaces using inheritance.
6. Creating an array containing five student objects.
7. Implementing a function to display student details.
8. Displaying all five students.
9. Creating a student object with additional contact details.

## 5. How to Execute the Program

### Step 1: Initialize the Node.js project

```bash
npm init -y
```

### Step 2: Install TypeScript

```bash
npm install typescript --save-dev
```

### Step 3: Initialize TypeScript configuration

```bash
npx tsc --init
```

### Step 4: Create the TypeScript file

Create a file named:

```text
index.ts
```

Paste the Student Management program into this file.

### Step 5: Compile TypeScript into JavaScript

```bash
npx tsc index.ts
```

This generates:

```text
index.js
```

### Step 6: Execute the JavaScript file

```bash
node index.js
```

## 6. Expected Output

```text
STUDENT DETAILS

----------------------
ID: 101
Name: Divya
Department: Computer Science
Age: 23
Email: divya@gmail.com

----------------------
ID: S102
Name: Anu
Department: Information Technology
Age: 22
Email: Not provided

----------------------
ID: 103
Name: Rahul
Department: Electronics
Age: 24
Email: rahul@gmail.com

----------------------
ID: S104
Name: Arun
Department: Mechanical
Age: 23
Email: Not provided

----------------------
ID: 105
Name: Meera
Department: Civil
Age: 21
Email: meera@gmail.com

STUDENT WITH CONTACT DETAILS

{
    id: 106,
    name: 'John',
    department: 'Computer Science',
    age: 22,
    email: 'john@gmail.com',
    phone: '9876543210',
    address: 'Kerala, India'
}
```

## 7. Key Learnings

After completing this exercise, I understood:

- How to define interfaces in TypeScript.
- How to use union types.
- How to define optional properties.
- How to inherit multiple interfaces.
- How to create strongly typed arrays.
- How to use functions with parameter and return types.
- How to iterate through arrays using `forEach()`.
- How to use the nullish coalescing operator (`??`).
- How TypeScript code is compiled into JavaScript.

## 8. Learning Outcome

Successfully implemented a Student Management program using TypeScript interfaces, inheritance, union types, optional properties, arrays, and functions.

This exercise provides a foundation for understanding TypeScript concepts that will be used in Angular components, services, models, and dependency injection.

---

**Learning Track:** Angular & TypeScript

**Practice Topic:** TypeScript Fundamentals – Interfaces and Data Types

**Status:** Completed
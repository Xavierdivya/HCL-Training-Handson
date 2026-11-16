# Angular Signals Counter App

A simple interactive Counter Application built using Angular Signals. This project demonstrates three important Angular reactive programming concepts: `signal()`, `computed()`, and `effect()`.

## Technologies Used

- Angular
- TypeScript
- HTML5
- CSS3

## Concepts Covered

### 1. Signal (`signal()`)

Used to create and manage reactive state. The `count` signal stores the current counter value.

```typescript
count = signal(0);
```

- `set()` assigns a new value.
- `update()` modifies the existing value.

### 2. Computed Signal (`computed()`)

Used to derive values from other signals. Computed signals are read-only and automatically track their dependencies.

```typescript
square = computed(() => this.count() * this.count());

status = computed(() =>
  this.count() % 2 === 0 ? 'Even' : 'Odd'
);
```

### 3. Effect (`effect()`)

Used to perform side effects whenever the signals read inside the effect change.

```typescript
effect(() => {
  console.log('Counter updated:', this.count());
});
```

## Features

- Increment the counter.
- Decrement the counter.
- Reset the counter to zero.
- Automatically calculate the square of the counter.
- Automatically identify whether the counter is even or odd.
- Log counter changes in the browser console.
- Reactive UI updates using Angular Signals.

## Project Structure

```text
signal-counter/
├── src/
│   └── app/
│       ├── app.ts
│       ├── app.html
│       └── app.css
├── angular.json
├── package.json
└── README.md
```

## Prerequisites

- Node.js and npm
- Angular CLI

Install Angular CLI if required:

```bash
npm install -g @angular/cli
```

## Installation and Execution

1. Clone the repository or open the project directory.

2. Navigate to the project folder:

```bash
cd signal-counter
```

3. Install dependencies:

```bash
npm install
```

4. Start the Angular development server:

```bash
ng serve --open
```

5. Open the application at:

```text
http://localhost:4200
```

## Expected Output

| Action | Count | Square | Status |
|---|---:|---:|---|
| Initial | 0 | 0 | Even |
| Increment | 1 | 1 | Odd |
| Increment | 2 | 4 | Even |
| Decrement | 1 | 1 | Odd |
| Reset | 0 | 0 | Even |

## Learning Outcomes

After completing this project, you will understand:

- How to create and update Angular Signals.
- Difference between `set()` and `update()`.
- How computed signals derive values from source signals.
- How effects track signal dependencies and execute side effects.
- How Angular automatically updates the UI when reactive state changes.

## Key Differences

| Concept | Purpose | Example |
|---|---|---|
| `signal()` | Stores reactive state | `count` |
| `computed()` | Derives reactive values | `square`, `status` |
| `effect()` | Performs side effects | Console logging |

## Future Enhancements

- Add minimum and maximum counter limits.
- Add a progress bar using computed signals.
- Persist the counter value using localStorage and effect.
- Add a dark/light theme.

---

**Project:** Angular Signals Counter App  
**Purpose:** Hands-on practice of Angular Signals, Computed Signals and Effects.
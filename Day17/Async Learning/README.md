# Day17 – Asynchronous Programming in C#

## 1. Project Overview

This project demonstrates asynchronous programming in C# using `async`, `await`, `Task`, and `Task<T>`.

The objective is to understand how asynchronous operations work, how multiple tasks can execute concurrently, how to handle exceptions, and how to cancel running operations.

## 2. Technologies Used

- C#
- .NET 10
- Visual Studio
- Task-based Asynchronous Programming (TAP)
- System.Threading.Tasks
- System.Diagnostics

## 3. Concepts Covered

### 3.1 Basic Async and Await

**Method:** `BasicAsync()`

- `async` allows a method to use the `await` keyword.
- `await` waits asynchronously for an operation to complete.
- `Task.Delay()` simulates an asynchronous operation.

**Execution Flow:**

1. Download started.
2. Wait asynchronously for 2 seconds.
3. Download completed.
4. Processing started.

**Key Learning:** The `await` keyword does not block the calling thread while waiting for an asynchronous operation.

### 3.2 Task Tracking and Task<T>

**Methods:** `TrackTaskAsync()` and `TaskTrackingAsync()`

- `Task` represents an asynchronous operation.
- `Task<T>` represents an asynchronous operation that returns a value.
- `IsCompleted` checks whether the task has completed.
- `Status` indicates the current state of a task.
- `await` retrieves the result after task completion.

**Execution Flow:**

1. Main method starts.
2. Sub-method starts.
3. Main method continues executing.
4. Task status is checked.
5. Main method waits for the task using `await`.
6. Sub-method completes and returns 12.
7. Main method displays the result.

**Key Learning:** An asynchronous method can return a value using `Task<T>`.

### 3.3 Sequential Execution

**Method:** `SequentialAsync()`

Sequential execution means one asynchronous operation completes before the next operation starts.

Example:

- File A takes 2 seconds.
- File B takes 3 seconds.
- File B starts only after File A completes.

**Expected Execution Time:** Approximately 5 seconds.

**Key Learning:** Using `await` one after another executes asynchronous operations sequentially.

### 3.4 Concurrent Execution

**Method:** `ConcurrentAsync()`

Concurrent execution allows multiple asynchronous operations to start without waiting for each other individually.

Example:

- File A takes 2 seconds.
- File B takes 3 seconds.
- Both operations start before either is awaited.

`Task.WhenAll()` waits until both operations complete.

**Expected Execution Time:** Approximately 3 seconds.

**Key Learning:** Concurrent execution can reduce the total execution time when independent operations run together.

### 3.5 Task.WhenAll()

**Method:** `WhenAllAsync()`

`Task.WhenAll()` waits until all supplied tasks complete.

In this example:

- Task 1 returns 20 after 2 seconds.
- Task 2 returns 30 after 3 seconds.
- Task 3 returns 40 after 1 second.

The results are stored in an integer array.

**Execution Flow:**

1. Create three asynchronous tasks.
2. Start all three tasks.
3. Wait for all tasks using `Task.WhenAll()`.
4. Collect their return values.
5. Display all results.

**Expected Output:**

```text
All tasks completed
20
30
40
```

**Key Learning:** `Task.WhenAll()` is useful when all asynchronous operations must finish before continuing.

### 3.6 Task.WhenAny()

**Method:** `WhenAnyAsync()`

`Task.WhenAny()` returns when the first task completes.

In this example:

- Operation A takes 4 seconds.
- Operation B takes 1.5 seconds.
- Operation C takes 3 seconds.

Operation B completes first.

**Execution Flow:**

1. Start three asynchronous operations.
2. Wait for the first task using `Task.WhenAny()`.
3. Display the first completed task's result.
4. Wait for the remaining tasks using `Task.WhenAll()`.

**Expected Output:**

```text
First completed: Operation B finished
All operations have now completed
```

**Key Learning:** `Task.WhenAny()` is useful when the first available result is needed, while other tasks may continue running.

### 3.7 Exception Handling in Async Methods

**Method:** `ExceptionHandlingAsync()`

Exceptions occurring in asynchronous methods can be handled using `try-catch-finally`.

- `try`: Contains the operation that may throw an exception.
- `catch`: Handles exceptions.
- `finally`: Executes cleanup logic whether an exception occurs or not.

In this example, `RiskyOperationAsync()` throws an `InvalidOperationException`.

**Execution Flow:**

1. Start the operation.
2. Wait for one second.
3. An exception is thrown.
4. The appropriate catch block handles it.
5. Finally block executes.

**Expected Output:**

```text
Starting operation...
Operation error: Learner could not be registered
Cleanup completed
```

**Key Learning:** Use `try-catch` around awaited operations to handle asynchronous exceptions.

### 3.8 CancellationToken and CancellationTokenSource

**Method:** `CancellationAsync()`

Cancellation allows an asynchronous operation to stop before it finishes.

Important classes:

- `CancellationTokenSource`: Requests cancellation.
- `CancellationToken`: Passes the cancellation request to an operation.
- `Cancel()`: Requests cancellation.
- `ThrowIfCancellationRequested()`: Throws an `OperationCanceledException` if cancellation has been requested.

**Execution Flow:**

1. Create a `CancellationTokenSource`.
2. Start processing learners.
3. Process learners one by one.
4. Request cancellation after 2 seconds.
5. The running operation detects cancellation.
6. Handle `OperationCanceledException`.

**Key Learning:** Cancellation is cooperative. The running operation must check and respond to the cancellation token.

### 3.9 Task.Run()

**Method:** `TaskRunAsync()`

`Task.Run()` schedules work on a thread-pool thread.

In this example:

- A background operation calculates the sum of numbers from 1 to 5.
- `Thread.Sleep(500)` simulates blocking work.
- The main method continues doing other work.
- `await` retrieves the final result.

**Expected Output:**

```text
Main method started
Background work started
Main method is doing other work
Background work completed
Result: 15
Main method completed
```

**Key Learning:**

- `Task.Run()` is useful for CPU-bound or blocking work that should not occupy the calling thread.
- `await` allows the calling method to continue after the background operation completes.
- `Thread.Sleep()` blocks a thread, whereas `Task.Delay()` waits asynchronously.

## 4. Task.Delay() vs Thread.Sleep()

| Task.Delay() | Thread.Sleep() |
|---|---|
| Asynchronous delay | Synchronous delay |
| Does not block the calling thread while waiting | Blocks the current thread |
| Can be awaited | Cannot be awaited |
| Suitable for asynchronous waiting | Suitable for intentional blocking |

## 5. Task.WhenAll() vs Task.WhenAny()

| Task.WhenAll() | Task.WhenAny() |
|---|---|
| Waits for all tasks | Waits for the first task to complete |
| Returns results of all generic tasks | Returns the first completed task |
| Useful when all results are required | Useful when the fastest result is required |

## 6. Important Keywords and Classes

| Keyword / Class | Purpose |
|---|---|
| `async` | Marks a method as asynchronous |
| `await` | Asynchronously waits for a task |
| `Task` | Represents an asynchronous operation |
| `Task<T>` | Represents an operation that returns a value |
| `Task.Delay()` | Creates an asynchronous delay |
| `Task.Run()` | Schedules work on a thread-pool thread |
| `Task.WhenAll()` | Waits for all tasks |
| `Task.WhenAny()` | Waits for the first completed task |
| `Stopwatch` | Measures execution time |
| `CancellationToken` | Communicates cancellation requests |
| `CancellationTokenSource` | Initiates cancellation |
| `try-catch-finally` | Handles exceptions and cleanup |

## 7. Program Execution

The `Program.cs` file controls which method executes.

Example:

```csharp
using Day17Practice;

LearnAsyncProgramming learning = new LearnAsyncProgramming();

await learning.TaskRunAsync();
```

To execute another concept, comment out the current method and uncomment the required method.

Example:

```csharp
await learning.SequentialAsync();
```

## 8. Learning Outcomes

After completing this project, I can:

- Understand asynchronous programming in C#.
- Implement methods using `async` and `await`.
- Understand `Task` and `Task<T>`.
- Track task execution and task status.
- Differentiate sequential and concurrent execution.
- Use `Task.WhenAll()` and `Task.WhenAny()`.
- Handle exceptions in asynchronous methods.
- Cancel asynchronous operations using cancellation tokens.
- Execute background work using `Task.Run()`.
- Understand the difference between `Task.Delay()` and `Thread.Sleep()`.

## 9. Practical Applications

These concepts are useful in real-world applications such as:

- Downloading multiple files.
- Calling multiple Web APIs.
- Database operations.
- Background processing.
- File processing.
- Sending emails.
- Handling long-running operations.
- Cancelling user-requested operations.

## 10. Project Conclusion

This project provides hands-on experience with asynchronous programming in C#.

It demonstrates how to execute operations asynchronously, improve application responsiveness, coordinate multiple tasks, handle exceptions, and manage cancellation.

**Day17 Learning Status:** Completed – Asynchronous Programming Fundamentals.
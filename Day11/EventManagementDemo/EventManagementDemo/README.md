# Event Management Demo – C# Delegates and Lambda Expressions

## 📌 Overview

This project is a C# console application created to practice **delegates and predefined generic delegates** in C#. It demonstrates how methods can be assigned to delegates, how methods can be passed as arguments, and how lambda expressions can be used to implement conditions dynamically.

The project covers custom delegates and the predefined `Action<T>`, `Func<T, TResult>`, and `Predicate<T>` delegates provided by .NET.

## 🎯 Objectives

- Understand the concept of delegates in C#.
- Create and use custom delegates.
- Explore predefined generic delegates: `Action`, `Func`, and `Predicate`.
- Pass methods as arguments to other methods.
- Implement conditions using lambda expressions.
- Practice generic methods and string validation.

## 🛠️ Technologies Used

- **Language:** C#
- **Platform:** .NET
- **Application Type:** Console Application
- **IDE:** Visual Studio

## 📂 Project Structure

```text
EventManagementDemo/
│
├── EventManagement/
│   └── Event.cs
│
├── Program.cs
└── README.md
```

## 📚 Concepts Practiced

### 1. Custom Delegate and Action

**Methods:** `Run()`, `Display()`

- Defines a custom delegate named `delgName`.
- Assigns the `Display()` method to the custom delegate.
- Uses the predefined `Action<string>` delegate to perform the same operation.
- Demonstrates delegates that call methods without returning a value.

### 2. Custom Function Delegate and Func

**Methods:** `Run1()`, `Add()`

- Defines a custom delegate named `delgName2`.
- Assigns the `Add()` method to the delegate.
- Uses `Func<int, int, int>` to perform addition.
- Demonstrates how delegates can return values.

**Example:**
- `Add(5, 6)` returns `11`.
- `Add(2, 3)` returns `5`.

### 3. Custom Predicate and Predicate<T>

**Methods:** `Run3()`, `IsValid()`

- Defines a custom Boolean delegate named `delgName3`.
- Checks whether a given integer is greater than 18.
- Uses `Predicate<int>` to perform the same validation.
- Demonstrates delegates that return a Boolean value.

### 4. Passing Methods as Arguments

**Methods:** `Run4()`, `CheckString()`, `IsPalindrome()`

- Accepts a string and a `Predicate<string>` as method arguments.
- Checks whether a string is a palindrome.
- Uses lambda expressions to check different string conditions.

**Conditions demonstrated:**
- Check whether a string is a palindrome.
- Check whether all characters in a string are distinct.
- Check whether the string length is greater than 8.
- Check whether the first and last characters are equal.

### 5. Generic Method and Predicate<T>

**Methods:** `Run5()`, `CheckValue<T>()`

- Implements a generic method that accepts a value and a `Predicate<T>`.
- Demonstrates how generic methods can work with different data types.
- Uses lambda expressions to evaluate conditions on strings and integer arrays.

## ▶️ How to Run the Project

1. Open the project in Visual Studio.
2. Ensure that `Program.cs` and `Event.cs` are included in the project.
3. Build the solution using **Build → Build Solution**.
4. Run the application using **Ctrl + F5**.
5. View the delegate and lambda expression results in the console.

## 💡 Key Learning Outcomes

After completing this practice, you will understand:

- The difference between custom and predefined delegates.
- How `Action<T>` is used for methods that return no value.
- How `Func<T, TResult>` is used for methods that return a value.
- How `Predicate<T>` represents a condition that returns `true` or `false`.
- How lambda expressions simplify conditional logic.
- How generic methods support reusable code for different types.

## 🚀 Conclusion

This practice strengthens the understanding of C# delegates, generic delegates, lambda expressions, and method references. These concepts are useful for writing reusable code and working with LINQ, callbacks, and event-driven programming in .NET.

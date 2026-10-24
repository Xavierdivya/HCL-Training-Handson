# LINQ Mastery Practice Project

## 📌 Project Overview

**LINQ Mastery** is a C# console application designed to practice and understand Language Integrated Query (LINQ) using sample business data. The project demonstrates how to filter, transform, sort, group, join, and analyze collections using both **LINQ Method Syntax** and **LINQ Query Syntax**.

The project uses in-memory sample data representing products, customers, orders, departments, employees, and training batches. It is intended for learning and practicing LINQ operations through hands-on exercises.

## 🎯 Objectives

- Understand LINQ fundamentals in C#.
- Practice Method Syntax and Query Syntax.
- Retrieve and transform data from collections.
- Perform grouping, aggregation, and sorting.
- Understand inner joins, left joins, and group joins.
- Create inventory summaries and order reports.
- Practice nested queries and set operations.

## 🛠️ Technologies Used

- **C#**
- **.NET Console Application**
- **LINQ (Language Integrated Query)**
- **System.Linq**
- In-memory collections using `List<T>`

## 📚 Project Exercises

The project contains 20 LINQ practice exercises:

| No. | Exercise | Concepts Covered |
|---|---|---|
| 1 | Filtering | `Where()` and conditional expressions |
| 2 | Projection | `Select()` and anonymous types |
| 3 | SelectMany | Flattening nested collections |
| 4 | Sorting | `OrderBy()` and `OrderByDescending()` |
| 5 | Element Operators | `FirstOrDefault()` and `Single()` |
| 6 | Aggregation | `Count()`, `Sum()`, `Min()`, `Max()`, `Average()` |
| 7 | Any and All | Checking collection conditions |
| 8 | Grouping | `GroupBy()` and grouped summaries |
| 9 | Set Operations | `Distinct()`, `Union()`, `Intersect()`, `Except()` |
| 10 | Inner Join | Matching employees with departments |
| 11 | Left Join | Including employees without a matching department |
| 12 | Group Join | Counting employees in each department |
| 13 | Partitioning | `Take()` and `Skip()` |
| 14 | Quantifiers and Membership | `Contains()` and ID matching |
| 15 | String and Numeric Queries | `StartsWith()` and modulo operations |
| 16 | Nested Queries | Average-price filtering and customer order checks |
| 17 | Combined Queries | Filtering, sorting, and projection together |
| 18 | Dictionary and Lookup | `ToDictionary()` and `ToLookup()` |
| 19 | Orders and Customers Join | Combining order and customer details |
| 20 | Reports | Order-status summaries and inventory reports |

## 🗂️ Project Structure

The project is organized using separate namespaces for data and models.

```text
LinqMastery/
├── Data/
│   └── StoreData.cs
├── Models/
│   ├── Product.cs
│   ├── Customer.cs
│   ├── Order.cs
│   ├── Department.cs
│   └── Employee.cs
├── Program.cs
└── LinqMastery.csproj
```

*Note: This is an illustrative structure based on the namespaces and types referenced in the provided code. Your actual project may contain additional files.*

## 🧾 Sample Data

The `StoreData` class provides in-memory data for LINQ exercises.

### Products
Includes product names, categories, prices, stock quantities, supplier IDs, and tags.

Examples:
- Laptop
- Mouse
- Keyboard
- Office Chair
- Monitor
- Notebook
- Pen

### Customers
Contains customer IDs, names, cities, and email addresses.

### Orders
Contains order IDs, customer IDs, order dates, order statuses, discounts, and order items.

The sample data includes delivered, pending, and cancelled orders. It also intentionally includes an order with unmatched customer and product IDs for practicing data-handling scenarios.

### Employees and Departments
Used to practice inner joins, left joins, and group joins. One employee has a department ID that does not match any department, allowing left-join behavior to be explored.

### Additional Collections
- `ListA` and `ListB` for set operations.
- `TrainingBatches` for flattening nested collections using `SelectMany()`.

## 🔍 LINQ Syntax Examples

### 1. Method Syntax

```csharp
var expensiveProducts = StoreData.Products
    .Where(p => p.Price > 5000)
    .OrderByDescending(p => p.Price)
    .Select(p => p.ProductName);

foreach (var product in expensiveProducts)
{
    Console.WriteLine(product);
}
```

### 2. Query Syntax

```csharp
var expensiveProducts =
    from p in StoreData.Products
    where p.Price > 5000
    orderby p.Price descending
    select p.ProductName;

foreach (var product in expensiveProducts)
{
    Console.WriteLine(product);
}
```

Both examples retrieve product names for products priced above 5,000, ordered from highest to lowest price.

## ▶️ How to Run the Project

1. Install the .NET SDK compatible with your project.
2. Open the project in Visual Studio or Visual Studio Code.
3. Ensure that the `LinqMastery.Models` classes and `StoreData.cs` are included in the project.
4. Open `Program.cs`.
5. In the `Main()` method, uncomment the exercise you want to run.
6. Keep other exercise calls commented out if you want to run one exercise at a time.
7. Run the application from the project directory:

```bash
dotnet run
```

For example, to run the grouping exercise, enable:

```csharp
Exercise08_Grouping();
```

The application will print the corresponding results to the console.

## 🧠 Key Learning Outcomes

After completing the exercises, you should be able to:

- Write LINQ queries using both supported styles.
- Filter and project collections efficiently.
- Calculate totals, averages, minimums, and maximums.
- Group records and generate summaries.
- Combine related collections using joins.
- Handle unmatched records with left joins.
- Work with nested collections and membership checks.
- Build basic reports from in-memory business data.

## ⚠️ Notes

- The project uses in-memory sample data; it does not require a database.
- Most exercise methods are commented out in `Main()`, so uncomment the desired method before running it.
- The sample data includes intentionally unmatched IDs for practicing joins and data relationships.
- Some exercises may need small corrections or additional tests as you continue practicing. For example, use `Any(p => p.Price > 50000)` to check whether any product exceeds 50,000, and use `All(p => p.Stock >= 0)` to check whether every product has non-negative stock.
- `LeftJoin()` requires a compatible .NET version; the code comments identify .NET 10 as the target for that API. The query-syntax alternative uses `GroupJoin()` with `DefaultIfEmpty()`.

## 🚀 Future Enhancements

- Add more LINQ exercises and sample datasets.
- Include expected output for each exercise.
- Add unit tests to validate query results.
- Introduce Entity Framework Core and a database-backed version.
- Add exception handling and input-driven query examples.
- Create a menu-driven console interface for selecting exercises.

## 👩‍💻 Author

**Divya**

## 📄 License

This project is intended for learning and educational practice. Add a license file if you plan to distribute it publicly.

using LinqMastery.Data;
using LinqMastery.Models;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace LinqMastery;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("LINQ Mastery Practice Project");
        Console.WriteLine("Uncomment one exercise method at a time in Main().");

        //Exercise01_Filtering();
        //Exercise02_Projection();
        //Exercise03_SelectMany();
        //Exercise04_Sorting();
        //Exercise05_ElementOperators();
        //Exercise06_Aggregation();
        //Exercise07_AnyAll();
        //Exercise08_Grouping();
        // Exercise09_SetOperations();
        // Exercise10_InnerJoin();
        //Exercise11_LeftJoin();
        //Exercise12_GroupJoin();
        //Exercise13_Partitioning();
        //Exercise14_QuantifiersAndMembership();
        //Exercise15_StringAndNumericQueries();
        //Exercise16_NestedQueries();
        //Exercise17_CombinedQueries();
        //Exercise18_DictionaryAndLookup();
        //Exercise19_JoinOrdersAndCustomers();
        //Exercise20_Reports();
    }

    // RULE: For every task, implement both styles where supported.
    // Use these labels to keep your work organised:
    // METHOD SYNTAX PRACTICE
    // QUERY SYNTAX PRACTICE

    static void Exercise01_Filtering()
    {
        //var products = StoreData.Products;

        // Task 1: Find products priced above 5,000.
        // METHOD SYNTAX PRACTICE
        // TODO: products.Where(...)

        //var expensiveMethod = products.Where(p => p.Price > 5000);

        // QUERY SYNTAX PRACTICE
        // TODO: from p in products where ... select p

        //var expensiveQuery = from p in products
        //                     where p.Price > 5000
        //                     select p;

        // Task 2: Find Electronics products that are in stock.
        // METHOD SYNTAX PRACTICE
        // TODO: Where with &&

        //var stockMethed = products.Where(p => p.Category == "Electronics" && p.Stock > 0);

        // QUERY SYNTAX PRACTICE
        // TODO: where with &&

        //var stockQuery = from p in products
        //                  where p.Category == "Electronics" && p.Stock > 0
        //                  select p;

        // Task 3: Find products priced between 1,000 and 20,000.
        // METHOD SYNTAX PRACTICE
        // TODO

        //var priceRangeMehtod = products.Where(p => p.Price > 1000 && p.Price < 20000);

        // QUERY SYNTAX PRACTICE
        // TODO

        //var priceRangeQuery   = from p in products 
        //                        where p.Price > 1000 && p.Price < 20000
        //                        select p;

        // Task 4: Find products whose names contain "Office".
        // METHOD SYNTAX PRACTICE
        // TODO: Contains()

        //var officeProductMethod = products.Where(p => p.ProductName.Contains("Office"));

        // QUERY SYNTAX PRACTICE
        // TODO

        //    var officeProductQuery = from p in products
        //                           where p.ProductName.Contains("Office")
        //                           select p;


        //    Console.WriteLine(new string('-' ,30));
        //    Console.WriteLine("Task1 Method Syntax");
        //    foreach(var p in expensiveMethod)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }
        //    Console.WriteLine("Task1 Query Syntax");
        //    foreach (var p in expensiveQuery)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }


        //    Console.WriteLine(new string('-', 30));
        //    Console.WriteLine("Task2 Method Syntax");
        //    foreach (var p in stockMethed)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Stock : {p.Stock}");

        //    }
        //    Console.WriteLine("Task2 Query Syntax");
        //    foreach (var p in stockQuery)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Stock : {p.Stock}");

        //    }

        //    Console.WriteLine(new string('-', 30));
        //    Console.WriteLine("Task3 Method Syntax");
        //    foreach (var p in priceRangeMehtod)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }
        //    Console.WriteLine("Task3 Query Syntax");
        //    foreach (var p in priceRangeQuery)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }

        //    Console.WriteLine(new string('-', 30));
        //    Console.WriteLine("Task4 Method Syntax");
        //    foreach (var p in officeProductMethod)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }
        //    Console.WriteLine("Task4 Query Syntax");
        //    foreach (var p in officeProductQuery)
        //    {
        //        Console.WriteLine($"Product Name : {p.ProductName} Price : {p.Price}");

        //    }

        }

        static void Exercise02_Projection()
        {
            var products = StoreData.Products;

        // Task 1: Select only product names.
        // METHOD SYNTAX PRACTICE
        // TODO: Select(...)

         var productNameMethod = products.Select(p => p.ProductName);

        // QUERY SYNTAX PRACTICE
        // TODO: select p.ProductName

        var productNameQuery = from p in products
                               select p.ProductName;

        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Exercise02_Projection");
        //Console.WriteLine("Task1 Method");
        //foreach(var p in productNameMethod)
        //{
        //    Console.WriteLine($"Product Name : {p}");
        //}
        //Console.WriteLine("Task2 Query");
        //foreach(var p in productNameQuery)
        //{
        //    Console.WriteLine($"Product Name : {p}");
        //}



        // Task 2: Return ProductName and Price using an anonymous type.
        // METHOD SYNTAX PRACTICE
        // TODO: Select(p => new { ... })

        var anonymousMethod = products.Select(p => new { p.ProductName, p.Price });

            // QUERY SYNTAX PRACTICE
            // TODO: select new { ... }

            var anonymousQuery = from p in products
                                 select new { p.ProductName, p.Price };

        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Exercise02_Projection");
        //Console.WriteLine("Task2 Method");
        //foreach (var p in anonymousMethod)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName} \n Price :  {p.Price}");
        //}
        //Console.WriteLine("Task2 Query");
        //foreach (var p in anonymousQuery)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName} \n Price : {p.Price}");
        //}



        // Task 3: Return ProductName, Price, and PriceWithTax (18%).
        // METHOD SYNTAX PRACTICE
        // TODO

        var productDetailsMethod = products.Select(p => new
            {
                p.ProductName,
                p.Price,
                PriceWithTax = p.Price * 1.18m
            });

            // QUERY SYNTAX PRACTICE
            // TODO

            var productDetailsQuery = from p in products
                                      select new
                                      {
                                          p.ProductName,
                                          p.Price,
                                          PriceWithTax = p.Price * 1.18m
                                      };


        //Console.WriteLine("Exercise02_Projection");
        //Console.WriteLine("Task3 Method");
        //foreach (var p in productDetailsMethod)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName} \nPrice :  {p.Price} \nPrice with Tax : {p.PriceWithTax}");
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task3 Query");
        //foreach (var p in productDetailsQuery)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName} \nPrice : {p.Price} \nPrice with Tax : {p.PriceWithTax}");
        //}
    }

    static void Exercise03_SelectMany() 
        {
            var batches = StoreData.TrainingBatches;
            var products = StoreData.Products;

        // Task 1: Flatten all training batches into one sequence.
        // METHOD SYNTAX PRACTICE
        // TODO: SelectMany()
        var flattenMethod = batches.SelectMany(batch => batch);


        // QUERY SYNTAX PRACTICE
        // TODO: use two from clauses

        var flattenQuery = from batch in batches
                           from item in batch
                           select item;

        //Console.WriteLine("Exercise03_SelectMany");
        //Console.WriteLine("Task1 Method");
        //foreach (var p in flattenMethod)
        //{
        //    Console.WriteLine(p);
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task1 Query");
        //foreach (var p in flattenQuery)
        //{
        //    Console.WriteLine(p);
        //}



        // Task 2: Get every product tag as one flat sequence.
        // METHOD SYNTAX PRACTICE
        // TODO
        var flattenProductTagMethod = products.SelectMany(p => p.Tags);

        // QUERY SYNTAX PRACTICE
        // TODO

        var flattenProductTagQuery = from p in products
                                     from tag in p.Tags
                                     select tag;

        //Console.WriteLine("Exercise03_SelectMany");
        //Console.WriteLine("Task2 Method");
        //foreach (var p in flattenProductTagMethod)
        //{
        //    Console.WriteLine(p);
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //foreach (var p in flattenProductTagQuery)
        //{
        //    Console.WriteLine(p);
        //}


    }

    static void Exercise04_Sorting()
        {
            var products = StoreData.Products;

        // Task 1: Sort by price ascending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderBy()
        var priceAscOrderMethod = products.OrderBy(p => p.Price);

        // QUERY SYNTAX PRACTICE
        // TODO: orderby

        var priceAscOrderQuery = from p in products
                                 orderby p.Price
                                 select p;

        //Console.WriteLine("Exercise04_Sorting");
        //Console.WriteLine("Task1 Method");
        //foreach (var p in priceAscOrderMethod)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\n Price : {p.Price}");
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //foreach (var p in priceAscOrderQuery)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\n Price : {p.Price}");

       // }


        // Task 2: Sort by price descending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderByDescending()

        var priceDscOrderMethod = products.OrderByDescending(p => p.Price);
        // QUERY SYNTAX PRACTICE
        // TODO: orderby ... descending


        var priceDscOrderQuery = from p in products
                                 orderby p.Price descending
                                 select p;

        //Console.WriteLine("Exercise04_Sorting");
        //Console.WriteLine("Task2 Method");
        //foreach (var p in priceDscOrderMethod)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\n Price : {p.Price}");
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //foreach (var p in priceDscOrderQuery)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\n Price : {p.Price}");

        //}

        // Task 3: Sort by Category ascending, then Price descending.
        // METHOD SYNTAX PRACTICE
        // TODO: OrderBy().ThenByDescending()
        var categoryAscMethod = products.OrderBy(p => p.Category)
            .ThenByDescending(p => p.Price);

        // QUERY SYNTAX PRACTICE
        var categoryAscQuery = from p in products
                               orderby p.Category ascending
                               orderby p.Price descending
                               select p;

        //Console.WriteLine("Exercise04_Sorting");
        //Console.WriteLine("Task3 Method");
        //foreach (var p in categoryAscMethod)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\nCategory : {p.Category}\nPrice : {p.Price}");
        //}
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task3 Query");
        //foreach (var p in categoryAscQuery)
        //{
        //    Console.WriteLine($"Product Name : {p.ProductName}\nCategory : {p.Category}\nPrice : {p.Price}");

        //}


    }

    static void Exercise05_ElementOperators()
    {
        var products = StoreData.Products;

        // Task 1: Find the first product priced above 10,000.
        // METHOD SYNTAX PRACTICE
        // TODO: First()
        var highPriceProductMethod = products.FirstOrDefault(p => p.Price > 10000);

        // QUERY SYNTAX PRACTICE
        // TODO: query + First()

        var highPriceProductQuery = from p in products
                                    where p.Price > 10000
                                    select p;
        var highPriceFirstProductQuery = highPriceProductQuery.FirstOrDefault();


        //Console.WriteLine("Exercise05_ElementOperators");
        //Console.WriteLine("Task1 Method");
        //Console.WriteLine($"Product Name : {highPriceProductMethod?.ProductName ?? "Not found"}\nPrice : {highPriceProductMethod?.Price}");
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task1 Query");
        //Console.WriteLine($"Product Name : {highPriceFirstProductQuery?.ProductName ?? "Not found"}\nPrice : {highPriceFirstProductQuery?.Price}");
        
        // Task 2: Search for ProductId 999 safely.
        // METHOD SYNTAX PRACTICE
        // TODO: FirstOrDefault()
        var searchProductIdMethod = products.FirstOrDefault(p => p.ProductId == 999);

        // QUERY SYNTAX PRACTICE
        // TODO: query + FirstOrDefault()
        var searchProductIdQuery = from product in products
                                   where product.ProductId == 999
                                   select product;
        var searchProductIdQueryRes = searchProductIdQuery.FirstOrDefault();

        //Console.WriteLine("Exercise05_Sorting");
        //Console.WriteLine("Task2 Method");
        //Console.WriteLine($"Product Name : {searchProductIdMethod?.ProductName ?? "Not found"}\nProduct Id : {searchProductIdMethod?.ProductId}");
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //Console.WriteLine($"Product Name : {searchProductIdQueryRes?.ProductName ?? "Not found"}\nProduct Id : {searchProductIdQueryRes?.ProductId}");


        // Task 3: Find ProductId 3 using Single().
        // METHOD SYNTAX PRACTICE
        // TODO: Single()
        var findProductIdMethod = products.Single(p => p.ProductId == 3);

        // QUERY SYNTAX PRACTICE
        // TODO: query + Single()
        var findProductIdQuery = from product in products
                                 where product.ProductId == 3
                                 select product;
        var findProductIdQueryRes = findProductIdQuery.FirstOrDefault();

        //Console.WriteLine("Exercise05_Sorting");
        //Console.WriteLine("Task3 Method");
        //Console.WriteLine($"Product Name : {searchProductIdMethod?.ProductName ?? "Not found"}\nProduct Id : {searchProductIdMethod?.ProductId}");
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task3 Query");
        //Console.WriteLine($"Product Name : {searchProductIdQueryRes?.ProductName ?? "Not found"}\nProduct Id : {searchProductIdQueryRes?.ProductId}");


    }

    static void Exercise06_Aggregation()
        {
            var products = StoreData.Products;

            // Task 1: Count all products.
            // METHOD SYNTAX PRACTICE
            // TODO: Count()
            var productCountMethod = products.Count();

        // QUERY SYNTAX PRACTICE
        // TODO: query + Count()
        var productCountQuery = from p in products select p.ProductName;
        var productCountQueryRes = productCountQuery.Count();

        //Console.WriteLine("Exercise06_Aggregation");
        //Console.WriteLine("Task1 Method");
        //Console.WriteLine("Product Count : "+productCountMethod);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task1 Query");
        //Console.WriteLine("Product Count : " + productCountQueryRes);

        // Task 2: Calculate total stock.
        // METHOD SYNTAX PRACTICE
        // TODO: Sum()

        var totalStockMethod = products.Sum(p => p.Stock);

        // QUERY SYNTAX PRACTICE
        // TODO: query + Sum()
        var totalStockQuery = (from p in products
                              select p.Stock).Sum();
        //var totalStockQueryRes = totalStockQuery.Sum(p => p.Stock);

        //Console.WriteLine("Exercise06_Aggregation");
        //Console.WriteLine("Task2 Method");
        //Console.WriteLine("Stock Sum : " + totalStockMethod);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //Console.WriteLine("Stock Sum : " + totalStockQuery);

        // Task 3: Find average, minimum and maximum price.
        // METHOD SYNTAX PRACTICE
        // TODO

        var minPriceMethod = products.Min(p => p.Price);
        var maxPriceMethod = products.Max(p => p.Price);
        var avgPriceMethod = products.Average(p => p.Price);


        // QUERY SYNTAX PRACTICE
        // TODO
        var minPriceQuery = (from p in products
                            select p.Price).Min();
        var maxPriceQuery = (from p in products
                             select p.Price).Max();
        var avgPriceQuery = (from p in products
                             select p.Price).Average();

        //Console.WriteLine("Exercise06_Aggregation");
        //Console.WriteLine("Task3 Method");
        //Console.WriteLine("Minimum : " + minPriceMethod);
        //Console.WriteLine("Maximum : " + maxPriceMethod);
        //Console.WriteLine("Average : " + avgPriceMethod);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task3 Query");
        //Console.WriteLine("Minimum : " + minPriceQuery);
        //Console.WriteLine("Maximum : " + maxPriceQuery);
        //Console.WriteLine("Average : " + avgPriceQuery);

        // Task 4: Calculate total inventory value (Price * Stock).
        // METHOD SYNTAX PRACTICE
        // TODO
        var totalInventoryMethod = products.Sum(p => p.Price * p.Stock);

        // QUERY SYNTAX PRACTICE
        // TODO
        var totalInventoryQuery = (from p in products
                                  select p.Price*p.Stock).Sum();

        //Console.WriteLine("Exercise06_Aggregation");
        //Console.WriteLine("Task3 Method");
        //Console.WriteLine("Total Sum : " + totalInventoryMethod);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task3 Query");
        //Console.WriteLine("Total Sum : " + totalInventoryQuery);


    }

    static void Exercise07_AnyAll()
        {
            var products = StoreData.Products;

        // Task 1: Check whether any product costs more than 50,000.
        // METHOD SYNTAX PRACTICE
        // TODO: Any()
        var moreCostMethed = products.Any(p => p.Price > 50000);

        // QUERY SYNTAX PRACTICE
        // TODO: query + Any()
        var moreCostQuery = (from Product in products
                             select Product.Price).Any();

        //Console.WriteLine("Exercise07_AnyAll");
        //Console.WriteLine("Task1 Method");
        //Console.WriteLine("High Cost : " + moreCostMethed);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task1 Query");
        //Console.WriteLine("High Cost : " + moreCostQuery);

        // Task 2: Check whether all products have non-negative stock.
        // METHOD SYNTAX PRACTICE
        // TODO: All()
        var isNonNegativeStockMethod = products.All(p => p.Stock > 0);

        // QUERY SYNTAX PRACTICE
        // TODO: query + All()
        var isNonNegativeStockQuery = (from product in products
                                       select product).All(product => product.Stock > 0);

        //Console.WriteLine("Exercise07_AnyAll");
        //Console.WriteLine("Task2 Method");
        //Console.WriteLine("Non Negative Stock : " + isNonNegativeStockMethod);
        //Console.WriteLine(new string('-', 30));
        //Console.WriteLine("Task2 Query");
        //Console.WriteLine("Non Negative Stock : " + isNonNegativeStockQuery);


    }

    static void Exercise08_Grouping()
        {
            var products = StoreData.Products;

        // Task 1: Group products by Category.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy()

        var groupProductsByCategoryMethod = products.GroupBy(p => p.Category);

        // QUERY SYNTAX PRACTICE
        // TODO: group ... by ...

        var groupProductsByCategoryQuery = from product in products
                                           group product by product.Category;

        Console.WriteLine("Exercise08_Grouping");
        Console.WriteLine("Task1 Method");
        foreach(var group in groupProductsByCategoryMethod) {
            Console.WriteLine($"Category: {group.Key}");
            foreach(var product in group)
            {
                Console.WriteLine($"{product.ProductName}");
            }
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task1 Query");
        foreach(var group in groupProductsByCategoryQuery)
        {
            Console.WriteLine($"Category: {group.Key}");
            foreach(var product in group)
            {
                Console.WriteLine($"{product.ProductName}");
            }
        }


        // Task 2: For each category, calculate product count,
        // total stock, and average price.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy + Select

        var productCategoryMethod = products.GroupBy(p => p.Category).
            Select(p => new
            {
                Category = p.Key,
                ProductCount = p.Count(),
                TotalStock = p.Sum(p => p.Stock),
                AvgPrice = p.Average(p => p.Price)
            });

        // QUERY SYNTAX PRACTICE
        // TODO: group ... by ... into ...
        var productCategoryQuery =from product in products
                                  group product by product.Category into g
                                  select new {
                                  Category = g.Key,
                                  ProductCount = g.Count(),
                                  TotalStock = g.Sum(p => p.Stock),
                                  AvgPrice = g.Average(p => p.Price)
                                  };

        Console.WriteLine("Exercise08_Grouping");
        Console.WriteLine("Task1 Method");
        foreach (var group in productCategoryMethod)
        {
            Console.WriteLine($"Category : {group.Category}\nProduct Count : {group.ProductCount}\nTotal Stock : {group.TotalStock}\nAverage Price : {group.AvgPrice}");
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task1 Query");
        foreach (var group in productCategoryQuery)
        {
            Console.WriteLine($"Category : {group.Category}\nProduct Count : {group.ProductCount}\nTotal Stock : {group.TotalStock}\nAverage Price : {group.AvgPrice}");
        }
    }

    static void Exercise09_SetOperations()
        {
            var a = StoreData.ListA;
            var b = StoreData.ListB;

        // Task 1: Remove duplicates.
        // METHOD SYNTAX PRACTICE
        // TODO: Distinct()
        var setOperationMethod1a = a.Distinct().ToList();
        var setOperationMethod1b = b.Distinct().ToList();


        // QUERY SYNTAX PRACTICE
        // TODO: query + Distinct()
        var setOperationQuery1a = (from num in a select num).Distinct().ToList();
        var setOperationQuery1b = (from num in b select num).Distinct().ToList();

        Console.WriteLine("Exercise09_SetOperations");
        Console.WriteLine("Task1 Method");
        Console.WriteLine("Distinct Values by Method: ");
        Console.WriteLine("In List a");
        foreach (var l in setOperationMethod1a)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("In List b");
        foreach (var l in setOperationMethod1b)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task1 Query");
        Console.WriteLine("Distinct Values by Query: ");
        Console.WriteLine("In List a");
        foreach (var l in setOperationQuery1a)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("In List b");
        foreach (var l in setOperationQuery1b)
        {
            Console.WriteLine(l);
        }



        // Task 2: Find union, intersection and A-only values.
        // METHOD SYNTAX PRACTICE
        // TODO: Union(), Intersect(), Except()

        var unionMethod = a.Union(b);
        var intersectionMethod = a.Intersect(b);
        var exceptMethod = a.Except(b);

        // QUERY SYNTAX PRACTICE
        // TODO: query results + set operators
        var unionQuery = (from num in a select num).Union(from num in b select num);
        var intersectionQuery = (from num in a select num).Intersect(from num in b select num);
        var exceptQuery = (from num in a select num).Except(from num in b select num);

        Console.WriteLine("Exercise09_SetOperations");
        Console.WriteLine("Task2 Method");
        Console.WriteLine("Union by Method: ");
        foreach (var l in unionMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("Intersection by Method: ");
        foreach (var l in intersectionMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("Exception by Method: ");
        foreach (var l in exceptMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task2 Query");
        Console.WriteLine("Union by Query: ");
        foreach (var l in unionQuery)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("Intersection by Query: ");
        foreach (var l in intersectionQuery)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine("Exception by Query: ");
        foreach (var l in exceptQuery)
        {
            Console.WriteLine(l);
        }

    }

    static void Exercise10_InnerJoin()
        {
            var employees = StoreData.Employees;
            var departments = StoreData.Departments;

        // Task: Display EmployeeName and DepartmentName for matching records.
        // METHOD SYNTAX PRACTICE
        // TODO: Join()
        var matchingRecordMethod = employees.Join(departments,
            emp => emp.DepartmentId,
            dept => dept.DepartmentId,
            (emp, dept) => new { EmployeeName = emp.EmployeeName, DapartmentName = dept.DepartmentName }
            );

        // QUERY SYNTAX PRACTICE
        // TODO: join ... on ... equals ...
        var matchingRecordQuery = from emp in employees
                                  join dept in departments
                                  on emp.DepartmentId equals dept.DepartmentId
                                  select new { EmployeeName = emp.EmployeeName, DepartmentName = dept.DepartmentName };

        Console.WriteLine("Exercise10_InnerJoin");
        Console.WriteLine("Task Method");
        foreach (var l in matchingRecordMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine(new string ('-', 30));
        Console.WriteLine("Task Query");
        foreach (var l in matchingRecordQuery)
        {
            Console.WriteLine(l);
        }
    }

        static void Exercise11_LeftJoin()
        {
            var employees = StoreData.Employees;
            var departments = StoreData.Departments;

        // Task: Return every employee, displaying "Unassigned" if
        // no matching department exists.
        // METHOD SYNTAX PRACTICE
        // TODO: LeftJoin() on .NET 10, or GroupJoin + SelectMany
        var leftJoinMethod = employees.LeftJoin(departments,
            emp => emp.DepartmentId,
            dept => dept.DepartmentId,
            (emp, dept) => new
            {
                EmployeeName = emp.EmployeeName,
                DepartmentName = dept != null ? dept.DepartmentName : "Unassigned"
            });

        var leftJoinQuery = from emp in employees
                            join dept in departments
                            on emp.DepartmentId equals dept.DepartmentId into employeeDepts
                            from subDept in employeeDepts.DefaultIfEmpty()
                            select new
                            {
                                EmployeeName = emp.EmployeeName,
                                DepartmentName = subDept != null ? subDept.DepartmentName : "Unassigned"
                            };

        Console.WriteLine("Exercise11_LeftJoin");
        Console.WriteLine("Task Method");
        foreach (var l in leftJoinMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task Query");
        foreach (var l in leftJoinQuery)
        {
            Console.WriteLine(l);
        }

        // QUERY SYNTAX PRACTICE
        // TODO: join ... into ... then DefaultIfEmpty()
    }

        static void Exercise12_GroupJoin()
        {
            var employees = StoreData.Employees;
            var departments = StoreData.Departments;

        // Task: Return each department with its employee count.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupJoin()
                var groupJoinMethod = departments.GroupJoin(
            employees,
            dept => dept.DepartmentId,
            emp => emp.DepartmentId,
            (dept, empGroup) => new { DepartmentName = dept.DepartmentName, EmployeeCount = empGroup.Count() }
        );
        // QUERY SYNTAX PRACTICE
        // TODO: join ... into ...
        var groupJoinQuery = from dept in departments
                             join emp in employees
                             on dept.DepartmentId equals emp.DepartmentId into empGroup
                             select new { DepartmentName = dept.DepartmentName, EmployeeCount = empGroup.Count() };

        Console.WriteLine("Exercise12_GroupJoin");
        Console.WriteLine("Task Method");
        foreach (var l in groupJoinMethod)
        {
            Console.WriteLine(l);
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task Query");
        foreach (var l in groupJoinQuery)
        {
            Console.WriteLine(l);
        }
    }

        static void Exercise13_Partitioning()
        {
            var products = StoreData.Products.OrderBy(p => p.ProductId);

        // Task 1: Get first three products.
        // METHOD SYNTAX PRACTICE
        // TODO: Take(3)
        var takeMethod = products.Take(3);
        var takeQuery = (from p in products select p).Take(3);
        // QUERY SYNTAX PRACTICE
        // TODO: query + Take(3)
        var skipMethod = products.Skip(3);
        var skipQuery = (from p in products select p).Skip(3);
        // Task 2: Skip first three products.
        // METHOD SYNTAX PRACTICE
        // TODO: Skip(3)

        // QUERY SYNTAX PRACTICE
        // TODO: query + Skip(3)
        Console.WriteLine("Exercise13_Partitioning");
        Console.WriteLine("Task Method");
        Console.WriteLine("Take Method");
        foreach (var l in takeMethod)
        {
            Console.WriteLine(l.ProductName);
        }
        Console.WriteLine("Skip Method");
        foreach (var l in skipMethod)
        {
            Console.WriteLine(l.ProductName);
        }
        Console.WriteLine(new string('-', 30));
        Console.WriteLine("Task Query");
        Console.WriteLine("Take Method");
        foreach (var l in takeQuery)
        {
            Console.WriteLine(l.ProductName);
        }
        Console.WriteLine("Skip Method");
        foreach (var l in skipQuery)
        {
            Console.WriteLine(l.ProductName);
        }
    }

        static void Exercise14_QuantifiersAndMembership()
        {
        var products = StoreData.Products;
        List<int> requestedIds = new() { 1, 3, 5 };

        // Task: Find products whose IDs occur in requestedIds.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + Contains
        var membershipMethod = products.Where(p => requestedIds.Contains(p.ProductId));

        var membershipQuery = from p in products
                              where requestedIds.Contains(p.ProductId)
                              select p;
        // QUERY SYNTAX PRACTICE
        // TODO: where + Contains
        Console.WriteLine("Exercise14_QuantifiersAndMembership");
        Console.WriteLine("--- Method Syntax ---");
        foreach (var p in membershipMethod)
        {
            Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}");
        }
        Console.WriteLine("--- Query Syntax ---");
        foreach (var p in membershipQuery)
        {
            Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}");
        }
        Console.WriteLine(new string('=', 30));
    }

        static void Exercise15_StringAndNumericQueries()
        {
            var products = StoreData.Products;

        // Task 1: Find names starting with "O".
        // METHOD SYNTAX PRACTICE
        // TODO: StartsWith()
        var stringMethod = products.Where(p => p.ProductName.StartsWith("O"));
        var stringQuery = from p in products
                          where p.ProductName.StartsWith("O")
                          select p;


        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 2: Find products with even ProductIds.
        // METHOD SYNTAX PRACTICE
        // TODO: modulo operator %
        var numericMethod = products.Where(p => p.ProductId % 2 == 0);
        var numericQuery = from p in products
                           where p.ProductId % 2 == 0
                           select p;
        // QUERY SYNTAX PRACTICE
        // TODO
        Console.WriteLine("Exercise15_StringAndNumericQueries");
        Console.WriteLine("--- Method Syntax (StartsWith 'O') ---");
        foreach (var p in stringMethod) Console.WriteLine(p.ProductName);
        Console.WriteLine("--- Query Syntax (StartsWith 'O') ---");
        foreach (var p in stringQuery) Console.WriteLine(p.ProductName);

        Console.WriteLine("--- Method Syntax (Even ProductIds) ---");
        foreach (var p in numericMethod) Console.WriteLine($"ID: {p.ProductId} - {p.ProductName}");
        Console.WriteLine("--- Query Syntax (Even ProductIds) ---");
        foreach (var p in numericQuery) Console.WriteLine($"ID: {p.ProductId} - {p.ProductName}");
        Console.WriteLine(new string('=', 30));
    }

        static void Exercise16_NestedQueries()
        {
            var products = StoreData.Products;
            var orders = StoreData.Orders;
        var customers = StoreData.Customers; 

        // Task 1: Find products priced above average.
        // METHOD SYNTAX PRACTICE
        // TODO: Average + Where
        var averagePrice = products.Average(p => p.Price);

        var nestedMethod1 = products.Where(p => p.Price > averagePrice);
        var nestedQuery1 = from p in products
                           where p.Price > (from prod in products select prod.Price).Average()
                           select p;

        // QUERY SYNTAX PRACTICE
        // TODO

        // Task 2: Find customers with at least one order.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + Any
        var nestedMethod2 = customers.Where(c => orders.Any(o => o.CustomerId == c.CustomerId));
        var nestedQuery2 = from c in customers
                           where (from o in orders where o.CustomerId == c.CustomerId select o).Any()
                           select c;
        // QUERY SYNTAX PRACTICE
        // TODO
        Console.WriteLine("Exercise16_NestedQueries");
        Console.WriteLine($"Average Price Reference: {averagePrice:C}");
        Console.WriteLine("--- Method Syntax (Above Average Price) ---");
        foreach (var p in nestedMethod1) Console.WriteLine($"{p.ProductName} (${p.Price})");
        Console.WriteLine("--- Query Syntax (Above Average Price) ---");
        foreach (var p in nestedQuery1) Console.WriteLine($"{p.ProductName} (${p.Price})");

        Console.WriteLine("--- Method Syntax (Active Customers) ---");
        foreach (var c in nestedMethod2) Console.WriteLine(c.CustomerName);
        Console.WriteLine("--- Query Syntax (Active Customers) ---");
        foreach (var c in nestedQuery2) Console.WriteLine(c.CustomerName);
        Console.WriteLine(new string('=', 30));

    }

    static void Exercise17_CombinedQueries()
        {
            var products = StoreData.Products;

        // Task: Find in-stock Electronics products, sort by descending
        // price, and return only product names.
        // METHOD SYNTAX PRACTICE
        // TODO: Where + OrderByDescending + Select
            var combinedMethod = products
       .Where(p => p.Category == "Electronics" && p.Stock > 0)
       .OrderByDescending(p => p.Price)
       .Select(p => p.ProductName);
        // QUERY SYNTAX PRACTICE
        // TODO: from + where + orderby + select
        var combinedQuery = from p in products
                            where p.Category == "Electronics" && p.Stock > 0
                            orderby p.Price descending
                            select p.ProductName;

        Console.WriteLine("Exercise17_CombinedQueries");
        Console.WriteLine("--- Method Syntax ---");
        foreach (var name in combinedMethod) Console.WriteLine(name);
        Console.WriteLine("--- Query Syntax ---");
        foreach (var name in combinedQuery) Console.WriteLine(name);
        Console.WriteLine(new string('=', 30));
    }

        static void Exercise18_DictionaryAndLookup()
        {
            var products = StoreData.Products;

        // Task 1: Create a dictionary keyed by ProductId.
        // METHOD SYNTAX PRACTICE
        // TODO: ToDictionary()
        var dictMethod = products.ToDictionary(p => p.ProductId);
        var dictQuery = (from p in products select p).ToDictionary(p => p.ProductId);
        // QUERY SYNTAX PRACTICE
        // TODO: query + ToDictionary()

        // Task 2: Create a lookup by Category.
        // METHOD SYNTAX PRACTICE
        // TODO: ToLookup()
        var lookupMethod = products.ToLookup(p => p.Category);
        var lookupQuery = (from p in products select p).ToLookup(p => p.Category);
        // QUERY SYNTAX PRACTICE
        // TODO: query + ToLookup()
        Console.WriteLine("Exercise18_DictionaryAndLookup");
        Console.WriteLine("--- Dictionary Lookup Example (Key: 3) ---");
        if (dictMethod.TryGetValue(3, out var prod))
            Console.WriteLine($"Found Product 3: {prod.ProductName}");

        Console.WriteLine("--- Lookup Grouping Elements ---");
        foreach (var group in lookupMethod)
        {
            Console.WriteLine($"Category Group: {group.Key}");
            foreach (var p in group)
            {
                Console.WriteLine($"  -> {p.ProductName}");
            }
        }
        Console.WriteLine(new string('=', 30));
    }

        static void Exercise19_JoinOrdersAndCustomers()
        {
            var orders = StoreData.Orders;
            var customers = StoreData.Customers;

        // Task: Show OrderId, CustomerName, OrderDate and Status.
        // METHOD SYNTAX PRACTICE
        // TODO: Join + Select
        var orderJoinMethod = orders.Join(
            customers,
            o => o.CustomerId,
            c => c.CustomerId,
            (o, c) => new { o.OrderId, CustomerName = c.CustomerName, o.OrderDate, o.Status }
        );

     
        var orderJoinQuery = from o in orders
                             join c in customers on o.CustomerId equals c.CustomerId
                             select new { o.OrderId, CustomerName = c.CustomerName, o.OrderDate, o.Status };
        // QUERY SYNTAX PRACTICE
        // TODO: join + select

        Console.WriteLine("Exercise19_JoinOrdersAndCustomers");
        Console.WriteLine("--- Method Syntax ---");
        foreach (var item in orderJoinMethod)
        {
            Console.WriteLine($"Order ID: {item.OrderId} | Customer: {item.CustomerName} | Status: {item.Status}");
        }
        Console.WriteLine("--- Query Syntax ---");
        foreach (var item in orderJoinQuery)
        {
            Console.WriteLine($"Order ID: {item.OrderId} | Customer: {item.CustomerName} | Status: {item.Status}");
        }
        Console.WriteLine(new string('=', 30));
    }

        static void Exercise20_Reports()
        {
            var products = StoreData.Products;
            var orders = StoreData.Orders;

        // Task 1: Count orders by Status.
        // METHOD SYNTAX PRACTICE
        // TODO: GroupBy + Count
        var statusMethod = orders.GroupBy(o => o.Status)
                             .Select(g => new { Status = g.Key, Count = g.Count() });

        var statusQuery = from o in orders
                          group o by o.Status into g
                          select new { Status = g.Key, Count = g.Count() };


        // QUERY SYNTAX PRACTICE
        // TODO: group by + Count

        // Task 2: Create a category report with product count,
        // average price, total stock and inventory value.
        // METHOD SYNTAX PRACTICE
        // TODO
        var reportMethod = products.GroupBy(p => p.Category)
                            .Select(g => new
                            {
                                Category = g.Key,
                                ProductCount = g.Count(),
                                AveragePrice = g.Average(p => p.Price),
                                TotalStock = g.Sum(p => p.Stock),
                                InventoryValue = g.Sum(p => p.Price * p.Stock)
                            });
        // QUERY SYNTAX PRACTICE
        // TODO
        var reportQuery = from p in products
                          group p by p.Category into g
                          select new
                          {
                              Category = g.Key,
                              ProductCount = g.Count(),
                              AveragePrice = g.Average(p => p.Price),
                              TotalStock = g.Sum(p => p.Stock),
                              InventoryValue = g.Sum(p => p.Price * p.Stock)
                          };
        Console.WriteLine("Exercise20_Reports");
        Console.WriteLine("--- Orders Status Breakdown ---");
        foreach (var item in statusMethod)
        {
            Console.WriteLine($"Status: {item.Status} | Count: {item.Count}");
        }
        Console.WriteLine("--- Full Inventory Category Report ---");
        foreach (var r in reportMethod)
        {
            Console.WriteLine($"Category: {r.Category}");
            Console.WriteLine($"  - Total Products: {r.ProductCount}");
            Console.WriteLine($"  - Avg Price:      {r.AveragePrice:C}");
            Console.WriteLine($"  - Total Stock:    {r.TotalStock}");
            Console.WriteLine($"  - Total Value:    {r.InventoryValue:C}");
        }
        Console.WriteLine(new string('=', 30));
    }
    }

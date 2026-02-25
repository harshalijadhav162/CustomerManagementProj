using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CustomerManagement.Models;


namespace CustomerManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            while (true)
            {
                Console.WriteLine("\nMENU");
                Console.WriteLine("1. Insert Customer");
                Console.WriteLine("2. Insert Order");
                Console.WriteLine("3. View All Customers");
                Console.WriteLine("4. Filter Customers (Age > 25)");
                Console.WriteLine("5. Sort Customers By Name");
                Console.WriteLine("6. Join Customers & Orders");
                Console.WriteLine("7. Grouping & Aggregation");
                Console.WriteLine("8. Eager Loading");
                Console.WriteLine("9. Explicit Loading");
                Console.WriteLine("10. Lazy Loading");
                Console.WriteLine("11. Projection");
                Console.WriteLine("12. Exit");
                Console.Write("Enter choice: ");

                int choice = int.Parse(Console.ReadLine() ?? "0");

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine() ?? "";

                        Console.Write("Enter Age: ");
                        int age = int.Parse(Console.ReadLine() ?? "0");

                        context.Customers.Add(new Customer { Name = name, Age = age });
                        context.SaveChanges();
                        Console.WriteLine("Customer Inserted");
                        break;

                    case 2:
                        Console.Write("Enter CustomerId: ");
                        int custId = int.Parse(Console.ReadLine() ?? "0");

                        Console.Write("Enter Total Amount: ");
                        decimal amount = decimal.Parse(Console.ReadLine() ?? "0");

                        context.Orders.Add(new Order { CustomerId = custId, TotalAmount = amount });
                        context.SaveChanges();
                        Console.WriteLine("Order Inserted");
                        break;

                    case 3:
                        foreach (var c in context.Customers.ToList())
                            Console.WriteLine($"{c.CustomerId} - {c.Name} - {c.Age}");
                        break;

                    case 4:
                        var filtered = context.Customers.Where(c => c.Age > 25).ToList();
                        foreach (var c in filtered)
                            Console.WriteLine($"{c.CustomerId} - {c.Name} - {c.Age}");
                        break;

                    case 5:
                        var sorted = context.Customers.OrderBy(c => c.Name).ToList();
                        foreach (var c in sorted)
                            Console.WriteLine($"{c.CustomerId} - {c.Name} - {c.Age}");
                        break;

                    case 6: // JOIN
                        var joinData = from c in context.Customers
                                       join o in context.Orders
                                       on c.CustomerId equals o.CustomerId
                                       select new
                                       {
                                           c.Name,
                                           o.TotalAmount
                                       };

                        foreach (var item in joinData)
                            Console.WriteLine($"{item.Name} - {item.TotalAmount}");
                        break;

                    case 7: // GROUPING & AGGREGATION
                        var grouped = context.Orders
                            .GroupBy(o => o.CustomerId)
                            .Select(g => new
                            {
                                CustomerId = g.Key,
                                OrderCount = g.Count(),
                                TotalAmount = g.Sum(x => x.TotalAmount),
                                AverageAmount = g.Average(x => x.TotalAmount)
                            })
                            .ToList();

                        foreach (var item in grouped)
                        {
                            Console.WriteLine($"CustomerId: {item.CustomerId}");
                            Console.WriteLine($"Orders: {item.OrderCount}");
                            Console.WriteLine($"Total: {item.TotalAmount}");
                            Console.WriteLine($"Average: {item.AverageAmount}");
                            Console.WriteLine("-------------------");
                        }
                        break;

                    case 8: // EAGER LOADING
                        var eager = context.Customers
                            .Include(c => c.Orders)
                            .ToList();

                        foreach (var c in eager)
                        {
                            Console.WriteLine($"Customer: {c.Name}");
                            foreach (var o in c.Orders)
                                Console.WriteLine($"   Order: {o.TotalAmount}");
                        }
                        break;

                    case 9: // EXPLICIT LOADING
                        var customer = context.Customers.FirstOrDefault();
                        if (customer != null)
                        {
                            context.Entry(customer)
                                   .Collection(c => c.Orders)
                                   .Load();

                            Console.WriteLine($"{customer.Name} Orders Count: {customer.Orders.Count}");
                        }
                        break;

                    case 10: // LAZY LOADING
                        var lazyCustomer = context.Customers.FirstOrDefault();
                        if (lazyCustomer != null)
                        {
                            Console.WriteLine(lazyCustomer.Name);
                            foreach (var o in lazyCustomer.Orders)
                                Console.WriteLine($"Order: {o.TotalAmount}");
                        }
                        break;

                    case 11: // PROJECTION
                        var projection = context.Customers
                            .Select(c => new
                            {
                                c.Name,
                                c.Age,
                                OrderCount = c.Orders.Count()
                            })
                            .ToList();

                        foreach (var item in projection)
                            Console.WriteLine($"{item.Name} - {item.Age} - Orders: {item.OrderCount}");
                        break;

                    case 12:
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}

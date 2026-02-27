using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement
{
    class Program
    {
        static void Main()
        {
            using var db = new AppDbContext();

            while (true)
            {
                Console.WriteLine("\n=================================");
                Console.WriteLine("   CUSTOMER MANAGEMENT SYSTEM");
                Console.WriteLine("=================================");

                Console.WriteLine("1. Customer Management");
                Console.WriteLine("2. Address Management");
                Console.WriteLine("3. Contact Person Management");
                Console.WriteLine("4. Customer Interactions");
                Console.WriteLine("5. Customer Analytics");
                Console.WriteLine("6. Deleted Records");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                    continue;

                switch (choice)
                {
                    case 1:
                        CustomerMenu(db);
                        break;

                    case 2:
                        AddressMenu(db);
                        break;

                    case 3:
                        ContactMenu(db);
                        break;

                    case 4:
                        InteractionMenu(db);
                        break;

                    case 5:
                        AnalyticsMenu(db);
                        break;

                    case 6:
                        DeletedCustomers(db);
                        break;

                    case 7:
                        return;
                }
            }
        }

        // ===============================
        // CUSTOMER MANAGEMENT
        // ===============================

        static void CustomerMenu(AppDbContext db)
        {
            while (true)
            {
                Console.WriteLine("\nCustomer Management");
                Console.WriteLine("1 View All Customers");
                Console.WriteLine("2 Add Customer");
                Console.WriteLine("3 Update Customer");
                Console.WriteLine("4 Delete Customer");
                Console.WriteLine("5 Search Customer");
                Console.WriteLine("6 Return");

                Console.Write("Enter choice: ");

                int choice = Convert.ToInt32(Console.ReadLine());

                // VIEW CUSTOMERS
                if (choice == 1)
                {
                    var customers = db.Customers
                        .Where(c => !c.IsDeleted)
                        .ToList();

                    foreach (var c in customers)
                    {
                        Console.WriteLine("\n-----------------------------");
                        Console.WriteLine($"Name          : {c.CustomerName}");
                        Console.WriteLine($"Email         : {c.Email}");
                        Console.WriteLine($"Phone         : {c.Phone}");
                        Console.WriteLine($"Website       : {c.Website}");
                        Console.WriteLine($"Industry      : {c.Industry}");
                        Console.WriteLine($"Company Size  : {c.CompanySize}");
                        Console.WriteLine($"Type          : {c.Type}");
                        Console.WriteLine($"Classification: {c.Classification}");
                        Console.WriteLine($"SegmentId     : {c.SegmentId}");
                    }
                }

                // ADD CUSTOMER
                else if (choice == 2)
                {
                    Console.Write("Customer Name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("Email: ");
                    string email = Console.ReadLine() ?? "";

                    Console.Write("Phone: ");
                    string phone = Console.ReadLine() ?? "";

                    var customer = new Customer
                    {
                        CustomerName = name,
                        Email = email,
                        Phone = phone,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        AccountValue = 0,
                        HealthScore = 100
                    };

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    Console.WriteLine("Customer Added Successfully");
                }

                // UPDATE CUSTOMER (SQL)
                else if (choice == 3)
                {
                    Console.Write("Customer ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Console.Write("New Name: ");
                    string name = Console.ReadLine() ?? "";

                    Console.Write("New Email: ");
                    string email = Console.ReadLine() ?? "";

                    db.Database.ExecuteSqlRaw(
                        "UPDATE Customer SET CustomerName = {0}, Email = {1} WHERE CustomerId = {2}",
                        name, email, id);

                    Console.WriteLine("Customer Updated Successfully");
                }

                // DELETE CUSTOMER (SOFT DELETE TRIGGER)
                else if (choice == 4)
                {
                    Console.Write("Customer ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    db.Database.ExecuteSqlRaw(
                        "DELETE FROM Customer WHERE CustomerId = {0}", id);

                    Console.WriteLine("Customer Soft Deleted Successfully");
                }

                // SEARCH CUSTOMER
                else if (choice == 5)
                {
                    Console.Write("Enter Customer Name: ");
                    string name = Console.ReadLine() ?? "";

                    var result = db.Customers
                        .Where(c => c.CustomerName.Contains(name))
                        .ToList();

                    foreach (var c in result)
                    {
                        Console.WriteLine($"{c.CustomerName} | {c.Email} | {c.Phone}");
                    }
                }

                else if (choice == 6)
                    return;
            }
        }

        // ===============================
        // ADDRESS MANAGEMENT
        // ===============================

        static void AddressMenu(AppDbContext db)
        {
            var addresses = db.CustomerAddresses.ToList();

            foreach (var a in addresses)
            {
                Console.WriteLine("\n-----------------------------");
                Console.WriteLine($"CustomerId : {a.CustomerId}");
                Console.WriteLine($"Street     : {a.Street}");
                Console.WriteLine($"City       : {a.City}");
                Console.WriteLine($"State      : {a.State}");
                Console.WriteLine($"Country    : {a.Country}");
            }
        }

        // ===============================
        // CONTACT PERSON
        // ===============================

        static void ContactMenu(AppDbContext db)
        {
            var contacts = db.ContactPersons.ToList();

            foreach (var c in contacts)
            {
                Console.WriteLine("\n-----------------------------");
                Console.WriteLine($"Name  : {c.Name}");
                Console.WriteLine($"Email : {c.Email}");
                Console.WriteLine($"Phone : {c.Phone}");
                Console.WriteLine($"Title : {c.Title}");
            }
        }

        // ===============================
        // CUSTOMER INTERACTIONS
        // ===============================

        static void InteractionMenu(AppDbContext db)
        {
            var interactions = db.CustomerInteractions.ToList();

            foreach (var i in interactions)
            {
                Console.WriteLine("\n-----------------------------");
                Console.WriteLine($"CustomerId : {i.CustomerId}");
                Console.WriteLine($"Type       : {i.InteractionType}");
                Console.WriteLine($"Subject    : {i.Subject}");
                Console.WriteLine($"Date       : {i.InteractionDate}");
            }
        }

        // ===============================
        // ANALYTICS
        // ===============================

        static void AnalyticsMenu(AppDbContext db)
        {
            Console.WriteLine("\nCustomer Analytics");

            int total = db.Customers.Count();
            int active = db.Customers.Count(c => !c.IsDeleted);

            Console.WriteLine($"Total Customers : {total}");
            Console.WriteLine($"Active Customers: {active}");

            var stats = db.Orders
                .GroupBy(o => o.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalOrders = g.Count(),
                    TotalAmount = g.Sum(x => x.TotalAmount)
                }).ToList();

            foreach (var s in stats)
            {
                Console.WriteLine($"Customer {s.CustomerId} -> Orders: {s.TotalOrders}, Amount: {s.TotalAmount}");
            }
        }

        // ===============================
        // DELETED CUSTOMERS
        // ===============================

        static void DeletedCustomers(AppDbContext db)
        {
            var deleted = db.Customers
                .Where(c => c.IsDeleted)
                .ToList();

            foreach (var c in deleted)
            {
                Console.WriteLine($"{c.CustomerName} (Deleted)");
            }
        }
    }
}
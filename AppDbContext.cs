using Microsoft.EntityFrameworkCore;
using CustomerManagement.Models;

namespace CustomerManagement
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

               protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseLazyLoadingProxies()
        .UseSqlServer("Server=.\\SQLEXPRESS;Database=CustomerManagementDb;Trusted_Connection=True;TrustServerCertificate=True;");
}
       
    }
}

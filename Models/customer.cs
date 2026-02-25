using System.Collections.Generic;

namespace CustomerManagement.Models
{
    public class Customer
{
    public int CustomerId { get; set; }   // Primary Key
    public string Name { get; set; }
    public int Age { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
}

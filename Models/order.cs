using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerManagement.Models
{
    public class Order
{
    public int OrderId { get; set; }   // Primary Key
    public string ProductName { get; set; }
    public double Price { get; set; }

    public int CustomerId { get; set; }   // Foreign Key
    public virtual Customer Customer { get; set; }
}
}

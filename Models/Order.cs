using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public virtual IdentityUser User { get; set; }
        public string AddressId { get; set; } 
        public  Address Address { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } 
        public List<OrderProduct> OrderProducts { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
    }
}

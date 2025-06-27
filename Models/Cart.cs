using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId{ get; set; }
        public int qty { get; set; }
        public string UserId { get; set; } 

        public IdentityUser User { get; set; }
    }
}

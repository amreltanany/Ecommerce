using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public virtual IdentityUser User { get; set; }
        public string AddressesLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
   
    }
}

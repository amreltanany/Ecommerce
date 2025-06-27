using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Controllers
{

    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public CheckOutController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var addresses = await _context.addresses
                .Include(a => a.User)
                .Where(u => u.UserId == currentUser.Id)
                .ToListAsync();
            ViewBag.Addresses = addresses;
            return View();
        }

     
        public async Task<IActionResult> Confirm(int addressId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var address = await _context.addresses
                .Where(a => a.Id == addressId)
                .FirstOrDefaultAsync();
            if (address == null)
            {
                return NotFound();
            }
            //make new order
            decimal orderTotal = 0;

            var carts = await _context.carts
                .Include(c => c.Product)
                .Where(u => u.UserId == currentUser.Id)
                .ToListAsync();

            foreach (var Cart in carts)
            {
                orderTotal += Cart.Product.Price * Cart.qty;
            }

                var order = new Order
            {
                UserId = currentUser.Id,
                AddressId = address.Id.ToString(),
                Amount = orderTotal, // Set this to the total amount of the order
                Status = "Order Placed",
                CreatedAt = DateTime.Now
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var Cart in carts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = Cart.ProductId,
                    Price = Cart.Product.Price,
                    Qty = Cart.qty

                };  
                _context.OrderProducts.Add(orderProduct);
            }
                await _context.SaveChangesAsync();
                return RedirectToAction("ThankYou");
        }
        public IActionResult ThankYou()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Address address)
        {
          if(ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                address.UserId = currentUser.Id;
                _context.addresses.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
          return View(address);

        }
    }


}

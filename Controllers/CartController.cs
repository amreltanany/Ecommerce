using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public CartController(ApplicationDbContext context,                 UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var cart = await _context.carts.Include(x=>x.Product).Where(u =>u.UserId == currentUser.Id).ToListAsync();

            var TotalCost = 0.0m;

            foreach (var CartItem in cart)
            {
                TotalCost += CartItem.Product.Price * CartItem.qty;
            }
            ViewBag.TotalCost = TotalCost;
            return View(cart);
        }
        
        public async Task<IActionResult> UpdateQty(int ProductId, int qty)
        {
            var product = await _context.products.Where(p=>p.Id== ProductId).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var cart = await _context.carts.Where(u => u.UserId == currentUser.Id).Where(u=>u.ProductId == ProductId).FirstOrDefaultAsync();

           if(cart == null)
            {
                return NotFound();
            }
            //update qty
            cart.qty = qty;
            _context.carts.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //add products to cart
        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var product = await _context.products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            var cart = new Models.Cart{ ProductId = productId,qty = qty, UserId = currentUser.Id };
            await _context.carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {

            var cart = await _context.carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }
            _context.carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

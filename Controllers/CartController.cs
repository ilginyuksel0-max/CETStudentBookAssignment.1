using CetStudentBook.Data;
using CetStudentBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CetStudentBook.Controllers
{
    
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            var userId = _userManager.GetUserId(User);

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.BookId == id && c.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    BookId = id,
                    UserId = userId!,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Checkout(int bookid)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Account/Register", new
                {
                    area = "Identity",
                    returnUrl = Url.Action("Checkout", "Cart", new { id = bookid })
                });
            }

            var model = new CheckoutViewModel
            {
                BookId = bookid
            };

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                AddressLine = model.AddressLine,
                City = model.City,
                PostalCode = model.PostalCode,
                OrderItems = new List<OrderItem>()
            };

            if (model.BookId > 0 )
            {
                order.OrderItems.Add(new OrderItem
                {
                    BookId = model.BookId,
                    Quantity = 1
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders", "Orders");
        }
    }
}
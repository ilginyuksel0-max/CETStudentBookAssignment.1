using CetStudentBook.Data;
using CetStudentBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CetStudentBook.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

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
        public async Task<IActionResult> Buy(int id)
        {
            var userId = _userManager.GetUserId(User);

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return NotFound();

            var order = new Order
            {
                UserId = userId!,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        BookId = book.Id,
                        Quantity = 1,
                        Price = book.PageCount
                    }
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders", "Orders");
        }

        [HttpGet]
        public async Task<IActionResult> BuyCart()
        {
            var userId = _userManager.GetUserId(User);

            var cartItems = await _context.CartItems
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Index");

            var order = new Order
            {
                UserId = userId!,
                OrderDate = DateTime.Now,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    BookId = c.BookId,
                    Quantity = c.Quantity,
                    Price = c.Book!.PageCount
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders", "Orders");
        }
    }
}
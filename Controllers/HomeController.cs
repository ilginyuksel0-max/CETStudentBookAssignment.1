using CetStudentBook.Data;
using CetStudentBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CetStudentBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Category)
                .ToListAsync();

            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Reviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int bookId, int rating, string comment)
        {
            if (rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(comment))
            {
                TempData["Message"] = "Please enter a rating and comment.";
                return RedirectToAction("Details", new { id = bookId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = new BookReview
            {
                BookId = bookId,
                UserId = userId!,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.BookReviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Your review has been added successfully.";

            return RedirectToAction("Details", new { id = bookId });
        }

        public async Task<IActionResult> Category(int id)
        {
            var books = await _context.Books
                .Where(b => b.CategoryId == id)
                .ToListAsync();

            return View("Index", books);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
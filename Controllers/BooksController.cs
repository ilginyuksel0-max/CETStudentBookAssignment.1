using System.Threading.Tasks;
using CetStudentBook.Data;
using CetStudentBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CetStudentBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Books
        public async Task<IActionResult> Index()
        {
            var books = await _db.Books.AsNoTracking().ToListAsync();
            return View(books);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            return View(new Book());
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _db.Books.FindAsync(id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return BadRequest();

            if (!ModelState.IsValid)
                return View(book);

            var exists = await _db.Books.AnyAsync(x => x.Id == id);
            if (!exists) return NotFound();

            _db.Update(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _db.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: /Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetStudentBook.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        public IActionResult Add(int id)
        {
            // Şimdilik gerçek veritabanına kaydetmesek bile
            // login kontrolü çalışsın diye bu yeterli
            TempData["Message"] = $"Product {id} added to favorites.";
            return RedirectToAction("Details", "Home", new { id });
        }
    }
}
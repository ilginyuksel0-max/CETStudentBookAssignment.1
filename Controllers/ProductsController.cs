using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CetStudentBook.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToFavorites(int id)
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Buy(int id)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
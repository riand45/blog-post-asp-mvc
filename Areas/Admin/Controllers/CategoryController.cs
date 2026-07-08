using CompanyProfileMVC.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyProfileMVC.Controllers.Admin
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            var categories = new List<CategoryViewModel>
            {
                new() { Id = 1, Name = "Budi Utomo" },
                new() { Id = 2, Name = "Siti Aminah" }
            };

            return View(categories);
        }
    }
}
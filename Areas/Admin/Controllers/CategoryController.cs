using CompanyProfileMVC.Areas.Admin.Models;
using CompanyProfileMVC.Data;
using CompanyProfileMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyProfileMVC.Controllers.Admin
{
    [Area("Admin")]
    public class CategoryController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Nama kategori wajib diisi" });
            }

            var category = new Category { Name = model.Name };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Json(new { id = category.Id, name = category.Name });
        }
    }
}
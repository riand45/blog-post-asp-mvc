using CompanyProfileMVC.Areas.Admin.Models;
using CompanyProfileMVC.Data;
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
    }
}
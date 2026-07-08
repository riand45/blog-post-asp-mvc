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
        private const int PageSize = 10;

        private readonly AppDbContext _context = context;

        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await GetPagedCategoriesAsync(page);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> List(int page = 1)
        {
            var result = await GetPagedCategoriesAsync(page);
            return Json(result);
        }

        private async Task<CategoryListViewModel> GetPagedCategoriesAsync(int page)
        {
            page = Math.Max(page, 1);
            var totalCount = await _context.Categories.CountAsync();

            var items = await _context.Categories
                .OrderBy(c => c.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name })
                .ToListAsync();

            return new CategoryListViewModel
            {
                Items = items,
                Page = page,
                PageSize = PageSize,
                TotalCount = totalCount
            };
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Nama kategori wajib diisi" });
            }

            var category = await _context.Categories.FindAsync(model.Id);
            if (category is null)
            {
                return NotFound(new { message = "Kategori tidak ditemukan" });
            }

            category.Name = model.Name;
            await _context.SaveChangesAsync();

            return Json(new { id = category.Id, name = category.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound(new { message = "Kategori tidak ditemukan" });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
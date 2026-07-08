using Microsoft.AspNetCore.Mvc;

namespace CompanyProfileMVC.Controllers.Admin
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
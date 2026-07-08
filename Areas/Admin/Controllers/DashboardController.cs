using Microsoft.AspNetCore.Mvc;

namespace CompanyProfileMVC.Controllers.Admin
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
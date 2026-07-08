using Microsoft.AspNetCore.Mvc;

namespace CompanyProfileMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult Index(LoginViewModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         return RedirectToAction("Index", "Home");
        //     }
        //     return View(model);
        // }
    }
}
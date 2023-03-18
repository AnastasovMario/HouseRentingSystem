using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class House : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

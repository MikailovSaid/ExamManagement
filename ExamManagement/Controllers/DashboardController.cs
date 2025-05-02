using Microsoft.AspNetCore.Mvc;

namespace ExamManagement.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

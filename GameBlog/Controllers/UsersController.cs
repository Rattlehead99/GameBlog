using Microsoft.AspNetCore.Mvc;

namespace GameBlog.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

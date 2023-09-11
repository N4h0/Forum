using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Forum.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserLogInn()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class PostController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

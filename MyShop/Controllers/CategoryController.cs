using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class CategoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

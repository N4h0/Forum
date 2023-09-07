using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class ItemController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

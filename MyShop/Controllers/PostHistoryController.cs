using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class PostHistoryController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

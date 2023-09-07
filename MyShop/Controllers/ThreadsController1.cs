using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
	public class ThreadsController1 : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

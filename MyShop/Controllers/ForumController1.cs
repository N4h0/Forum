using Microsoft.AspNetCore.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
	public class ForumController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

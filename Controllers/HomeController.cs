using BookTrackerMain.Models;
using BookTrackerMain.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BookTrackerMain.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserBookDetailRepository userBookDetailRepository;

		public HomeController(ILogger<HomeController> logger, UserBookDetailRepository userBookDetailRepository)
		{
			_logger = logger;
			this.userBookDetailRepository = userBookDetailRepository;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<UserBookDetail> userBookDetails = null;
			bool userAuthenticated = User.Identity != null && User.Identity.IsAuthenticated;
			if (userAuthenticated)
			{
				var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
				var result = await userBookDetailRepository.GetMultipleAsync(userId, null);
				if (result != null)
				{
					userBookDetails = result.DistinctBy(userBookDetail => userBookDetail.UserId + "_" + userBookDetail.BookId).ToList();
				}
			}
			ViewBag.userAuthenticated = userAuthenticated;
			return View(userBookDetails);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
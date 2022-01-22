using BookTrackerMain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BookTrackerMain.Controllers
{
	[Route("search")]
	public class SearchController : Controller
	{
		private HttpClient client;

		public SearchController()
		{
			client = new HttpClient();
		}

		[Route("")]
		public async Task<IActionResult> SearchResults(string query)
		{
			var searchResults = await client.GetFromJsonAsync<SearchResults>($"https://openlibrary.org/search.json?q={query}");
			return View(searchResults);
		}
	}
}

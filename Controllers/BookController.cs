using BookTrackerMain.Models;
using BookTrackerMain.repository;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackerMain.Controllers
{
	[Route("book")]
	public class BookController : Controller
	{
		private BookRepository bookRepository;

		public BookController(BookRepository bookRepository)
		{
			this.bookRepository = bookRepository;
		}
		[HttpGet]
		[Route("{bookId}")]
		public IActionResult BookDetail(string bookId)
		{
			var book = bookRepository.Get(bookId);
			if (book == null) return LocalRedirect("/Home/Error");
			return View(book);
		}
	}
}

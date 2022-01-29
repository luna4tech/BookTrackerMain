using BookTrackerMain.Models;
using BookTrackerMain.repository;
using BookTrackerMain.Repository;
using Cassandra;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace BookTrackerMain.Controllers
{
	[Route("book")]
	public class BookController : Controller
	{
		private BookRepository bookRepository;
		private UserBookRepository userBookRepository;

		public BookController(BookRepository bookRepository, UserBookRepository userBookRepository)
		{
			this.bookRepository = bookRepository;
			this.userBookRepository = userBookRepository;
		}
		[HttpGet]
		[Route("{bookId}")]
		public async Task<IActionResult> BookDetail(string bookId)
		{
			var book = await bookRepository.GetAsync(bookId);
			if (book == null) return LocalRedirect("/Home/Error");

			string? userId = null;
			UserBook? userBook = null;
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
				userBook = await userBookRepository.GetAsync(bookId, userId);
				if (userBook == null)
				{
					userBook = new UserBook { BookId = bookId, UserId = userId };
				}
			}
			
			var tupleModel = new Tuple<Book, UserBook?>(book, userBook);
			return View(tupleModel);
		}

		[HttpPost]
		[Route("userdetail/{bookId}/{userId}")]
		public async Task<IActionResult> UserBookDetail(string bookId, string userId)
		{
			var userBook = new UserBook
			{
				BookId = bookId,
				UserId = userId
			};
			
			if(validate(Request.Form["Item2.StartDate"]))
			{
				userBook.StartDate = LocalDate.Parse(Request.Form["Item2.StartDate"]);
			}
			if (validate(Request.Form["Item2.EndDate"]))
			{
				userBook.EndDate = LocalDate.Parse(Request.Form["Item2.EndDate"]);
			}
			if (validate(Request.Form["Item2.Rating"]))
			{
				userBook.Rating = int.Parse(Request.Form["Item2.Rating"]);
			}
			if (validate(Request.Form["Item2.Status"]))
			{
				userBook.Status = Request.Form["Item2.Status"];
			}
			await userBookRepository.InsertAsync(userBook);
			return LocalRedirect($"/book/{userBook.BookId}");
		}

		private bool validate(StringValues values)
		{
			return !(values.FirstOrDefault() == null || values.FirstOrDefault() == "");
		}
	}
}

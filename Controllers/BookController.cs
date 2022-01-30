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
		private UserBookDetailRepository userBookDetailRepository;

		public BookController(BookRepository bookRepository, UserBookRepository userBookRepository, UserBookDetailRepository userBookDetailRepository)
		{
			this.bookRepository = bookRepository;
			this.userBookRepository = userBookRepository;
			this.userBookDetailRepository = userBookDetailRepository;
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

			var book = await bookRepository.GetAsync(bookId);
			var userBookDetail = new UserBookDetail
			{
				UserId = userBook.UserId,
				BookId = userBook.BookId,
				Status = userBook.Status,
				StartDate = userBook.StartDate,
				EndDate = userBook.EndDate,
				Rating = userBook.Rating,
				Title = book!.Name,
				AuthorNames = book.AuthorNames,
				CoverId = book.CoverIds.FirstOrDefault(),
				LastModified = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
			};
			await userBookRepository.InsertAsync(userBook);
			await userBookDetailRepository.InsertAsync(userBookDetail);
			return LocalRedirect($"/book/{userBook.BookId}");
		}

		private bool validate(StringValues values)
		{
			return !(values.FirstOrDefault() == null || values.FirstOrDefault() == "");
		}
	}
}

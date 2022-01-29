using BookTrackerMain.Models;
using Cassandra.Data.Linq;
using Microsoft.Extensions.Configuration;

namespace BookTrackerMain.repository
{
	public class BookRepository : CassandraRepository<Book>
	{
		public BookRepository(Cassandra.ISession session) : base(session, "book_id")
		{

		}
	}
}

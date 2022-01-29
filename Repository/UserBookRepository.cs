using BookTrackerMain.Models;
using BookTrackerMain.repository;

namespace BookTrackerMain.Repository
{
	public class UserBookRepository: CassandraRepository<UserBook>
	{
		public UserBookRepository(Cassandra.ISession session): base(session, "book_id", "user_id") { }
	}
}

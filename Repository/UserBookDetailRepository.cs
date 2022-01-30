using BookTrackerMain.Models;
using BookTrackerMain.repository;

namespace BookTrackerMain.Repository
{
	public class UserBookDetailRepository: CassandraRepository<UserBookDetail>
	{
		public UserBookDetailRepository(Cassandra.ISession session): base(session, "user_id") { }
	}
}

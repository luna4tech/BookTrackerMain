using BookTrackerMain.Models;
using Cassandra.Data.Linq;
using Microsoft.Extensions.Configuration;

namespace BookTrackerMain.repository
{
	public class AuthorRepository : CassandraRepository<Author>
	{
		public AuthorRepository(Cassandra.ISession session) : base(session, "author_id")
		{

		}
	}
}

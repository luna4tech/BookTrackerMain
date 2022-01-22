using BookTrackerMain.models;
using Cassandra.Data.Linq;
using Microsoft.Extensions.Configuration;

namespace BookTrackerMain.repository
{
	public class AuthorRepository : CassandraRepository<Author>
	{
		public AuthorRepository(IConfiguration config) : base(config, "author_id")
		{

		}
	}
}

using Cassandra.Data.Linq;
using Cassandra.Mapping;

namespace BookTrackerMain.repository
{
	public class CassandraRepository<T>
	{
		protected Cassandra.ISession Session { get; }
		protected IMapper Mapper { get; }
		protected Table<T> Table { get; }
		protected string PrimaryKeyName { get; }
		protected string? ClusteringKeyName { get; }
		public CassandraRepository(Cassandra.ISession session, string primaryKeyName, string? clusteringKeyName = null)
		{
			Session = session;

			Mapper = new Mapper(Session);

			Table = new Table<T>(Session);
			Table.CreateIfNotExists();

			PrimaryKeyName = primaryKeyName;
			ClusteringKeyName = clusteringKeyName;
		}

		public async Task InsertAsync<T>(T row)
		{
			await Mapper.InsertAsync<T>(row);
		}

		public async Task<T?> GetAsync(string id, string? clustering_id = null)
		{
			try
			{
				var query = $"select * from {Table.Name} where {PrimaryKeyName}='{id}'";
				var clusteringQuery = ClusteringKeyName != null ? $" and {ClusteringKeyName}='{clustering_id}'" : "";
				return await Mapper.SingleAsync<T>(query + clusteringQuery);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return default(T);
			}
		}

		public async Task<IPage<T>> GetMultipleAsync(string id, byte[] pagingState)
		{
			try
			{

				var query = $"select * from {Table.Name} where {PrimaryKeyName}='{id}'";
				var result = await Mapper.FetchPageAsync<T>(5, pagingState, query, null);
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}
	}
}

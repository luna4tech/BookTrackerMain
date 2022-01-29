using Cassandra;
using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BookTrackerMain.Models
{
	[Table("userbook_by_id")]
	public class UserBook
	{
		[PartitionKey(0)]
		[Column("user_id")]
		public string UserId { get; set; } = default!;

		[ClusteringKey]
		[Column("book_id")]
		public string BookId { get; set; } = default!;

		
		[Column("status")]
		public string Status { get; set; } = default!;

		[Column("start_date")]
		public LocalDate StartDate { get; set; } = default!;

		[Column("end_date")]
		public LocalDate EndDate { get; set; } = default!;

		[Column("rating")]
		public int Rating { get; set; } = default!;
	}
}

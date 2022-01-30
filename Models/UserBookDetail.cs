using Cassandra;
using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BookTrackerMain.Models
{
	[Table("userbook_detail_by_id")]
	public class UserBookDetail
	{
		[PartitionKey(0)]
		[Column("user_id")]
		public string UserId { get; set; } = default!;

		[Column("book_id")]
		public string BookId { get; set; } = default!;

		[ClusteringKey(1, SortOrder.Ascending)]
		[Column("status")]
		public string Status { get; set; } = default!;

		[ClusteringKey(0, SortOrder.Descending)]
		[Column("last_modified")]
		public long LastModified { get; set; } = default!;

		[Column("start_date")]
		public LocalDate StartDate { get; set; } = default!;

		[Column("end_date")]
		public LocalDate EndDate { get; set; } = default!;

		[Column("rating")]
		public int Rating { get; set; } = default!;

		[Column("title")]
		public string Title { get; set; } = default!;

		[Column("author_names")]
		public List<string> AuthorNames { get; set; } = default!;

		[Column("cover_id")]
		public string? CoverId { get; set; }
	}
}

using System.Text.Json.Serialization;

namespace BookTrackerMain.Models
{
	public class SearchResult
	{
		[JsonPropertyName("key")]
		public string Key { get; set; } = default!;
		[JsonPropertyName("title")]
		public string Title { get; set; } = default!;
		[JsonPropertyName("cover_i")]
		public int CoverId { get; set; } = default!;
		[JsonPropertyName("author_name")]
		public List<string> AuthorNames { get; set; } = default!;
	}
}

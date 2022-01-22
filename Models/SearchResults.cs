namespace BookTrackerMain.Models
{
	public class SearchResults
	{
		public int NumFound { get; set; }
		public List<SearchResult> Docs { get; set; }
	}
}

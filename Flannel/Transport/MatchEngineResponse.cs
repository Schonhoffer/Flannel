namespace Flannel.Transport
{
	public class MatchEngineResponse
	{
		public string status { get; set; }
		public string method { get; set; }
		public SearchResult[] result { get; set; }
		public string[] error { get; set; }
	}
}
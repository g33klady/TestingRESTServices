namespace TestingRESTServices.Models
{
	public class User
	{
		public string id { get; set; }
		public string username { get; set; }
		public string displayName { get; set; }
		public string url { get; set; }
		public string avatarUrl { get; set; }
		public string avatarUrlSmall { get; set; }
		public string avatarUrlMedium { get; set; }
		public string[] providers { get; set; }
		public int v { get; set; }
		public string gv { get; set; }
	}

}

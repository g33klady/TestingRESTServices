using System;

namespace TestingRESTServices.Models
{
	public class GetMessages_ResponseModel
	{
		public string id { get; set; }
		public string text { get; set; }
		public string html { get; set; }
		public DateTime sent { get; set; }
		public DateTime editedAt { get; set; }
		public User fromUser { get; set; }
		public bool unread { get; set; }
		public int readBy { get; set; }
		public object[] urls { get; set; }
		public object[] mentions { get; set; }
		public object[] issues { get; set; }
		public object[] meta { get; set; }
		public int v { get; set; }
	}

}
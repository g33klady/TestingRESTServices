using System;

namespace TestingRESTServices.Models
{
	public class GetRooms_ResponseModel
	{
		public string id { get; set; }
		public string name { get; set; }
		public string topic { get; set; }
		public string avatarUrl { get; set; }
		public string uri { get; set; }
		public bool oneToOne { get; set; }
		public int userCount { get; set; }
		public int unreadItems { get; set; }
		public int mentions { get; set; }
		public DateTime lastAccessTime { get; set; }
		public bool lurk { get; set; }
		public string url { get; set; }
		public string githubType { get; set; }
		public string security { get; set; }
		public bool noindex { get; set; }
		public string[] tags { get; set; }
		public bool roomMember { get; set; }
		public string groupId { get; set; }
		public bool _public { get; set; }
		public User user { get; set; }
		public int v { get; set; }
	}



}


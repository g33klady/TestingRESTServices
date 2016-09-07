using NUnit.Framework;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using TestingRESTServices.Models;
using Newtonsoft.Json;

namespace TestingRESTServices
{
	[TestFixture]
	public class UnitTest1
	{
		private static string _accessToken;
		private static string _baseUri;
		private static string _groupId;

		[OneTimeSetUp]
		public void TestClassInitialize()
		{
			_accessToken = ConfigurationManager.AppSettings["accessToken"];
			_baseUri = ConfigurationManager.AppSettings["baseUri"];
			_groupId = ConfigurationManager.AppSettings["groupId"];
		}

		[Test]
		public void GetRoomsWhereUserIsMe()
		{
			string uri = _baseUri + "/user/me/rooms?access_token=" + _accessToken;
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "GET");
			Console.WriteLine(response);
			var respString = response.Content.ReadAsStringAsync().Result;
			Console.WriteLine(respString);
			List<GetRooms_ResponseModel> list = JsonConvert.DeserializeObject<List<GetRooms_ResponseModel>>(respString);
			foreach(var room in list)
			{
				Console.WriteLine("Room name:" + room.name + "; Room group ID: " + room.groupId);
				Assert.AreEqual(_groupId, room.groupId, "Room " + room.name + "has group ID " + room.groupId + "which is unexpected");
			}
		}
	}
}

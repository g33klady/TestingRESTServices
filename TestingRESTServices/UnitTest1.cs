using NUnit.Framework;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using TestingRESTServices.Models;
using Newtonsoft.Json;
using System.Collections;
using System.IO;

namespace TestingRESTServices
{
	[TestFixture]
	public class UnitTest1
	{
		private static string _accessToken;
		private static string _baseUri;
		private static string _groupId;
		private static string _roomId;

		[OneTimeSetUp]
		public void TestClassInitialize()
		{
			_accessToken = ConfigurationManager.AppSettings["accessToken"];
			_baseUri = ConfigurationManager.AppSettings["baseUri"];
			_groupId = ConfigurationManager.AppSettings["groupId"];
			_roomId = ConfigurationManager.AppSettings["roomId"];
		}

		[Test]
		public void GetRoomsWhereUserIsMe()
		{
			string uri = _baseUri + "/user/me/rooms?access_token=" + _accessToken;
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "GET");
			Assert.IsTrue(response.IsSuccessStatusCode, "Response code was not success");
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

		[Test]
		public void PostMessageToRoom()
		{
			string uri = _baseUri + "/rooms/" + _roomId + "/chatMessages?access_token=" + _accessToken;
			string req = "{\"text\":\"this is a test\"}";
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "POST", req);
			var respString = response.Content.ReadAsStringAsync().Result;
			Assert.IsTrue(response.IsSuccessStatusCode, "Response code was not success");
			GetMessages_ResponseModel message = JsonConvert.DeserializeObject<GetMessages_ResponseModel>(respString);
			string messageId = message.id;

			string getUri = _baseUri + "/rooms/" + _roomId + "/chatMessages/" + messageId + "?access_token=" + _accessToken;
			HttpResponseMessage responseToGet = Utilities.SendHttpWebRequest(uri, "GET");
			Assert.IsTrue(responseToGet.IsSuccessStatusCode, "Response code was not success");
		}

		[Test, TestCaseSource(typeof(MyDataClass), "TestCases")]
		public string PostMessagesToRoom(string file)
		{
			string uri = _baseUri + "/rooms/" + _roomId + "/chatMessages?access_token=" + _accessToken;
			string path = @"C:\Source\Repos\TestingRestServicesGit\TestingRESTServices\bin\Debug\TestData\" + file;
			string req;
			using (StreamReader sr = new StreamReader(path))
			{
				req = sr.ReadToEnd();
			}
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "POST", req);
			var respString = response.Content.ReadAsStringAsync().Result;
			return response.StatusCode.ToString();
		}
	}

	public class MyDataClass
	{
		public static IEnumerable TestCases
		{
			get
			{
				yield return new TestCaseData("ZeroCharactersTestData.json").Returns("OK");
				yield return new TestCaseData("TenCharactersTestData.json").Returns("OK");
				yield return new TestCaseData("FourThousandNinetySixCharactersTestData.json").Returns("OK");
				yield return new TestCaseData("FourThousandNinetySevenCharactersTestData.json").Returns("BadRequest");
			}
		}
	}
}

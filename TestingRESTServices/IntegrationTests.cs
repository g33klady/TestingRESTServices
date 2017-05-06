using System.Configuration;
using System.Collections;
using System.IO;
using System.Net.Http;
using TestingRESTServices.Models;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Reflection;

namespace TestingRESTServices
{
	[TestFixture]
	public class IntegrationTests
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
		public void GetUserInfoReturnsAvatarsInThreeSizes()
		{
			string uri = _baseUri + "/user/me?access_token=" + _accessToken;
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "GET");
			Assert.IsTrue(response.IsSuccessStatusCode, "Response code was not successful");

			string respString = Utilities.ReadWebResponse(response);
			User user = JsonConvert.DeserializeObject<User>(respString);
			Assert.IsNotNull(user.avatarUrl, "AvatarUrl is null");
			Assert.IsNotNull(user.avatarUrlMedium, "AvatarUrlMedium is null");
			Assert.IsNotNull(user.avatarUrlSmall, "AvatarUrlSmall is null");
		}

		[Test]
		public void PostMessageToRoomPostsTheMessageToTheRoom()
		{
			string uri = _baseUri + "/rooms/" + _roomId + "/chatMessages?access_token=" + _accessToken;
			string reqBody = "{\"text\":\"this is a test\"}";

			HttpResponseMessage responseToPost = Utilities.SendHttpWebRequest(uri, "POST", reqBody);
			string respString = Utilities.ReadWebResponse(responseToPost);
			Assert.IsTrue(responseToPost.IsSuccessStatusCode, "Response code to POST was not a success");

			GetMessages_ResponseModel message = JsonConvert.DeserializeObject<GetMessages_ResponseModel>(respString);
			string messageId = message.id;

			//do the GET with the messageId
			string getUri = _baseUri + "/rooms/" + _roomId + "/chatMessages/" + messageId + "?access_token=" + _accessToken;
			HttpResponseMessage respToGet = Utilities.SendHttpWebRequest(getUri, "GET");
			Assert.IsTrue(respToGet.IsSuccessStatusCode, "Response code to GET was not successful");

			string respToGetString = Utilities.ReadWebResponse(respToGet);
			GetMessages_ResponseModel getMessage = JsonConvert.DeserializeObject<GetMessages_ResponseModel>(respToGetString);
			Assert.AreEqual("this is a test", getMessage.text, "message text mismatch after GET");
		}

		[Test, TestCaseSource(typeof(MyTestDataClass), "TestCases")]
		public string PostMessagesToRoomUsingBoundaries(string file)
		{
			string uri = _baseUri + "/rooms/" + _roomId + "/chatMessages?access_token=" + _accessToken;
			string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string path = dir + "\\TestData\\" + file;
			string req;
			using (StreamReader sr = new StreamReader(path))
			{
				req = sr.ReadToEnd();
			}
			HttpResponseMessage response = Utilities.SendHttpWebRequest(uri, "POST", req);
			string respString = Utilities.ReadWebResponse(response);
			return response.StatusCode.ToString();
		}
	}

	public class MyTestDataClass
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

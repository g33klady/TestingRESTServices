using NUnit.Framework;
using System.Configuration;

namespace TestingRESTServices
{
	[TestFixture]
	public class UnitTest1
	{
		private static string _accessToken;
		private static string _baseUri;

		[OneTimeSetUp]
		public void TestClassInitialize(TestContext context)
		{
			_accessToken = ConfigurationManager.AppSettings["accessToken"];
			_baseUri = ConfigurationManager.AppSettings["baseUri"];
		}

		[Test]
		public void TestMethod1()
		{
		}
	}
}

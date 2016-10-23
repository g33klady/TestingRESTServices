using System;
using System.Configuration;
using NUnit.Framework;
using System.Data;

namespace UsingDatabasesInTesting
{
    [TestFixture]
    public class SqlIntegrationTests
    {
        private static string _connectionString;
        [OneTimeSetUp]
        public void SetUp()
        {
            _connectionString = ConfigurationManager.AppSettings["dbConnectionString"];
        }

        [Test]
        public void VerifyInventoryIsUpdated()
        {
            //set up the values for our test
            int quantityModifier = -1;
            string productId = "875";
            string selectQuery = GetInventoryQueryString(productId);
            
            //get the number we have in the database at the start of our tests
            DataTable initialInventoryDt = Utilities.ExecuteSqlQuery(selectQuery, _connectionString); 
            int initialQuantity = Int32.Parse(initialInventoryDt.Rows[0][0].ToString()); //we need to grab the 1st row and 1st column of the datatable, and then make sure it's an integer
            int expectedQuantity = initialQuantity + quantityModifier; //what the Quantity should be if our PUT was successful 

            //here we would do our REST call to update the quantity
            //for example:
            // string url = String.Format("http://ourwarehouse/api/products/{0}/inventory/{1}, productId, quantityModifier");
            //  HttpResponseMessage response = Utilities.SendHttpWebRequest(url, "PUT");
            //  Assert.IsTrue(response.IsSuccessStatusCode, "Response code to PUT was not successful");

            //******NOTE: This section wouldn't be in our tests normally BUT to make our tests pass, we need to actually update the DB!*****
            string updateCmd = GetUpdateInventoryString(expectedQuantity, productId);
            int code = Utilities.ExecuteSqlCommand(updateCmd, _connectionString);
            Assert.IsTrue(code == 1, "more than 1 row was affected, something went wrong");
            //*****end the section of code that is only needed for this example because we don't have a REST api to use


            //get the number we have in the database now
            DataTable updatedInventoryDt = Utilities.ExecuteSqlQuery(selectQuery, _connectionString);
            int updatedQuantity = Int32.Parse(updatedInventoryDt.Rows[0][0].ToString()); //we need to grab the 1st row and 1st column of the datatable, and then make sure it's an integer

            //we now can assert that our updatedQuantity is the same as expectedQuantity (or the initialQuantity - 1)
            Assert.AreEqual(expectedQuantity, updatedQuantity, "Updated Quantity is not as expected; it is " + updatedQuantity + " but should be " + expectedQuantity);
        }

        [Test]
        public void VerifyApiIsGettingInformationCorrectly()
        {
            string productId = "875";
            string selectQuery = GetInventoryQueryString(productId);

            //get the number we have in the database
            DataTable dbInventoryDt = Utilities.ExecuteSqlQuery(selectQuery, _connectionString);
            int dbQuantity = Int32.Parse(dbInventoryDt.Rows[0][0].ToString());

            //here we would do our REST call to GET the quantity
            //for example:
            // string url = String.Format("http://ourwarehouse/api/products/{0}/inventory/", productId);
            //  HttpResponseMessage response = Utilities.SendHttpWebRequest(url, "GET");
            //  Assert.IsTrue(response.IsSuccessStatusCode, "Response code to GET was not successful");
            //  string responseString = Utilities.ReadWebResponse(response);
            //  Inventory_ResponseModel getInventory = JsonConvert.DeserializeObject<Inventory_ResponseModel>(responseString);
            //  int apiQuantity = getInventory.Quantity;

            //******NOTE: because we don't have an API to hit for this, we're going to pretend. This won't be in your tests!
            int apiQuantity = dbQuantity;
            //****** end of the section of code that WILL NOT BE IN YOU TESTS! We're forcing this to pass!

            Assert.AreEqual(dbQuantity, apiQuantity, "API did not return the same quantity as in the database");


        }

        public string GetInventoryQueryString(string productId)
        {
            return String.Format("SELECT Quantity FROM AdventureWorks2012.Production.ProductInventory WHERE ProductID = {0}", productId);
        }

        public string GetUpdateInventoryString(int quantity, string productId)
        {
            return String.Format("UPDATE AdventureWorks2012.Production.ProductInventory SET Quantity = {0} WHERE ProductID = {1}", quantity, productId);
        }
    }
}

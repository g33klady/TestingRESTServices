using System.Data;
using System.Data.SqlClient;

namespace UsingDatabasesInTesting
{
	public static class Utilities
	{
		public static DataTable GetQuantityOfProduct(string productId, string connectionString)
		{
			string commandText = "SELECT Quantity FROM AdventureWorks2012.Production.ProductInventory WHERE ProductID = @ID;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				SqlCommand command = new SqlCommand(commandText, connection);
				command.Parameters.Add("@ID", SqlDbType.NVarChar);
				command.Parameters["@ID"].Value = productId;
				using (var da = new SqlDataAdapter(command))
				{
					var dt = new DataTable();
					da.Fill(dt);
					return dt;
				}
			}
		}

		public static int UpdateQuantityOfProduct(string productId, int quantity, string connectionString)
		{
			int code = 0;
			string commandText = "UPDATE AdventureWorks2012.Production.ProductInventory SET Quantity = @Quantity WHERE ProductID = @ID;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(commandText, connection))
				{
					command.Parameters.Add("@Quantity", SqlDbType.Int);
					command.Parameters["@Quantity"].Value = quantity;
					command.Parameters.Add("@ID", SqlDbType.NVarChar);
					command.Parameters["@ID"].Value = productId;
					connection.Open();
					code = command.ExecuteNonQuery();
					command.Dispose();
				}
			}
			return code;
		}

	}
}

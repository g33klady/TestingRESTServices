using System.Data;
using System.Data.SqlClient;

namespace UsingDatabasesInTesting
{
    public static class Utilities
    {
        public static DataTable ExecuteSqlQuery(string query, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var da = new SqlDataAdapter(query, conn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static int ExecuteSqlCommand(string query, string connectionString)
        {
            int code = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    code = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            return code; //number of rows affected by the command if UPDATE, INSERT, DELETE
        }
    }
}

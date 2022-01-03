using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AzureWebsite.Library.Inkasso
{
    public static class Database
    {
        internal static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        private static string GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "(LocalDb)\\MSSQLLocalDB";
            builder.IntegratedSecurity = true;
            builder.PersistSecurityInfo = false;
            builder.Pooling = false;
            builder.MultipleActiveResultSets = false;
            builder.ConnectTimeout = 60;
            builder.Encrypt = false;
            builder.TrustServerCertificate = false;
            builder.InitialCatalog = "Inkasso";
            return builder.ConnectionString;
        }
        public static Task<DataSet> GetDataSetAsync(string sSQL)
        {
            return Task.Run(() =>
            {
                using (var newConnection = new SqlConnection(GetConnectionString()))
                using (var mySQLAdapter = new SqlDataAdapter(sSQL, newConnection))
                {
                    mySQLAdapter.SelectCommand.CommandType = CommandType.Text;
                    DataSet myDataSet = new DataSet();
                    mySQLAdapter.Fill(myDataSet);
                    return myDataSet;
                }
            });
        }

        public static Task<int> ExecuteCommandAsync(string sSQL)
        {
            return Task.Run(() =>
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sSQL, connection))
                    {
                        return (int)command.ExecuteScalar();
                    }
                }
            });
        }

    }
}

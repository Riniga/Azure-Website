using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            builder.UserID = "inkasso";
            builder.Password = "B252bdb8!";
            builder.InitialCatalog = "Inkasso";

            return builder.ConnectionString;
        }

       
    }
}

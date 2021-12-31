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

       
    }
}

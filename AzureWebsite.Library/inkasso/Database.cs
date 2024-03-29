﻿using System;
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
        public static async Task<DataSet> GetDataSetAsync(string sSQL)
        {
                using (var newConnection = new SqlConnection(GetConnectionString()))
                using (var mySQLAdapter = new SqlDataAdapter(sSQL, newConnection))
                {
                    mySQLAdapter.SelectCommand.CommandType = CommandType.Text;
                    var result= new DataSet();
                    await Task.Run(() => mySQLAdapter.Fill(result));
                    return result;
                }
        }

        public static async Task<int> ExecuteScalarCommandAsync(string sSQL)
        {
            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            using SqlCommand command = new SqlCommand(sSQL, connection);
            return (int)await command.ExecuteScalarAsync();
        }
        public static async Task ExecuteCommandAsync(string sSQL)
        {
            using SqlConnection connection = new SqlConnection(GetConnectionString());
            connection.Open();
            using SqlCommand command = new SqlCommand(sSQL, connection);
            await command.ExecuteNonQueryAsync();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AzureWebsite.Library.Inkasso
{
    public static class Contracts
    {
        public static List<Contract> GetContracts()
        {
            var contracts = new List<Contract>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, Name FROM Contracts";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contracts.Add(new Contract { Id = (Guid)reader["Id"], Name = (string)reader["Name"] });
                        }
                    }
                }
            }
            return contracts;
        }
        public static Contract GetContract(Guid Id)
        {
            Contract contract = null;
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, Name FROM Contracts WHERE Id ='{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contract = new Contract { Id = (Guid)reader["Id"], Name = (string)reader["Name"] };
                        }
                    }
                }
            }
            if (contract == null) throw new Exception($"Contract with id {Id} not found");
            return contract;
        }

        public static void CreateContract(Contract contract)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"INSERT INTO Contracts (Name) VALUES('{contract.Name}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        
        public static void SeedContracts(int count, string companies)
        {
            Random randomizer = new Random();
            var listOfCompanies = companies.Replace("\"", "").Split(',');

            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"TRUNCATE table Contracts";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            for (int i = 0; i < count; i++)
            {
                var name = listOfCompanies[randomizer.Next(listOfCompanies.Length)].ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);
                CreateContract(new Contract { Name = name });
            }
        }
    }
}

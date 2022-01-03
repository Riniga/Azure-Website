using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AzureWebsite.Library.Inkasso
{
    public static class Contracts
    {
        public static async Task<List<Contract>> GetContractsAsync()
        {
            var contracts = new List<Contract>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Name FROM Contracts");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    contracts.Add(new Contract { Id = (int)row["Id"], Name = (string)row["Name"] });
                }
            }
            return contracts;
        }
        public static async Task<Contract> GetContractAsync(int Id)
        {
            Contract contract = null;
            var contracts = new List<Contract>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Name FROM Contracts WHERE Id ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    
                    contract = new Contract { Id = (int)row["Id"], Name = (string)row["Name"] };
                }
            }
            if (contract == null) throw new Exception($"Contract with id {Id} not found");
            return contract;
        }

        private static async void CreateContractAsync(Contract contract)
        {
            await Database.ExecuteCommandAsync($"INSERT INTO Contracts (Name) OUTPUT INSERTED.ID VALUES('{contract.Name}')");
        }
        
        public static void SeedContracts(int count, string companies)
        {
            Random randomizer = new Random();
            var listOfCompanies = companies.Replace("\"", "").Split(',');

            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT TOP 1 Id FROM Contracts";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) return;
                    }
                }
            }
            for (int i = 0; i < count; i++)
            {
                var name = listOfCompanies[randomizer.Next(listOfCompanies.Length)].ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);
                CreateContractAsync(new Contract { Name = name });
            }
        }
    }
}

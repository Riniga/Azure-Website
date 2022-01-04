using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AzureWebsite.Library.Inkasso
{
    public static class Persons    
    {
        public static async Task<List<Person>> GetPersonsAsync()
        {
            var persons = new List<Person>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Id, Name FROM Persons");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    persons.Add(new Person { Id = (int)row["Id"], Name = (string)row["Name"] });
                }
            }
            return persons;
        }
        public static async Task<Person> GetPersonAsync(int Id)
        {
            Person person = null;
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT Name FROM Persons WHERE Id ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    person = new Person { Id = Id, Name = (string)row["Name"] };
                }
            }
            if (person == null) throw new Exception($"Person with id {Id} not found");
            return person;
        }

        public static async Task CreatePersonAsync(Person person, Contract contract, decimal amount)
        {
            person.Id = await Database.ExecuteScalarCommandAsync($"INSERT INTO Persons (Name) OUTPUT INSERTED.ID VALUES('{person.Name}')");
            await Debts.CreateDebtAsync(person, contract, amount);
        }
        public static async void UpdatePersonAsync(Person person)
        {
            await Database.ExecuteCommandAsync($"UPDATE Persons SET Name='{person.Name}' WHERE Id='{person.Id}'");
        }

        public static async Task SeedPersonsAsync(int count, string names)
        {
            List<Person> persons = new List<Person>();
            Random randomizer = new Random();
            var contracts = Contracts.GetContractsAsync().Result;
            var listOfNames = names.Replace("\"", "").Split(',');
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT TOP 1 Id FROM Persons";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) return;
                    }
                }
            }
            var tasks = new List<Task>();

            for (int i = 0; i < count; i++)
            {
                var name = listOfNames[randomizer.Next(listOfNames.Length)].ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);
                var contract = contracts[randomizer.Next(contracts.Count)];
                var person = new Person { Name = name };
                decimal amount = (1000 + randomizer.Next(1000)) * 100;
                persons.Add(person);
                tasks.Add(CreatePersonAsync(person, contract, amount));
            }
            await Task.WhenAll(tasks);

            tasks = new List<Task>();
            foreach (var person in persons)
            {
                var NumberOfAditionalDebts = (decimal)Math.Floor(5 / (double)(randomizer.Next(13) + 1));
                for (var j = 0; j < NumberOfAditionalDebts; j++)
                {
                    var contract = contracts[randomizer.Next(contracts.Count)];
                    var amount = (1000 + randomizer.Next(1000)) * 100;
                    tasks.Add(Debts.CreateDebtAsync(person, contract, amount));
                }
            }
            await Task.WhenAll(tasks);
            /*TODO: Seed with transactions foreach debt
         *  each month 
         *      set saldo=0 if saldo<100
         *      add interest and fee (if debt>0)
         *      add payment 
         */



        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AzureWebsite.Library.Inkasso
{
    public static class Persons    
    {
        public static List<Person> GetPersons()
        {
            var persons = new List<Person>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();

                String sql = "SELECT Id, Name FROM Persons";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            persons.Add(new Person { Id= (int)reader["Id"], Name= (string)reader["Name"]});
                        }
                    }
                }
            }
            return persons;
        }
        public static Person GetPerson(int Id)
        {
            Person person = null;
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Name FROM Persons WHERE Id ='{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person = new Person { Id = Id, Name = (string)reader["Name"] };
                        }
                    }
                }
            }
            if (person == null) throw new Exception($"Person with id {Id} not found");
            return person;
        }

        public static void CreatePerson(Person person, Contract contract, decimal amount)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"INSERT INTO Persons (Name) OUTPUT INSERTED.ID VALUES('{person.Name}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    person.Id = (int)command.ExecuteScalar();

                }
            }
            Debts.CreateDebt(person, contract, amount);
        }
        public static bool UpdatePerson(Person person)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"UPDATE Persons SET Name='{person.Name}' WHERE Id='{person.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                return true;
            }
        }

        public static void SeedPersons(int count, string names)
        {
            Random randomizer = new Random();
            var contracts = Contracts.GetContracts();
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

            for (int i = 0; i < count; i++)
            {
                var name = listOfNames[randomizer.Next(listOfNames.Length)].ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);


                var contract = contracts[randomizer.Next(contracts.Count)];
                var person = new Person { Name = name };
                decimal amount = (1000 + randomizer.Next(1000)) * 100;
                CreatePerson(person, contract,amount );

                var NumberOfAditionalDebts = (decimal)Math.Floor(5 / (double)(randomizer.Next(13) + 1));
                for (var j = 0; j < NumberOfAditionalDebts; j++)
                {
                    contract = contracts[randomizer.Next(contracts.Count)];
                    amount = (1000 + randomizer.Next(1000)) * 100;
                    Debts.CreateDebt(person, contract, amount);
                }

                /*TODO: Seed with transactions foreach debt
             *  each month 
             *      set saldo=0 if saldo<100
             *      add interest and fee (if debt>0)
             *      add payment 
             */
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AzureWebsite.Library.Inkasso
{
    public static class Transactions
    {
        public static List<Transaction> GetTransactions(Debt debt)
        {
            var personDebts = new List<Transaction>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, Date, Type, Amount FROM Transactions WHERE DebtId='{debt.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var transaction = new Transaction { Id = (Guid)reader["Id"], };

                            personDebts.Add(new Debt { Id = (Guid)reader["Id"], Contract = contract, Person = person });
                        }
                    }
                }
            }
            return personDebts;
        }

        public static List<Debt> GetDebts(Person person)
        {
            var personDebts = new List<Debt>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts WHERE PersonId ='{person.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person = new Person { Id = (Guid)reader["PersonId"], Name = (string)reader["PersonName"] };
                            var contract = new Contract { Id = (Guid)reader["ContractId"], Name = (string)reader["ContractName"] };
                            personDebts.Add(new Debt { Id = (Guid)reader["Id"], Contract = contract, Person = person });
                        }
                    }
                }
            }
            return personDebts;
        }
        public static Debt GetDebt(Guid Id)
        {
            Debt debt = null;
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts WHERE Id ='{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract { Id = (Guid)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (Guid)reader["PersonId"], Name = (string)reader["PersonName"] };
                            debt = new Debt { Id = (Guid)reader["Id"], Contract = contract, Person = person};
                        }
                    }
                }
            }
            if (debt == null) throw new Exception($"Debt with id {Id} not found");
            return debt;
        }

        public static void CreateDebt(Person person, Contract contract, decimal amount)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"INSERT INTO Debts (ContractId, PersonId) OUTPUT INSERTED.ID VALUES('{contract.Id}','{person.Id}')";
                Guid debtId = Guid.Empty;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    debtId = (Guid)command.ExecuteScalar();
                }

                sql = $"INSERT INTO Transactions (DebtId, Date, Type, Amount) VALUES('{debtId}','{DateTime.Now}','{TransactionType.SetBalance}','{amount}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        

        public static void SeedDebts()
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"TRUNCATE table Debts";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                sql = $"TRUNCATE table Transactions";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            var persons = PersonManager.GetPersons();
            var contracts = Contracts.GetContracts();

            Random randomizer = new Random();
            foreach(Person  person in persons)
            {
                double random = 5 / (double)(randomizer.Next(13) + 2);

                var NumberOfPersonDebts = (decimal)Math.Ceiling(random);
                Debug.WriteLine("Number of debts: " + NumberOfPersonDebts);
                for (var i =0; i<NumberOfPersonDebts; i++)
                {
                    var contract = contracts[randomizer.Next(contracts.Count)];
                    decimal amount = (1000 + randomizer.Next(1000)) * 100;
                    CreateDebt(person, contract, amount);
                }
            }
        }
    }
}

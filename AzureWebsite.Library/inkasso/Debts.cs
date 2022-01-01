using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace AzureWebsite.Library.Inkasso
{
    public static class Debts
    {
        
        public static List<Debt> GetDebts()
        {
            var personDebts = new List<Debt>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT DebtId, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var person = new Person { Id = (int)reader["PersonId"], Name = (string)reader["PersonName"] };
                            var contract = new Contract { Id = (int)reader["ContractId"], Name = (string)reader["ContractName"] };
                            personDebts.Add(new Debt { Id = (int)reader["DebtId"], Contract = contract, Person = person });
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
                String sql = $"SELECT DebtId, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts WHERE PersonId ='{person.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person = new Person { Id = (int)reader["PersonId"], Name = (string)reader["PersonName"] };
                            var contract = new Contract { Id = (int)reader["ContractId"], Name = (string)reader["ContractName"] };
                            personDebts.Add(new Debt { Id = (int)reader["DebtId"], Contract = contract, Person = person });
                        }
                    }
                }
            }
            return personDebts;
        }
        public static Debt GetDebt(int Id)
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
                            var contract = new Contract { Id = (int)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (int)reader["PersonId"], Name = (string)reader["PersonName"] };
                            debt = new Debt { Id = (int)reader["Id"], Contract = contract, Person = person};
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
                int debtId = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    debtId = (int)command.ExecuteScalar();
                }

                sql = $"INSERT INTO Transactions (DebtId, Date, Type, Amount) VALUES('{debtId}','{DateTime.Now}','{TransactionType.SetBalance}','{amount}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

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
            var debtsTransactions = new List<Transaction>();
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT TransactionId, TransactionType, TransactionDate, TransactionAmount, ContractId, ContractName, PersonId, PersonName, DebtId FROM ViewDebtsTransactions  WHERE DebtId ='{debt.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract { Id = (int)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (int)reader["PersonId"], Name = (string)reader["PersonName"] };
                            debt = new Debt { Id = (int)reader["DebtId"], Contract = contract, Person = person };

                            debtsTransactions.Add(new Transaction 
                            { 
                                Id = (int)reader["TransactionId"],
                                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), (string)reader["TransactionType"]),
                                TimeStamp = (DateTime)reader["TransactionDate"],
                                Amount= (decimal)reader["TransactionAmount"], 
                                Debt=debt
                            });
                        }
                    }
                }
            }
            return debtsTransactions;
        }
        public static Transaction GetTransaction(int Id)
        {
            Transaction transaction = null;
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT TransactionId, TransactionType, TransactionDate, TransactionAmount, ContractId, ContractName, PersonId, PersonName, DebtId  FROM ViewDebtsTransactions WHERE TransactionId = '{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract { Id = (int)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (int)reader["PersonId"], Name = (string)reader["PersonName"] };
                            var debt = new Debt { Id = (int)reader["DebtId"], Contract = contract, Person = person };
                            transaction = new Transaction
                            {
                                Id = (int)reader["TransactionId"],
                                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), (string)reader["TransactionType"]),
                                TimeStamp = (DateTime)reader["TransactionDate"],
                                Amount = (decimal)reader["TransactionAmount"],
                                Debt = debt
                            };
                        }
                    }
                }
            }
            if (transaction == null) throw new Exception($"Transaction with id {Id} not found");
            return transaction;
        }
        public static void CreateTransaction(Transaction transaction)
        {
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"INSERT INTO Transactions (Id, DebtId, Type, Date, Amount ) OUTPUT INSERTED.ID VALUES('{transaction.Id}',{transaction.Debt.Id}',{transaction.TransactionType}',{transaction.TimeStamp}',{transaction.Amount}')";
                Guid transactionId = Guid.Empty;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    transactionId = (Guid)command.ExecuteScalar();
                }
            }
        }
    }
}

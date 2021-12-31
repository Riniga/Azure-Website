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
                String sql = $"SELECT Id, Type, Date, Amount, ContractId, ContractName, PersonId, PersonName, DebtId FROM ViewDebtsTransactions  WHERE DebtId ='{debt.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract { Id = (Guid)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (Guid)reader["PersonId"], Name = (string)reader["PersonName"] };
                            debt = new Debt { Id = (Guid)reader["DebtId"], Contract = contract, Person = person };

                            debtsTransactions.Add(new Transaction 
                            { 
                                Id = (Guid)reader["Id"], 
                                TransactionType= (TransactionType)reader["Type"], 
                                TimeStamp = (DateTime)reader["Date"],
                                Amount= (decimal)reader["Amount"], 
                                Debt=debt
                            });
                        }
                    }
                }
            }
            return debtsTransactions;
        }
        public static Transaction GetTransaction(Guid Id)
        {
            Transaction transaction = null;
            using (SqlConnection connection = Database.GetConnection())
            {
                connection.Open();
                String sql = $"SELECT Id, Type, Date, Amount, ContractId, ContractName, PersonId, PersonName, DebtId FROM ViewDebtsTransactions WHERE Id = '{Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var contract = new Contract { Id = (Guid)reader["ContractId"], Name = (string)reader["ContractName"] };
                            var person = new Person { Id = (Guid)reader["PersonId"], Name = (string)reader["PersonName"] };
                            var debt = new Debt { Id = (Guid)reader["DebtId"], Contract = contract, Person = person };
                            transaction = new Transaction
                            {
                                Id = (Guid)reader["Id"],
                                TransactionType = (TransactionType)reader["Type"],
                                TimeStamp = (DateTime)reader["Date"],
                                Amount = (decimal)reader["Amount"],
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

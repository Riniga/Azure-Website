using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AzureWebsite.Library.Inkasso
{
    public static class Transactions
    {
        public static async Task<List<Transaction>> GetTransactionsAsync(Debt debt)
        {
            var debtsTransactions = new List<Transaction>();

            DataSet dataSet = await Database.GetDataSetAsync($"SELECT TransactionId, TransactionType, TransactionDate, TransactionAmount, ContractId, ContractName, PersonId, PersonName, DebtId FROM ViewDebtsTransactions  WHERE DebtId ='{debt.Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    var contract = new Contract { Id = (int)row["ContractId"], Name = (string)row["ContractName"] };
                    var person = new Person { Id = (int)row["PersonId"], Name = (string)row["PersonName"] };
                    debt = new Debt { Id = (int)row["DebtId"], Contract = contract, Person = person };

                    debtsTransactions.Add(new Transaction
                    {
                        Id = (int)row["TransactionId"],
                        TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), (string)row["TransactionType"]),
                        TimeStamp = (DateTime)row["TransactionDate"],
                        Amount = (decimal)row["TransactionAmount"],
                        Debt = debt
                    });
                }
            }

            return debtsTransactions;

        }

        public static async Task<Transaction> GetTransactionAsync(int Id)
        {
            Transaction transaction = null;
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT TransactionId, TransactionType, TransactionDate, TransactionAmount, ContractId, ContractName, PersonId, PersonName, DebtId  FROM ViewDebtsTransactions WHERE TransactionId = '{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    var contract = new Contract { Id = (int)row["ContractId"], Name = (string)row["ContractName"] };
                    var person = new Person { Id = (int)row["PersonId"], Name = (string)row["PersonName"] };
                    var debt = new Debt { Id = (int)row["DebtId"], Contract = contract, Person = person };
                    transaction = new Transaction
                    {
                        Id = (int)row["TransactionId"],
                        TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), (string)row["TransactionType"]),
                        TimeStamp = (DateTime)row["TransactionDate"],
                        Amount = (decimal)row["TransactionAmount"],
                        Debt = debt
                    };
                }
            }
            if (transaction == null) throw new Exception($"Transaction with id {Id} not found");
            return transaction;
        }
        
        public static async void CreateTransactionAsync(Debt debt, TransactionType transactionType, decimal amount)
        {
            await Database.ExecuteCommandAsync($"INSERT INTO Transactions (DebtId, Type, Date, Amount ) OUTPUT INSERTED.ID VALUES('{debt.Id}','{transactionType}','{DateTime.Now}','{amount}')");
        }
    }
}

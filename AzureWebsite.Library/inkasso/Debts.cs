using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AzureWebsite.Library.Inkasso
{
    public static class Debts
    {
        
        public static async Task<List<Debt>> GetDebtsAsync()
        {
            var personDebts = new List<Debt>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT DebtId, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {

                    var person = new Person { Id = (int)row["PersonId"], Name = (string)row["PersonName"] };
                    var contract = new Contract { Id = (int)row["ContractId"], Name = (string)row["ContractName"] };
                    personDebts.Add(new Debt { Id = (int)row["DebtId"], Contract = contract, Person = person });
                }
            }
            return personDebts;
        }
        public static async Task<List<Debt>> GetDebtsAsync(Person person)
        {
            var personDebts = new List<Debt>();
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT DebtId, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts WHERE PersonId ='{person.Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    person = new Person { Id = (int)row["PersonId"], Name = (string)row["PersonName"] };
                    var contract = new Contract { Id = (int)row["ContractId"], Name = (string)row["ContractName"] };
                    personDebts.Add(new Debt { Id = (int)row["DebtId"], Contract = contract, Person = person });
                }
            }
            return personDebts;
        }
        public static async Task<Debt> GetDebtAsync(int Id)
        {
            Debt debt = null;
            DataSet dataSet = await Database.GetDataSetAsync($"SELECT DebtId, ContractId, PersonId, ContractName, PersonName FROM ViewPersonDebts WHERE DebtId ='{Id}'");
            foreach (DataTable thisTable in dataSet.Tables)
            {
                foreach (DataRow row in thisTable.Rows)
                {
                    var contract = new Contract { Id = (int)row["ContractId"], Name = (string)row["ContractName"] };
                    var person = new Person { Id = (int)row["PersonId"], Name = (string)row["PersonName"] };
                    debt = new Debt { Id = (int)row["DebtId"], Contract = contract, Person = person };
                }
            }
            if (debt == null) throw new Exception($"Debt with id {Id} not found");
            return debt;
        }
        public static async void CreateDebtAsync(Person person, Contract contract, decimal amount)
        {
            var debtId = await Database.ExecuteCommandAsync($"INSERT INTO Debts (ContractId, PersonId) OUTPUT INSERTED.ID VALUES('{contract.Id}','{person.Id}')");
            await Database.ExecuteCommandAsync($"INSERT INTO Transactions (DebtId, Date, Type, Amount) VALUES('{debtId}','{DateTime.Now}','{TransactionType.SetBalance}','{amount}')");
        }
    }
}

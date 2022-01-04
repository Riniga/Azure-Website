using AzureWebsite.Library.Inkasso;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureWebsite.UnitTests.Inkasso
{
    public class Inkasso
    {
        const int NumberOfPersons = 10;
        const int NumberOfContracts = 10; 
        const string newPersonName = "Rickard Nisses-Gagnér";
        const string newContractName = "Skanska AB";
        Random randomizer = new Random();

        [SetUp]
        public void Setup()
        {
            Task.Run(async () => { 
                await Contracts.SeedContractsAsync(NumberOfContracts, File.ReadAllText("companies.txt"));
                await Persons.SeedPersonsAsync(NumberOfPersons, File.ReadAllText("persons.txt"));
            });
            Task.Delay(300);
        }

        
        [Test]
        public void ListAllPersons()
        {
            var personer = Persons.GetPersonsAsync().Result;
            Assert.GreaterOrEqual(personer.Count, NumberOfPersons);
            foreach (var person in personer)
            {
                Assert.IsNotEmpty(person.Name);
                Assert.AreNotEqual(person.Id, Guid.Empty);
            }
        }
        [Test]
        public void UpdatePerson()
        {
            var persons = Persons.GetPersonsAsync().Result;
            var person = persons[randomizer.Next(persons.Count)];
            person.Name = newPersonName;
            Assert.AreEqual(newPersonName,person.Name );
        }
        [Test]
        public void GetPersonById()
        {
            var persons = Persons.GetPersonsAsync().Result;
            var person1 = persons[randomizer.Next(persons.Count)];
            var person2 = Persons.GetPersonAsync(person1.Id).Result;
            Assert.AreEqual(person1.Name, person2.Name);
            Assert.AreEqual(person1.Id, person2.Id);
        }


        [Test]
        public void ListAllContracts()
        {
            var contracts = Contracts.GetContractsAsync().Result;
            Assert.AreEqual(NumberOfContracts, contracts.Count);
            foreach (var contract in contracts)
            {
                Assert.IsNotEmpty(contract.Name);
                Assert.AreNotEqual(contract.Id, Guid.Empty);
            }
        }

        [Test]
        public void GetContractById()
        {
            var contracts = Contracts.GetContractsAsync().Result;
            var contract1 = contracts[randomizer.Next(contracts.Count)];
            var contract2 = Contracts.GetContractAsync(contract1.Id).Result;
            Assert.AreEqual(contract1.Name, contract2.Name);
            Assert.AreEqual(contract1.Id, contract2.Id);
        }

        [Test]
        public void ListAllDebts()
        {
            var debts = Debts.GetDebtsAsync().Result;
            Assert.Greater(debts.Count, NumberOfPersons);
            foreach (var debt in debts)
            {
                Assert.AreNotEqual(debt.Id, 0);
                Assert.AreNotEqual(debt.Person.Id, 0);
                Assert.AreNotEqual(debt.Contract.Id, 0);
                Assert.IsNotEmpty(debt.Person.Name);
                Assert.IsNotEmpty(debt.Contract.Name);
            }
        }

        [Test]
        public void GetPersonsDebts()
        {
            var persons = Persons.GetPersonsAsync().Result;
            foreach (var person in persons)
            {
                var debts = Debts.GetDebtsAsync(person).Result;
                foreach (var debt in debts)
                {
                    Assert.AreNotEqual(debt.Id, 0);
                    Assert.AreEqual(debt.Person.Id, person.Id);
                    Assert.AreEqual(debt.Person.Name, person.Name);
                    Assert.AreNotEqual(debt.Contract.Id, 0);
                    Assert.IsNotEmpty(debt.Contract.Name);
                }
            }

        }
        [Test]
        public void GetDebtTransactions()
        {
            var debts = Task.Run(() => Debts.GetDebtsAsync()).Result;
            Assert.Greater(debts.Count, NumberOfPersons);

            var transactions= Transactions.GetTransactionsAsync(debts[randomizer.Next(debts.Count)]).Result;
            //foreach (var transaction in transactions)
            //{
            //    Assert.AreNotEqual(transaction.Id, 0);
            //    Assert.Greater((int)transaction.TransactionType, -1);
            //    Assert.Greater(transaction.Amount, 0);
            //    Assert.IsNotEmpty(transaction.Debt.Person.Name);
            //    Assert.IsNotEmpty(transaction.Debt.Contract.Name);
            //}
        }
        [Test]
        public void GetTransaction()
        {
            var debts = Debts.GetDebtsAsync().Result;
            var transactions = Transactions.GetTransactionsAsync(debts[randomizer.Next(debts.Count)]).Result;
            var transaction1 = transactions[randomizer.Next(transactions.Count)];
            var transaction2 = Transactions.GetTransactionAsync(transaction1.Id).Result;

            Assert.AreEqual(transaction1.Amount, transaction2.Amount);
            Assert.AreEqual(transaction1.Id, transaction2.Id);
            Assert.AreEqual(transaction1.TimeStamp, transaction2.TimeStamp);
            Assert.AreEqual(transaction1.TransactionType, transaction2.TransactionType);
            Assert.AreEqual(transaction1.Debt.Person.Name, transaction2.Debt.Person.Name);
        }


    }
}
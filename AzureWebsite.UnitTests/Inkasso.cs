using AzureWebsite.Library.Inkasso;
using NUnit.Framework;
using System;
using System.IO;

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
            PersonManager.SeedPersons(NumberOfPersons, File.ReadAllText("persons.txt"));
            Contracts.SeedContracts(NumberOfContracts, File.ReadAllText("companies.txt"));
            Debts.SeedDebts();
        }

        
        [Test]
        public void ListAllPersons()
        {
            var personer = PersonManager.GetPersons();
            Assert.AreEqual(NumberOfPersons, personer.Count);
            foreach (var person in personer)
            {
                Assert.IsNotEmpty(person.Name);
                Assert.AreNotEqual(person.Id, Guid.Empty);
            }
        }
        [Test]
        public void UpdatePerson()
        {
            var persons = PersonManager.GetPersons();
            var person = persons[randomizer.Next(persons.Count)];
            person.Name = newPersonName;
            Assert.AreEqual(newPersonName,person.Name );
        }
        [Test]
        public void GetPersonById()
        {
            var persons = PersonManager.GetPersons();
            var person1 = persons[randomizer.Next(persons.Count)];
            var person2 = PersonManager.GetPerson(person1.Id);
            Assert.AreEqual(person1.Name, person2.Name);
            Assert.AreEqual(person1.Id, person2.Id);
        }


        [Test]
        public void ListAllContracts()
        {
            var contracts = Contracts.GetContracts();
            Assert.AreEqual(NumberOfContracts, contracts.Count);
            foreach (var contract in contracts)
            {
                Assert.IsNotEmpty(contract.Name);
                Assert.AreNotEqual(contract.Id, Guid.Empty);
            }
        }
        [Test]
        public void UpdateContract()
        {
            var contracts = Contracts.GetContracts();
            var contract = contracts[randomizer.Next(contracts.Count)];
            contract.Name = newContractName;
            Assert.AreEqual(newContractName, contract.Name);
        }
        [Test]
        public void GetContractById()
        {
            var contracts = Contracts.GetContracts();
            var contract1 = contracts[randomizer.Next(contracts.Count)];
            var contract2 = Contracts.GetContract(contract1.Id);
            Assert.AreEqual(contract1.Name, contract2.Name);
            Assert.AreEqual(contract1.Id, contract2.Id);
        }

        [Test]
        public void ListAllDebts()
        {
            var debts = Debts.GetDebts();
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
            var persons = PersonManager.GetPersons();
            foreach (var person in persons)
            {
                var debts = Debts.GetDebts(person);
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
            var debts = Debts.GetDebts();
            var transactions = Transactions.GetTransactions(debts[randomizer.Next(debts.Count)]);
            Assert.Greater(debts.Count, NumberOfPersons);
            foreach (var transaction in transactions)
            {
                Assert.AreNotEqual(transaction.Id, 0);
                Assert.Greater((int)transaction.TransactionType, -1);
                Assert.Greater(transaction.Amount, 0);
                Assert.IsNotEmpty(transaction.Debt.Person.Name);
                Assert.IsNotEmpty(transaction.Debt.Contract.Name);
            }
        }

        [Test]
        public void GetTransaction()
        {
            var debts = Debts.GetDebts();
            var transactions = Transactions.GetTransactions(debts[randomizer.Next(debts.Count)]);
            var transaction1 = transactions[randomizer.Next(transactions.Count)];
            var transaction2 = Transactions.GetTransaction(transaction1.Id);

            Assert.AreEqual(transaction1.Amount, transaction2.Amount);
            Assert.AreEqual(transaction1.Id, transaction2.Id);
            Assert.AreEqual(transaction1.TimeStamp, transaction2.TimeStamp);
            Assert.AreEqual(transaction1.TransactionType, transaction2.TransactionType);
            Assert.AreEqual(transaction1.Debt.Person.Name, transaction2.Debt.Person.Name);
        }


    }
}
CREATE VIEW [dbo].[ViewDebtsTransactions]
AS SELECT 
	dbo.Transactions.Id as TransactionId,
	dbo.Transactions.Type as TransactionType, 
	dbo.Transactions.Date as TransactionDate, 
	dbo.Transactions.Amount as TransactionAmount,    
	dbo.Debts.Id as DebtId,
	dbo.Contracts.Id as ContractId,
	dbo.Contracts.Name as ContractName,
	dbo.Persons.Id as PersonId,
	dbo.Persons.Name as PersonName
FROM dbo.Transactions
INNER JOIN dbo.Debts ON dbo.Transactions.DebtId = dbo.Debts.Id
INNER JOIN dbo.Contracts ON dbo.Debts.ContractId = dbo.Contracts.Id
INNER JOIN dbo.Persons ON dbo.Debts.PersonId = dbo.Persons.Id

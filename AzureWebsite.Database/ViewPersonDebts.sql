CREATE VIEW [dbo].[ViewPersonDebts]
	AS SELECT dbo.Debts.Id as DebtId, dbo.Contracts.Id as ContractId, dbo.Persons.Id as PersonId, dbo.Contracts.Name as ContractName, dbo.Persons.Name as PersonName 
	FROM dbo.Persons  INNER JOIN dbo.Debts 
	ON dbo.Persons.Id = dbo.Debts.PersonId 
	INNER JOIN dbo.Contracts ON dbo.Debts.ContractId = dbo.Contracts.Id
CREATE TABLE [dbo].[Debts](
	[Id] Int NOT NULL PRIMARY KEY IDENTITY,
	[ContractId] INT NOT NULL,
	[PersonId] INT NOT NULL, 
    CONSTRAINT [FK_Debts_Person] FOREIGN KEY ([PersonId]) REFERENCES [Persons]([Id]), 
    CONSTRAINT [FK_Debts_Contracts] FOREIGN KEY ([ContractId]) REFERENCES [Contracts]([Id])
) ON [PRIMARY]
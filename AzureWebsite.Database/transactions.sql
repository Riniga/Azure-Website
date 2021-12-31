CREATE TABLE [dbo].[Transactions](
	[Id] Int NOT NULL PRIMARY KEY IDENTITY,
	[DebtId] INT NOT NULL,
	[Date] [datetime] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL, 
    CONSTRAINT [FK_Transactions_Debts] FOREIGN KEY ([DebtId]) REFERENCES [Debts]([Id]),
) ON [PRIMARY]
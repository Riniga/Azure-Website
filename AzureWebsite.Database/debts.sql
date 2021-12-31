USE [Inkasso]
GO

/****** Object:  Table [dbo].[Debts]    Script Date: 2021-12-31 09:20:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Debts](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ContractId] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Debts] ADD  CONSTRAINT [DF_Debts_Id]  DEFAULT (newid()) FOR [Id]
GO


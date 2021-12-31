USE [Inkasso]
GO

/****** Object:  Table [dbo].[Transactions]    Script Date: 2021-12-31 09:21:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transactions](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[DebtId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NULL,
	[Type] [nvarchar](50) NULL,
	[Amount] [decimal](18, 0) NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_Id]  DEFAULT (newid()) FOR [Id]
GO


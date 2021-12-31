USE [Inkasso]
GO

/****** Object:  Table [dbo].[Contracts]    Script Date: 2021-12-31 09:20:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contracts](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](64) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Contracts] ADD  CONSTRAINT [DF_Contracts_Id]  DEFAULT (newid()) FOR [Id]
GO


USE [Inkasso]
GO

/****** Object:  Table [dbo].[Persons]    Script Date: 2021-12-31 09:19:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Persons](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Name] [nvarchar](64) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF_Persons_Id]  DEFAULT (newid()) FOR [Id]
GO


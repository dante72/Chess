CREATE DATABASE [Chess]
GO

USE [Chess]
GO

/****** Object:  Table [dbo].[Exercises]    Script Date: 26.12.2021 21:16:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Exercises](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[Moves] [int] NOT NULL
	)
GO



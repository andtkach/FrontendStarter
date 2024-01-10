USE [master]
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'PeopleDB')
  BEGIN
    CREATE DATABASE [PeopleDB]


  END
    GO
       USE [PeopleDB]
    GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='People' and xtype='U')
BEGIN
    CREATE TABLE People (
        Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
		Name varchar(255) NOT NULL,
		Email varchar(255) NOT NULL,
		Address varchar(1023),
		Age int
    )
END

--USE master
--GO
--DROP DATABASE uhrenWelt
--GO

CREATE DATABASE uhrenWelt
GO

USE uhrenWelt
GO

CREATE TABLE [dbo].[OrderLine](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Amount] [int] NOT NULL,
    [NetUnitPrice] [decimal](19, 2) NOT NULL,
    [TaxRate] [decimal](9, 2) NOT NULL,
)

CREATE TABLE [dbo].[Product](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [ProductName] [nvarchar](300) NOT NULL,
    [NetUnitPrice] [decimal](19, 2) NOT NULL,
    [ImagePath] [nvarchar](50) NULL,
    [Description] [nvarchar](max) NOT NULL,
    [ManufacturerId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
)

CREATE TABLE [dbo].[Category](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [TaxRate] [decimal](9, 2) NOT NULL
)

CREATE TABLE [dbo].[Order](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [PriceTotal] [decimal](28, 2) NOT NULL,
    [DateOrdered] [datetime] NULL,
    [Street] [nvarchar](50) NOT NULL,
    [Zip] [nvarchar](50) NOT NULL,
    [City] [nvarchar](200) NOT NULL,
    [FirstName] [nvarchar](150) NOT NULL,
    [LastName] [nvarchar](150) NOT NULL,
    [VoucherId] [int] NULL,
    [PriceToPay] [decimal](28, 2) NOT NULL
)

CREATE TABLE [dbo].[Manufacturer](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](200) NOT NULL
)

CREATE TABLE [dbo].[Customer](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](50) NULL,
    [FirstName] [nvarchar](150) NOT NULL,
    [LastName] [nvarchar](150) NOT NULL,
    [Email] [nvarchar](350) NOT NULL,
    [Street] [nvarchar](150) NOT NULL,
    [Zip] [nvarchar](50) NOT NULL,
    [City] [nvarchar](200) NOT NULL,
    [PwHash] [binary](32) NOT NULL,
    [Salt] [binary](32) NOT NULL
)

ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturer] ([Id])
GO
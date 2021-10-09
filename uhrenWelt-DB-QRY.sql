USE master
GO
DROP DATABASE uhrenWelt
GO

CREATE DATABASE uhrenWelt
GO

USE uhrenWelt
GO

CREATE TABLE [dbo].[OrderLine](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Amount] [int] NOT NULL,
    [NetUnitPrice] [decimal](12, 2) NOT NULL,
    [TaxRate] [decimal](9, 2) NOT NULL,
)

CREATE TABLE [dbo].[Product](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [ProductName] [varchar](300) NOT NULL,
    [NetUnitPrice] [decimal](12, 2) NOT NULL,
    [ImagePath] [varchar](50) NULL,
    [Description] [varchar](max) NOT NULL,
    [ManufacturerId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
)

CREATE TABLE [dbo].[Category](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] [varchar](200) NOT NULL,
    [TaxRate] [decimal](9, 2) NOT NULL
)

CREATE TABLE [dbo].[Order](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [PriceTotal] [decimal](12, 2) NOT NULL,
    [DateOrdered] [datetime] NULL,
    [Street] [varchar](150) NOT NULL,
    [Zip] [varchar](5) NOT NULL,
    [City] [varchar](200) NOT NULL,
    [FirstName] [varchar](150) NOT NULL,
    [LastName] [varchar](150) NOT NULL,
    [VoucherId] [int] NULL,
    [PriceToPay] [decimal](12, 2) NOT NULL
)

CREATE TABLE [dbo].[Manufacturer](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Name] [varchar](200) NOT NULL
)

CREATE TABLE [dbo].[Customer](
    [Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [Title] [varchar](150) NULL,
    [FirstName] [varchar](150) NOT NULL,
    [LastName] [varchar](150) NOT NULL,
    [Email] [varchar](250) NOT NULL,
    [Street] [varchar](150) NOT NULL,
    [Zip] [varchar](5) NOT NULL,
    [City] [varchar](200) NOT NULL,
    [PwHash] [varchar](200) NOT NULL,
    [Salt] [varchar](200) NOT NULL
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

insert into Category ([Name], TaxRate)
values
('Uhr', '20')
go

insert into Manufacturer([Name])
values
('Rolex')
go

insert into Manufacturer([Name])
values
('Breitling')
go

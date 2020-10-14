--Database
create database thinkbridge

use thinkbridge

--Table
CREATE TABLE [dbo].[Inventory](
	[InventoryID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[ProductDescription] [varchar](500) NOT NULL,
	[ProductPrice] [bigint] NOT NULL,
	[ProductPhoto] [varbinary](max) NULL,
	[IsActive] [bit] NOT NULL,
	[AddedOn] [datetimeoffset](7) NULL,
	[FileExtension] [varchar](50) NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[InventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


--SP's
CREATE PROCEDURE usp_GetInventory
@InventoryID BIGINT,
@recordCount BIGINT,
@errCode SMALLINT OUTPUT  
AS
SET @errCode = 200
BEGIN
	IF (@recordCount = 0)
	BEGIN
		SELECT * From Inventory Where IsActive = 1 Order By AddedON
	END
	ELSE IF (@recordCount = -1)
	BEGIN
		SELECT * From Inventory Where InventoryID = @InventoryID
	END
END

CREATE PROCEDURE usp_SaveInventory
@InventoryID BIGINT,
@productName VARCHAR(50),
@productDescription VARCHAR(500),
@productPrice BIGINT,
@productPhoto VARBINARY(MAX),
@IsActive BIT,
@fileExtension VARCHAR(50),
@errCode SMALLINT OUTPUT  
AS
SET @errCode = 200
BEGIN
	IF (@IsActive = 0)
	BEGIN
		UPDATE Inventory SET IsActive = 0 WHERE InventoryID = @InventoryID
	END
	ELSE 
	BEGIN
		INSERT INTO Inventory (ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive, FileExtension, AddedOn )
		VALUES (@productName, @productDescription, @productPrice, @productPhoto, 1, @fileExtension, GETUTCDATE())
	END
END

/*
insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('freeze','freezzzzz',15000,null,1)

insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('washing machine','freezzzzz',15000,null,1)

insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('iron','freezzzzz',15000,null,1)

insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('cooler','freezzzzz',15000,null,1)
insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('ac','freezzzzz',15000,null,1)
insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('computer','freezzzzz',15000,null,1)
insert into inventory(ProductName, ProductDescription, ProductPrice, ProductPhoto, IsActive) values('laptop','freezzzzz',15000,null,1)
*/


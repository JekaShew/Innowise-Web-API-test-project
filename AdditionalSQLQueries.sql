--Creating database
USE [Fridge]
GO
/****** Object:  Table [dbo].[FridgeModels]    Script Date: 10/17/2022 7:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FridgeModels](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK_FridgeModels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FridgeProducts]    Script Date: 10/17/2022 7:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FridgeProducts](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[FridgeId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_FridgeProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fridges]    Script Date: 10/17/2022 7:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fridges](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[OwnerName] [nvarchar](max) NULL,
	[FridgeModelId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Fridges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/17/2022 7:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[DefaultQuantity] [int] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/17/2022 7:57:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[FridgeProducts]  WITH CHECK ADD  CONSTRAINT [FK_FridgeProducts_Fridges_FridgeId] FOREIGN KEY([FridgeId])
REFERENCES [dbo].[Fridges] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FridgeProducts] CHECK CONSTRAINT [FK_FridgeProducts_Fridges_FridgeId]
GO
ALTER TABLE [dbo].[FridgeProducts]  WITH CHECK ADD  CONSTRAINT [FK_FridgeProducts_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FridgeProducts] CHECK CONSTRAINT [FK_FridgeProducts_Products_ProductId]
GO
ALTER TABLE [dbo].[Fridges]  WITH CHECK ADD  CONSTRAINT [FK_Fridges_FridgeModels_FridgeModelId] FOREIGN KEY([FridgeModelId])
REFERENCES [dbo].[FridgeModels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Fridges] CHECK CONSTRAINT [FK_Fridges_FridgeModels_FridgeModelId]
GO


-- stored procedure
ALTER PROCEDURE SelectFridgeProductWithoutQuantity
AS
BEGIN
	SELECT * FROM FridgeProducts
	WHERE Quantity = 0
END	

USE Fridge

SELECT * FROM products
--TASK 1 —делать выборку продуктов по холодильникам , модель которых начинаетс€ на ј

SELECT Products.Title,Fridges.Title
FROM Fridges
INNER JOIN FridgeModels on Fridges.FridgeModelId = FridgeModels.Id
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
INNER JOIN Products on Products.Id = FridgeProducts.ProductId
WHERE FridgeModels.Title LIKE 'ј%'
GROUP BY Fridges.Title, Products.Title


--TASK 2 —делать выборку холодильников, в которых есть продукты в количестве, меньше чем количество по умолчанию

SELECT Fridges.Title,Fridges.OwnerName 
FROM Fridges
WHERE EXISTS(
				SELECT * 
				FROM FridgeProducts
				INNER JOIN Products on Products.Id = FridgeProducts.ProductId
				WHERE FridgeProducts.Quantity < Products.DefaultQuantity and FridgeProducts.FridgeId = Fridges.Id
			)

--TASK 3 ¬ каком году выпустили холодильник с наибольшей вместимостью(сумма всех продуктов больше всего)

SELECT FridgeModels.Year
FROM Fridges
INNER JOIN FridgeModels on FridgeModels.Id = Fridges.FridgeModelId
WHERE Fridges.Id = (
	SELECT TOP 1 FridgeIdSum.ID 
	FROM
	(
		SELECT FridgeProducts.FridgeId AS [ID], SUM(FridgeProducts.Quantity) AS [SUM]
		FROM FridgeProducts
		GROUP BY FridgeProducts.FridgeId
		
	) AS FridgeIdSum
	ORDER BY FridgeIdSum.SUM DESC
)

-- Queries that helps to find the correct answer
SELECT Fridges.Title, FridgeModels.Title,FridgeModels.Year 
FROM Fridges 
INNER JOIN FridgeModels on FridgeModels.Id = Fridges.FridgeModelId
WHERE Fridges.Id ='377D489C-CA1D-44CC-AC08-A55FF4417D48'

SELECT FridgeProducts.FridgeId AS [ID], SUM(FridgeProducts.Quantity) AS [SUM]
FROM FridgeProducts
GROUP BY FridgeProducts.FridgeId
ORDER BY [SUM] DESC

--TASK 4 ¬ыбрать все продукты и им€ владельца из холодильника, в котором больше всего наименований продуктов. »менно наименований, не количества

SELECT Products.Title,Fridges.OwnerName
FROM Products
INNER JOIN FridgeProducts on FridgeProducts.ProductId = Products.Id
INNER JOIN Fridges on Fridges.Id = FridgeProducts.FridgeId
WHERE Fridges.Id = 
	(
		SELECT TOP 1 FridgesCount.ID
		FROM 
		(
			SELECT FridgeProducts.FridgeId as [ID], COUNT(*) as [COUNT]
			FROM FridgeProducts
			GROUP BY FridgeProducts.FridgeId
		) as FridgesCount
		ORDER BY FridgesCount.[COUNT] DESC
	)

-- Queries that helps to find the correct answer

SELECT FridgeProducts.FridgeId as [ID], COUNT(*) as [COUNT]
FROM FridgeProducts
GROUP BY FridgeProducts.FridgeId

SELECT Fridges.Title,Fridges.OwnerName, FridgeModels.Title,FridgeModels.Year 
FROM Fridges 
INNER JOIN FridgeModels on FridgeModels.Id = Fridges.FridgeModelId
WHERE Fridges.Id ='5CF8CD9C-FEAC-43C9-A7C8-1C484CA1757F'

--Ќабор запросов 2
--TASK 5 ¬ывести все продукты дл€ холодильника в id 2( или выбрать определенный Guid)

SELECT Products.Title 
FROM Products
INNER JOIN FridgeProducts on Products.Id = FridgeProducts.ProductId
WHERE FridgeProducts.FridgeId = 'C7A5DD50-8272-4A3B-9561-0CA0EB1DDC3C'

--TASK 6 ¬ывести все продукты дл€ всех холодильников

SELECT Products.Title, Fridges.Title
FROM Fridges
LEFT JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
LEFT JOIN Products on Products.Id = FridgeProducts.ProductId
GROUP BY Fridges.Title,Products.Title

--TASK 7 ¬ывести список холодильников и сумму всех продуктов дл€ каждого из них

SELECT Fridges.Title, SUM(FridgeProducts.Quantity)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
GROUP BY Fridges.Title

--TASK 8 ¬ывести им€ холодильника, название и год модели, а также количество продуктов дл€ каждого из них

SELECT Fridges.Title,FridgeModels.Title,FridgeModels.Year, SUM(FridgeProducts.Quantity)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
INNER JOIN FridgeModels on Fridges.FridgeModelId = FridgeModels.Id
GROUP BY Fridges.Title,FridgeModels.Title,FridgeModels.Year

--TASK 9 ¬ывести список холодильников, где содержатьс€ продукты, количество которых больше чем количество по умолчанию

SELECT Fridges.Title
FROM Fridges 
WHERE EXISTS(
				SELECT * 
				FROM FridgeProducts
				INNER JOIN Products on Products.Id = FridgeProducts.ProductId
				WHERE FridgeProducts.Quantity > Products.DefaultQuantity and FridgeProducts.FridgeId = Fridges.Id
			)
--TASK 10 ¬ывести список холодильников и дл€ каждого холодильника количество наименований продуктов, количество которых юоьше чем количесвто по умолчанию

SELECT Fridges.Title, COUNT(FridgeProducts.ProductId)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
WHERE FridgeProducts.ProductId in (
				SELECT FridgeProducts.ProductId 
				FROM FridgeProducts
				INNER JOIN Products on Products.Id = FridgeProducts.ProductId
				WHERE FridgeProducts.Quantity > Products.DefaultQuantity and FridgeProducts.FridgeId = Fridges.Id
			)
GROUP BY Fridges.Title

-- Queries that helps to find the correct answer
SELECT Fridges.Title, COUNT(FridgeProducts.ProductId)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
GROUP BY Fridges.Title
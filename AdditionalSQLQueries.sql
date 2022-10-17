ALTER PROCEDURE SelectFridgeProductWithoutQuantity
AS
BEGIN
	SELECT * FROM FridgeProducts
	WHERE Quantity = 0
END	

USE Fridge

SELECT * FROM Fridges
--TASK 1 ������� ������� ��������� �� ������������� , ������ ������� ���������� �� �

SELECT Products.Title,Fridges.Title
FROM Fridges
INNER JOIN FridgeModels on Fridges.FridgeModelId = FridgeModels.Id
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
INNER JOIN Products on Products.Id = FridgeProducts.ProductId
WHERE FridgeModels.Title LIKE '�%'
GROUP BY Fridges.Title, Products.Title


--TASK 2 ������� ������� �������������, � ������� ���� �������� � ����������, ������ ��� ���������� �� ���������

SELECT Fridges.Title,Fridges.OwnerName 
FROM Fridges
WHERE EXISTS(
				SELECT * 
				FROM FridgeProducts
				INNER JOIN Products on Products.Id = FridgeProducts.ProductId
				WHERE FridgeProducts.Quantity < Products.DefaultQuantity and FridgeProducts.FridgeId = Fridges.Id
			)

--TASK 3 � ����� ���� ��������� ����������� � ���������� ������������(����� ���� ��������� ������ �����)

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
SELECT Fridges.Title, FridgeModels.Title,FridgeModels.Year FROM Fridges INNER JOIN FridgeModels on FridgeModels.Id = Fridges.FridgeModelId
WHERE Fridges.Id ='377D489C-CA1D-44CC-AC08-A55FF4417D48'

SELECT FridgeProducts.FridgeId AS [ID], SUM(FridgeProducts.Quantity) AS [SUM]
FROM FridgeProducts
GROUP BY FridgeProducts.FridgeId
ORDER BY [SUM] DESC


--SELECT FridgeModels.Year
--FROM FridgeModels
--WHERE EXISTS(
--	SELECT Fridges.Id
--	FROM Fridges,
--	(
--		SELECT Fridges.Id as FridgeId, SUM(FridgeProducts.Quantity) as SumFridgeProducts
--		FROM FridgeProducts
--		INNER JOIN Fridges on Fridges.Id = FridgeProducts.FridgeId
--		GROUP BY Fridges.Id
--	) as FridgeProductsSums
--	WHERE 
--		Fridges.FridgeModelId = FridgeModels.Id and Fridges.Id = FridgeProductsSums.FridgeId	
--		and FridgeProductsSums.SumFridgeProducts = (
--			SELECT MAX(FridgeProductsSums.SumFridgeProducts)
--			FROM FridgeProductsSums
--			WHERE Fridges.Id = FridgeProductsSums.FridgeId
--		)												
--)

--TASK 4 ������� ��� �������� � ��� ��������� �� ������������, � ������� ������ ����� ������������ ���������. ������ ������������, �� ����������

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

SELECT Fridges.Title,Fridges.OwnerName, FridgeModels.Title,FridgeModels.Year FROM Fridges INNER JOIN FridgeModels on FridgeModels.Id = Fridges.FridgeModelId
WHERE Fridges.Id ='5CF8CD9C-FEAC-43C9-A7C8-1C484CA1757F'

--����� �������� 2
--TASK 5 ������� ��� �������� ��� ������������ � id 2( ��� ������� ������������ Guid)

SELECT Products.Title 
FROM Products
INNER JOIN FridgeProducts on Products.Id = FridgeProducts.ProductId
WHERE FridgeProducts.FridgeId = 'C7A5DD50-8272-4A3B-9561-0CA0EB1DDC3C'

--TASK 6 ������� ��� �������� ��� ���� �������������

SELECT Products.Title, Fridges.Title
FROM Fridges
LEFT JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
LEFT JOIN Products on Products.Id = FridgeProducts.ProductId
GROUP BY Fridges.Title,Products.Title

--TASK 7 ������� ������ ������������� � ����� ���� ��������� ��� ������� �� ���

SELECT Fridges.Title, SUM(FridgeProducts.Quantity)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
GROUP BY Fridges.Title

--TASK 8 ������� ��� ������������, �������� � ��� ������, � ����� ���������� ��������� ��� ������� �� ���

SELECT Fridges.Title,FridgeModels.Title,FridgeModels.Year, SUM(FridgeProducts.Quantity)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
INNER JOIN FridgeModels on Fridges.FridgeModelId = FridgeModels.Id
GROUP BY Fridges.Title,FridgeModels.Title,FridgeModels.Year

--TASK 9 ������� ������ �������������, ��� ����������� ��������, ���������� ������� ������ ��� ���������� �� ���������

SELECT Fridges.Title
FROM Fridges 
WHERE EXISTS(
				SELECT * 
				FROM FridgeProducts
				INNER JOIN Products on Products.Id = FridgeProducts.ProductId
				WHERE FridgeProducts.Quantity > Products.DefaultQuantity and FridgeProducts.FridgeId = Fridges.Id
			)
--TASK 10 ������� ������ ������������� � ��� ������� ������������ ���������� ������������ ���������, ���������� ������� ����� ��� ���������� �� ���������

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

SELECT Fridges.Title, COUNT(FridgeProducts.ProductId)
FROM Fridges
INNER JOIN FridgeProducts on Fridges.Id = FridgeProducts.FridgeId
GROUP BY Fridges.Title
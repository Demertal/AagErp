USE Store
GO

CREATE TRIGGER Products_INSERT
ON Products
AFTER INSERT
AS
INSERT INTO CountProducts(IdProduct, IdStore) SELECT IdProduct, IdStore FROM (SELECT INSERTED.Id as IdProduct, 1 as commonId FROM INSERTED) as tempA 
	INNER JOIN (SELECT Stores.Id as IdStore, 1 as commonId FROM Stores) as tempB on tempA.commonId = tempB.commonId
GO

CREATE TRIGGER Stores_INSERT
ON Stores
AFTER INSERT
AS
INSERT INTO CountProducts(IdProduct, IdStore) SELECT IdProduct, IdStore FROM (SELECT INSERTED.Id as IdStore, 1 as commonId FROM INSERTED) as tempA 
	INNER JOIN (SELECT Products.Id as IdProduct, 1 as commonId FROM Products) as tempB on tempA.commonId = tempB.commonId
GO

CREATE TRIGGER Groups_INSERT
ON Groups
AFTER INSERT
AS
INSERT INTO Stores(Title) SELECT Title FROM INSERTED WHERE IdParentGroup IS NULL
GO

CREATE TRIGGER Groups_UPDATE
ON Groups
AFTER UPDATE
AS
UPDATE Stores Set Title = INSERTED.Title FROM INSERTED INNER JOIN DELETED ON INSERTED.Id = DELETED.Id WHERE INSERTED.IdParentGroup IS NULL;
INSERT INTO Stores(Title) SELECT INSERTED.Title FROM INSERTED INNER JOIN DELETED ON INSERTED.Id = DELETED.Id WHERE INSERTED.IdParentGroup IS NULL AND DELETED.IdParentGroup IS NOT NULL
GO

CREATE TRIGGER Groups_DELETE
ON Groups
AFTER DELETE
AS
DELETE Stores FROM Stores INNER JOIN DELETED ON  DELETED.Title = Stores.Title
GO 

CREATE TRIGGER WarrantyPeriods_UPDATE
ON WarrantyPeriods
AFTER UPDATE
AS
SELECT * FROM DELETED WHERE Period = 'Нет';
if @@ROWCOUNT != 0
BEGIN
RAISERROR('Нельзя изменять гарантийный период: "Нет"', 16, 1)
ROLLBACK TRAN
END
GO 

CREATE TRIGGER WarrantyPeriods_DELETE
ON WarrantyPeriods
AFTER DELETE
AS
SELECT * FROM DELETED WHERE Period = 'Нет';
if @@ROWCOUNT != 0
BEGIN
RAISERROR('Нельзя удалять гарантийный период: "Нет"', 16, 1)
ROLLBACK TRAN
END
GO

CREATE TRIGGER ExchangeRates_UPDATE
ON ExchangeRates
AFTER UPDATE
AS
SELECT * FROM DELETED WHERE Title = 'ГРН' or Title = 'USD';
if @@ROWCOUNT != 0
BEGIN
RAISERROR('Нельзя изменять валюты: "ГРН" и "USD"', 16, 1)
ROLLBACK TRAN
END
GO 

CREATE TRIGGER ExchangeRates_DELETE
ON ExchangeRates
AFTER DELETE
AS
SELECT * FROM DELETED WHERE Title = 'ГРН' or Title = 'USD';
if @@ROWCOUNT != 0
BEGIN
RAISERROR('Нельзя удалять валюты: "ГРН" и "USD"', 16, 1)
ROLLBACK TRAN
END
GO

CREATE TRIGGER PurchaseReports_INSERT
ON PurchaseReports
INSTEAD OF INSERT
AS
INSERT INTO PurchaseReports(DataOrder, Сourse,	IdStore, IdSupplier) SELECT GETDATE(), INSERTED.Сourse, INSERTED.IdStore, INSERTED.IdSupplier FROM INSERTED
SELECT Id FROM PurchaseReports WHERE @@ROWCOUNT > 0 and Id = scope_identity(); 
GO

CREATE TRIGGER PurchaseInfos_INSERT
ON PurchaseInfos
AFTER INSERT
AS
	DECLARE @insertProdFirst TABLE (PurchasePrice MONEY, Сourse MONEY, IdExchangeRate INT, IdProduct INT)
	DECLARE @insertProd TABLE (PurchasePrice MONEY, IdExchangeRate INT, IdProduct INT)
	DECLARE @insertCountFirst TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insertCount TABLE (Count INT, IdProduct INT, IdStore INT)
	INSERT INTO @insertProdFirst(PurchasePrice, Сourse, IdExchangeRate, IdProduct) SELECT
		i.PurchasePrice, CAST(	CASE 
			WHEN (SELECT Title FROM ExchangeRates WHERE ExchangeRates.Id = i.IdExchangeRate) = 'USD'
			THEN (SELECT p.Сourse FROM PurchaseReports as p WHERE p.Id = i.IdPurchaseReport)
			ELSE 1
			END AS MONEY) AS Сourse, i.IdExchangeRate, i.IdProduct
		FROM inserted AS i 
	INSERT INTO @insertProd(PurchasePrice, IdProduct) SELECT MAX(inpf1.PurchasePrice * inpf1.Сourse), inpf1.IdProduct	FROM @insertProdFirst as inpf1 GROUP BY inpf1.IdProduct;
	UPDATE @insertProd SET IdExchangeRate = inpf2.IdExchangeRate, PurchasePrice=inpf2.PurchasePrice  FROM @insertProd as inp1 INNER JOIN @insertProdFirst as inpf2 ON inp1.IdProduct = inpf2.IdProduct
	WHERE inp1.PurchasePrice = inpf2.PurchasePrice*inpf2.Сourse
	UPDATE Products SET PurchasePrice = inp.PurchasePrice, IdExchangeRate = inp.IdExchangeRate FROM @insertProd as inp WHERE Id = inp.IdProduct
	INSERT INTO @insertCountFirst(Count, IdProduct, IdStore) SELECT ins.COUNT, ins.IdProduct, 
		CAST((SELECT IdStore FROM PurchaseReports INNER JOIN INSERTED on PurchaseReports.Id = INSERTED.IdPurchaseReport) AS INT) as idstore FROM INSERTED as ins
	INSERT INTO @insertCount(Count, IdProduct, IdStore) SELECT SUM(i.COUNT), i.IdProduct, i.IdStore FROM @insertCountFirst as i GROUP BY i.IdProduct, i.IdStore
	UPDATE CountProducts SET CountProducts.Count = CountProducts.Count + ic.Count FROM @insertCount as ic
		WHERE CountProducts.IdProduct = ic.IdProduct and CountProducts.IdStore = ic.IdStore
GO

CREATE TRIGGER SerialNumbers_INSERT
ON SerialNumbers
INSTEAD OF INSERT
AS
INSERT INTO SerialNumbers(Value, PurchaseDate,	IdProduct, IdSupplier) SELECT INSERTED.Value,  GETDATE(), INSERTED.IdProduct, INSERTED.IdSupplier FROM INSERTED 
SELECT Id FROM SerialNumbers WHERE @@ROWCOUNT > 0 and Id = scope_identity(); 
GO

CREATE TRIGGER SalesReports_INSERT
ON SalesReports
INSTEAD OF INSERT
AS
INSERT INTO SalesReports(DataSales, IdStore) SELECT GETDATE(), INSERTED.IdStore FROM INSERTED
SELECT Id FROM SalesReports WHERE @@ROWCOUNT > 0 and Id = scope_identity();  
GO

CREATE TRIGGER SalesInfos_INSERT
ON SalesInfos
AFTER INSERT
AS
DECLARE @insertFirst TABLE (Count INT, IdProduct INT, IdStore INT)
DECLARE @insert TABLE (Count INT, IdProduct INT, IdStore INT)
	INSERT INTO @insertFirst(Count, IdProduct, IdStore) SELECT i.Count, i.IdProduct, 
	(SELECT IdStore FROM SalesReports WHERE SalesReports.Id = i.IdSalesReport) FROM INSERTED as i
	INSERT INTO @insert(Count, IdProduct, IdStore) SELECT SUM(i.Count), i.IdProduct, i.IdStore FROM @insertFirst as i GROUP BY i.IdProduct, i.IdStore
SELECT * FROM @insert as i INNER JOIN CountProducts as ct ON i.IdProduct = ct.IdProduct and i.IdStore = ct.IdStore WHERE i.COUNT > ct.COUNT
if @@ROWCOUNT != 0
BEGIN
RAISERROR('Кол-во товара в некоторых продажах превышает имеющееся кол-во на складе', 16, 1)
ROLLBACK TRAN
END
UPDATE CountProducts SET CountProducts.Count = CountProducts.Count - i.Count FROM @insert as i 
	WHERE CountProducts.IdProduct = i.IdProduct and CountProducts.IdStore = i.IdStore
UPDATE SerialNumbers SET SerialNumbers.SelleDate = GETDATE() FROM INSERTED WHERE SerialNumbers.Id = INSERTED.IdSerialNumber
GO   

CREATE TRIGGER RevaluationProducts_INSERT
ON RevaluationProducts
INSTEAD OF INSERT
AS
DECLARE @insert TABLE (SalesPrice MONEY, IdProduct INT)
INSERT INTO @insert(SalesPrice, IdProduct) SELECT MAX(NewSalesPrice), IdProduct FROM INSERTED GROUP BY INSERTED.IdProduct;
INSERT INTO RevaluationProducts(Date, IdProduct, OldSalesPrice, NewSalesPrice) SELECT GETDATE(), INSERTED.IdProduct, INSERTED.OldSalesPrice, INSERTED.NewSalesPrice FROM INSERTED 
UPDATE Products SET Products.SalesPrice = i.SalesPrice FROM @insert as i WHERE Products.Id = i.IdProduct
SELECT Id FROM RevaluationProducts WHERE @@ROWCOUNT > 0 and Id = scope_identity();  
GO       
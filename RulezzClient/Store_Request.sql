USE AutomationAccountingGoods
GO

CREATE TRIGGER Products_INSERT ON Products
AFTER INSERT
AS
BEGIN
	INSERT INTO CountProducts(IdProduct, IdStore) SELECT IdProduct, IdStore FROM (SELECT INSERTED.Id as IdProduct, 1 as commonId FROM INSERTED) as tempA 
		INNER JOIN (SELECT Stores.Id as IdStore, 1 as commonId FROM Stores) as tempB on tempA.commonId = tempB.commonId;
	DECLARE @skip INT;
	SET @skip = 0
	DECLARE @groupsTable TABLE (Id INT, IdParentGroup INT)
	DECLARE @temp1 TABLE (IdParentGroup INT);
	DECLARE @temp2 TABLE (Id INT, IdParentGroup INT);
	DECLARE @idProduct INT, @idGroup INT;
	WHILE @skip != (SELECT COUNT(*) FROM inserted) BEGIN		
		SELECT @idProduct = Id, @idGroup = IdGroup FROM inserted ORDER BY Id OFFSET @skip ROWS FETCH NEXT 1 ROWS ONLY
		INSERT INTO @groupsTable SELECT Groups.Id, Groups.IdParentGroup FROM Groups WHERE Groups.Id = @idGroup;
		INSERT INTO @temp1 SELECT grT.IdParentGroup FROM @groupsTable as grT;
		WHILE @@ROWCOUNT != 0 BEGIN
			INSERT INTO @temp2 SELECT Groups.Id, Groups.IdParentGroup FROM Groups INNER JOIN @temp1 as tp1 on Groups.Id = tp1.IdParentGroup;
			INSERT INTO @groupsTable SELECT tp2.Id, tp2.IdParentGroup FROM @temp2 as tp2
			DELETE FROM @temp1
			INSERT INTO @temp1 SELECT tp2.IdParentGroup FROM @temp2 as tp2;
			DELETE FROM @temp2
		END
		SET @skip = @skip + 1;
		INSERT INTO PropertyProducts(IdProduct, IdPropertyName) SELECT @idProduct, PropertyNames.Id FROM PropertyNames INNER JOIN @groupsTable as grT on grT.Id = PropertyNames.IdGroup;
		DELETE FROM @groupsTable
	END	
END
GO

CREATE TRIGGER Products_DELETE ON Products
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM PropertyProducts WHERE Id IN (SELECT PropertyProducts.Id FROM PropertyProducts INNER JOIN deleted on (PropertyProducts.IdProduct = deleted.Id))
	DELETE FROM Products WHERE Id IN (SELECT Products.Id FROM Products INNER JOIN deleted on (Products.Id = deleted.Id))
END
GO  

CREATE TRIGGER Stores_INSERT ON Stores
AFTER INSERT
AS
BEGIN
	INSERT INTO CountProducts(IdProduct, IdStore) SELECT IdProduct, IdStore FROM (SELECT INSERTED.Id as IdStore, 1 as commonId FROM INSERTED) as tempA 
		INNER JOIN (SELECT Products.Id as IdProduct, 1 as commonId FROM Products) as tempB on tempA.commonId = tempB.commonId
END
GO

CREATE TRIGGER Groups_INSERT ON Groups
AFTER INSERT
AS
BEGIN
	INSERT INTO Stores(Title) SELECT Title FROM INSERTED WHERE IdParentGroup IS NULL
END
GO

CREATE TRIGGER Groups_UPDATE ON Groups
AFTER UPDATE
AS
BEGIN
	UPDATE Stores Set Title = INSERTED.Title FROM INSERTED INNER JOIN DELETED ON INSERTED.Id = DELETED.Id INNER JOIN Stores ON Stores.Title = deleted.Title WHERE INSERTED.IdParentGroup IS NULL AND DELETED.IdParentGroup IS NULL;
	INSERT INTO Stores(Title) SELECT INSERTED.Title FROM INSERTED INNER JOIN DELETED ON INSERTED.Id = DELETED.Id WHERE INSERTED.IdParentGroup IS NULL AND DELETED.IdParentGroup IS NOT NULL
END
GO

CREATE TRIGGER Groups_INSTEAD_DELETE ON Groups
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM PropertyNames WHERE Id IN (SELECT PropertyNames.Id FROM PropertyNames INNER JOIN deleted on (PropertyNames.IdGroup = deleted.Id))
	DELETE FROM Groups WHERE Id IN (SELECT Groups.Id FROM Groups INNER JOIN deleted on (Groups.Id = deleted.Id))
END
GO  

CREATE TRIGGER Groups_DELETE ON Groups
AFTER DELETE
AS
BEGIN
	DELETE Stores FROM Stores INNER JOIN DELETED ON  DELETED.Title = Stores.Title
END
GO 

CREATE TRIGGER WarrantyPeriods_UPDATE ON WarrantyPeriods
AFTER UPDATE
AS
BEGIN
	IF EXISTS(SELECT * FROM DELETED WHERE Period = 'Нет') BEGIN
		RAISERROR('Нельзя изменять гарантийный период: "Нет"', 16, 1)
		ROLLBACK TRAN
	END
END
GO 

CREATE TRIGGER WarrantyPeriods_DELETE ON WarrantyPeriods
AFTER DELETE
AS
BEGIN
	IF EXISTS(SELECT * FROM DELETED WHERE Period = 'Нет') BEGIN
		RAISERROR('Нельзя удалять гарантийный период: "Нет"', 16, 1)
		ROLLBACK TRAN
	END
END
GO

CREATE TRIGGER ExchangeRates_UPDATE ON ExchangeRates
AFTER UPDATE
AS
BEGIN
	IF NOT EXISTS(SELECT * FROM inserted WHERE Title = 'ГРН' or Title = 'USD') BEGIN
		RAISERROR('Нельзя изменять валюты: "ГРН" и "USD"', 16, 1)
		ROLLBACK TRAN
	END
END
GO 

CREATE TRIGGER ExchangeRates_DELETE ON ExchangeRates
AFTER DELETE
AS
BEGIN
	IF EXISTS(SELECT * FROM DELETED WHERE Title = 'ГРН' or Title = 'USD') BEGIN
		RAISERROR('Нельзя удалять валюты: "ГРН" и "USD"', 16, 1)
		ROLLBACK TRAN
	END
END
GO

CREATE TRIGGER PurchaseReports_INSERT ON PurchaseReports
AFTER INSERT
AS
BEGIN
	UPDATE PurchaseReports SET  DataOrder=GETDATE() FROM INSERTED INNER JOIN PurchaseReports ON INSERTED.Id = PurchaseReports.Id
END
GO

CREATE TRIGGER PurchaseInfos_INSERT ON PurchaseInfos
AFTER INSERT
AS
BEGIN
	DECLARE @insertCountFirst TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insertCount TABLE (Count INT, IdProduct INT, IdStore INT)
	INSERT INTO @insertCountFirst(Count, IdProduct, IdStore) SELECT ins.COUNT, ins.IdProduct, 
		CAST((SELECT IdStore FROM PurchaseReports INNER JOIN INSERTED on PurchaseReports.Id = INSERTED.IdPurchaseReport) AS INT) as idstore FROM INSERTED as ins
	INSERT INTO @insertCount(Count, IdProduct, IdStore) SELECT SUM(i.COUNT), i.IdProduct, i.IdStore FROM @insertCountFirst as i GROUP BY i.IdProduct, i.IdStore
	UPDATE CountProducts SET CountProducts.Count = CountProducts.Count + ic.Count FROM @insertCount as ic
		WHERE CountProducts.IdProduct = ic.IdProduct and CountProducts.IdStore = ic.IdStore
END
GO

CREATE TRIGGER SerialNumbers_INSERT ON SerialNumbers
AFTER INSERT
AS
BEGIN
	UPDATE SerialNumbers SET PurchaseDate = GETDATE() FROM SerialNumbers INNER JOIN INSERTED  on SerialNumbers.ID = INSERTED.Id
END 
GO

CREATE TRIGGER SalesReports_INSERT ON SalesReports
AFTER INSERT
AS
BEGIN
	UPDATE SalesReports SET DataSales = GETDATE() FROM SalesReports INNER JOIN INSERTED  on SalesReports.ID = INSERTED.Id
END
GO

CREATE TRIGGER SalesInfos_INSERT ON SalesInfos
AFTER INSERT
AS
BEGIN
	DECLARE @insertFirst TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insert TABLE (Count INT, IdProduct INT, IdStore INT)
	INSERT INTO @insertFirst(Count, IdProduct, IdStore) SELECT ins.Count, ins.IdProduct, 
		(SELECT IdStore FROM SalesReports WHERE SalesReports.Id = ins.IdSalesReport) FROM INSERTED as ins;
	INSERT INTO @insert(Count, IdProduct, IdStore) SELECT SUM(insf.Count), insf.IdProduct, insf.IdStore FROM @insertFirst as insf GROUP BY insf.IdProduct, insf.IdStore;
	IF EXISTS(SELECT * FROM @insert as inse INNER JOIN CountProducts as ct ON inse.IdProduct = ct.IdProduct and inse.IdStore = ct.IdStore
			 WHERE inse.COUNT > ct.COUNT) BEGIN
		RAISERROR('Кол-во товара в некоторых продажах превышает имеющееся кол-во на складе', 16, 1);
		ROLLBACK TRAN;
	END
	ELSE BEGIN
		UPDATE CountProducts SET CountProducts.Count = CountProducts.Count - inse.Count FROM @insert as inse 
			WHERE CountProducts.IdProduct = inse.IdProduct and CountProducts.IdStore = inse.IdStore;
		UPDATE SerialNumbers SET SerialNumbers.SelleDate = GETDATE() FROM INSERTED WHERE SerialNumbers.Id = INSERTED.IdSerialNumber;
	END
END
GO   

CREATE TRIGGER RevaluationProductsReports_INSERT ON RevaluationProductsReports
AFTER INSERT
AS
BEGIN
	UPDATE RevaluationProductsReports SET DataRevaluation = GETDATE() FROM RevaluationProductsReports INNER JOIN INSERTED  on RevaluationProductsReports.ID = INSERTED.Id
END
GO

CREATE TRIGGER RevaluationProductsInfos_INSERT ON RevaluationProductsInfos
AFTER INSERT
AS
BEGIN	
	DECLARE @insert TABLE (SalesPrice MONEY, IdProduct INT)
	INSERT INTO @insert(SalesPrice, IdProduct) SELECT MAX(NewSalesPrice), IdProduct FROM INSERTED GROUP BY INSERTED.IdProduct;	
	UPDATE Products SET Products.SalesPrice = i.SalesPrice FROM @insert as i WHERE Products.Id = i.IdProduct
END
GO

CREATE TRIGGER CountProducts_DELETE ON CountProducts
AFTER DELETE
AS
BEGIN
	if (SELECT SUM(deleted.Count) FROM deleted) != 0 BEGIN
		RAISERROR('Была попытка удалить товар или склад с кол-вом не равным 0', 16, 1);
		ROLLBACK TRAN;
	END
END
GO

CREATE TRIGGER PropertyNames_INSERT ON PropertyNames
AFTER INSERT
AS
BEGIN
	DECLARE @tabl TABLE (id INT)	
	CREATE TABLE #tmp1(id INT)	
	CREATE TABLE #tmp2(id INT)

	DECLARE @skip INT;
	SET @skip = 0
	DECLARE @groupsTable TABLE (Id INT, IdParentGroup INT)
	DECLARE @temp1 TABLE (IdParentGroup INT);
	DECLARE @temp2 TABLE (Id INT, IdParentGroup INT);
	DECLARE @idProperty INT, @idGroup INT;
	WHILE @skip != (SELECT COUNT(*) FROM inserted) BEGIN
		SELECT @idProperty = inserted.Id, @idGroup = inserted.IdGroup FROM inserted ORDER BY Id OFFSET @skip ROWS FETCH NEXT 1 ROWS ONLY
		INSERT INTO @tabl VALUES (@idGroup)
		INSERT INTO #tmp1(id) SELECT Groups.Id FROM Groups WHERE Groups.IdParentGroup = @idGroup
		INSERT INTO @tabl SELECT id FROM #tmp1
		WHILE EXISTS(SELECT Groups.Id FROM Groups INNER JOIN #tmp1 ON Groups.Id = #tmp1.id WHERE Groups.IdParentGroup IS NOT NULL)
		BEGIN
			INSERT INTO #tmp2(id) SELECT Groups.Id FROM Groups INNER JOIN #tmp1 ON Groups.IdParentGroup = #tmp1.id
			INSERT INTO @tabl SELECT id FROM #tmp2
			DELETE FROM #tmp1
			INSERT INTO #tmp1 SELECT id FROM #tmp2
			DELETE FROM #tmp2
		END
		SET @skip = @skip + 1;
		INSERT INTO PropertyProducts(IdProduct, IdPropertyName) SELECT Products.Id, @idProperty FROM Products INNER JOIN @tabl as tb ON Products.IdGroup = tb.id
		DELETE FROM @tabl
	END	
	 DROP TABLE #tmp1
	 DROP TABLE #tmp2
END
GO

CREATE TRIGGER PropertyNames_DELETE ON PropertyNames
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM PropertyProducts WHERE Id IN (SELECT PropertyProducts.Id FROM PropertyProducts INNER JOIN deleted on (PropertyProducts.IdPropertyName = deleted.Id))
	DELETE FROM PropertyValues WHERE Id IN (SELECT PropertyValues.Id FROM PropertyValues INNER JOIN deleted on (PropertyValues.IdPropertyName = deleted.Id))
	DELETE FROM PropertyNames WHERE Id IN (SELECT PropertyNames.Id FROM PropertyNames INNER JOIN deleted on (PropertyNames.Id = deleted.Id))
END
GO  

CREATE TRIGGER Warranties_INSERT ON Warranties
AFTER INSERT
AS
BEGIN
	UPDATE Warranties SET Warranties.DateReceipt = GETDATE() FROM Warranties INNER JOIN inserted  on Warranties.ID = inserted.Id
END
GO

CREATE PROCEDURE GoodsShipped
    @id INT
AS 
UPDATE Warranties SET Warranties.DateDeparture = GETDATE() WHERE Warranties.Id = @id
GO

CREATE PROCEDURE GoodsIssued
    @id INT
AS 
UPDATE Warranties SET Warranties.DateIssue = GETDATE() WHERE Warranties.Id = @id
GO
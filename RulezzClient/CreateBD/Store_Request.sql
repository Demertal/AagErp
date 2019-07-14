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

CREATE TRIGGER Groups_INSTEAD_DELETE ON Groups
INSTEAD OF DELETE
AS
BEGIN
	DELETE FROM PropertyNames WHERE Id IN (SELECT PropertyNames.Id FROM PropertyNames INNER JOIN deleted on (PropertyNames.IdGroup = deleted.Id))
	DELETE FROM Groups WHERE Id IN (SELECT Groups.Id FROM Groups INNER JOIN deleted on (Groups.Id = deleted.Id))
END
GO  

CREATE TRIGGER MovementGoodsReports_INSERT ON MovementGoodsReports
AFTER INSERT
AS
BEGIN
	UPDATE MovementGoodsReports SET  Date = GETDATE() FROM INSERTED INNER JOIN MovementGoodsReports ON INSERTED.Id = MovementGoodsReports.Id
END
GO

CREATE TRIGGER MovementGoodsInfos_INSERT ON MovementGoodsInfos
AFTER INSERT
AS
BEGIN
	DECLARE @insertCount TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insertCountArrival TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insertCountDisposal TABLE (Count INT, IdProduct INT, IdStore INT)
	DECLARE @insertCountResult TABLE (Count INT, IdProduct INT, IdStore INT)
	INSERT INTO @insertCount(Count, IdProduct, IdStore) SELECT ins.Count, ins.IdProduct, 
		CAST((SELECT IdArrivalStore FROM MovementGoodsReports INNER JOIN INSERTED on MovementGoodsReports.Id = INSERTED.IdReport WHERE MovementGoodsReports.TypeAction = 0) AS INT) as idstore FROM INSERTED as ins
	INSERT INTO @insertCountArrival(Count, IdProduct, IdStore) SELECT SUM(ic.Count), ic.IdProduct, ic.IdStore FROM @insertCount as ic GROUP BY ic.IdProduct, ic.IdStore
	DELETE FROM @insertCount
	INSERT INTO @insertCount(Count, IdProduct, IdStore) SELECT ins.Count, ins.IdProduct, 
		CAST((SELECT IdDisposalStore FROM MovementGoodsReports INNER JOIN INSERTED on MovementGoodsReports.Id = INSERTED.IdReport WHERE MovementGoodsReports.TypeAction = 1) AS INT) as idstore FROM INSERTED as ins
	INSERT INTO @insertCountDisposal(Count, IdProduct, IdStore) SELECT SUM(ic.Count), ic.IdProduct, ic.IdStore FROM @insertCount as ic GROUP BY ic.IdProduct, ic.IdStore
	INSERT INTO @insertCountResult(Count, IdProduct, IdStore) SELECT CAST(
             CASE
                  WHEN ica.Count IS NULL and icd.Count IS NULL
                     THEN 0
				  WHEN ica.Count IS NULL
                     THEN -icd.Count
				  WHEN icd.Count IS NULL
                     THEN ica.Count
                  ELSE ica.Count - icd.Count
             END AS INT),
			 CAST(
             CASE
                  WHEN ica.IdProduct IS NULL
                     THEN icd.IdProduct
				  ELSE ica.IdProduct
             END AS INT),
			 CAST(
             CASE
                  WHEN ica.IdStore IS NULL
                     THEN icd.IdStore
				  ELSE ica.IdStore
             END AS INT) FROM @insertCountArrival as ica FULL OUTER JOIN @insertCountDisposal as icd ON ica.IdProduct = icd.IdProduct and ica.IdStore = icd.IdStore
	IF EXISTS(SELECT * FROM @insertCountResult as icr INNER JOIN CountProducts as ct ON icr.IdProduct = ct.IdProduct and icr.IdStore = ct.IdStore
			 WHERE icr.Count + ct.Count < 0) BEGIN
		RAISERROR('Для выполнения операции не хватает кол-ва товара на складе', 16, 1);
		ROLLBACK TRAN;
	END
	ELSE BEGIN
		DECLARE @insertPrice TABLE (Amount INT, IdCounterparty INT)
		DECLARE @insertPriceResult TABLE (Amount INT, IdCounterparty INT)
		INSERT INTO @insertPrice(Amount, IdCounterparty) SELECT ins.Count*ins.Price,
		CAST((SELECT IdCounterparty FROM MovementGoodsReports INNER JOIN INSERTED on MovementGoodsReports.Id = INSERTED.IdReport WHERE MovementGoodsReports.TypeAction = 0 or MovementGoodsReports.TypeAction = 1) AS INT) as idCounterparty FROM INSERTED as ins
		INSERT INTO @insertPriceResult(Amount, IdCounterparty) SELECT SUM(Amount), IdCounterparty FROM @insertPrice as inp GROUP BY IdCounterparty
		UPDATE Counterparties SET Counterparties.Debt = Counterparties.Debt + inpr.Amount FROM @insertPriceResult as inpr WHERE Counterparties.Id = inpr.IdCounterparty
		UPDATE CountProducts SET CountProducts.Count = CountProducts.Count + icr.Count FROM @insertCountResult as icr
			WHERE CountProducts.IdProduct = icr.IdProduct and CountProducts.IdStore = icr.IdStore
	END
END
GO

CREATE TRIGGER RevaluationProductsReports_INSERT ON RevaluationProductsReports
AFTER INSERT
AS
BEGIN
	UPDATE RevaluationProductsReports SET DateRevaluation = GETDATE() FROM RevaluationProductsReports INNER JOIN INSERTED  on RevaluationProductsReports.ID = INSERTED.Id
END
GO

CREATE TRIGGER MoneyTransfers_INSERT ON MoneyTransfers
AFTER INSERT
AS
BEGIN
	UPDATE MoneyTransfers SET Date = GETDATE() FROM MoneyTransfers INNER JOIN INSERTED  on MoneyTransfers.ID = INSERTED.Id
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
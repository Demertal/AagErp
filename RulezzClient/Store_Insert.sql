USE AutomationAccountingGoods
GO

INSERT INTO UnitStorages VALUES ('шт')

INSERT INTO Groups(Title) VALUES ('Rulezz'), ('Craft')

INSERT INTO Groups(Title, IdParentGroup) VALUES ('Группа1', 2), ('Группа2', 1)

INSERT INTO Groups(Title, IdParentGroup) VALUES ('Группа3', 3), ('Группа4', 3)

INSERT INTO PropertyNames(Title, IdGroup) VALUES ('Свойство1', 1)

 INSERT INTO WarrantyPeriods VALUES ('Нет'), ('14 дней')

 INSERT INTO ExchangeRates VALUES ('ГРН', 1), ('USD', 20)

 INSERT INTO Counterparties(Title, WhoIsIt) VALUES ('Поставщик1', 0), 

 INSERT INTO Products (Title, VendorCode, Barcode, IdWarrantyPeriod, IdGroup, IdUnitStorage)
 VALUES ('Товар1', 'ven1', '8402487158014', 1, 1, 1), ('Товар2', 'ven2', '7131053223351', 2, 1, 1),
 ('Товар3', 'ven3', '4127230762266', 1, 2, 1), ('Товар4', 'ven4', '8051412688784', 2, 4, 1),
 ('Товар5', 'ven5', '1123426665840', 1, 3, 1), ('Товар6', 'ven6', '1056263388118', 2, 6, 1)

 INSERT INTO PurchaseReports(Course, IdStore, IdCounterparty) VALUES (1, 1, 1)
 INSERT INTO PurchaseInfos(Count, PurchasePrice, IdPurchaseReport, IdProduct, IdExchangeRate) VALUES (5, 100, 1, 1, 1)
 INSERT INTO PurchaseInfos(Count, PurchasePrice, IdPurchaseReport, IdProduct, IdExchangeRate) VALUES (1, 100, 1, 2, 1)
 INSERT INTO SerialNumbers(Value, IdProduct, IdCounterparty, IdPurchaseInfo) VALUES ('1', 2, 1, 2)
 INSERT INTO SalesReports(IdStore, IdCounterparty) VALUES (1, 2)
 INSERT INTO SalesInfos(Count,	SellingPrice, IdProduct, IdSalesReport) VALUES (2, 150, 1, 1)
 INSERT INTO SalesInfos(Count,	SellingPrice, IdProduct, IdSalesReport, IdSerialNumber) VALUES (1, 150, 2, 1, 1)

 SELECT * FROM ExchangeRates;
 SELECT * FROM Stores;
 SELECT * FROM UnitStorages;
 SELECT * FROM Groups;
 SELECT * FROM WarrantyPeriods;
 SELECT * FROM Products;
 SELECT * FROM PropertyNames;
 SELECT * FROM PropertyValues;
 SELECT * FROM PropertyProducts;
 SELECT * FROM Counterparties;
 SELECT * FROM PurchaseReports;
 SELECT * FROM PurchaseInfos;
 SELECT * FROM SerialNumbers;
 SELECT * FROM SalesReports;
 SELECT * FROM SalesInfos;
 SELECT * FROM Warranties;
 SELECT * FROM RevaluationProductsReports;
 SELECT * FROM RevaluationProductsInfos;
 SELECT * FROM CountProducts;
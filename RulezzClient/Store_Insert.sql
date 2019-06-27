USE AutomationAccountingGoods
GO

INSERT INTO UnitStorages VALUES ('��')

INSERT INTO Groups(Title) VALUES ('Rulezz'), ('Craft')

INSERT INTO Groups(Title, IdParentGroup) VALUES ('������1', 2), ('������2', 1)

INSERT INTO Groups(Title, IdParentGroup) VALUES ('������3', 3), ('������4', 3)

INSERT INTO PropertyNames(Title, IdGroup) VALUES ('��������1', 1)

 INSERT INTO WarrantyPeriods VALUES ('���'), ('14 ����')

 INSERT INTO ExchangeRates VALUES ('���', 1), ('USD', 20)

 INSERT INTO Counterparties(Title, WhoIsIt) VALUES ('���������1', 0), 

 INSERT INTO Products (Title, VendorCode, Barcode, IdWarrantyPeriod, IdGroup, IdUnitStorage)
 VALUES ('�����1', 'ven1', '8402487158014', 1, 1, 1), ('�����2', 'ven2', '7131053223351', 2, 1, 1),
 ('�����3', 'ven3', '4127230762266', 1, 2, 1), ('�����4', 'ven4', '8051412688784', 2, 4, 1),
 ('�����5', 'ven5', '1123426665840', 1, 3, 1), ('�����6', 'ven6', '1056263388118', 2, 6, 1)

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
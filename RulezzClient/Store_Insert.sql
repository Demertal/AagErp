INSERT INTO UnitStorages VALUES ('��')

INSERT INTO Groups(Title) VALUES ('Rulezz'), ('Craft')

INSERT INTO Groups(Title, IdParentGroup) VALUES ('������1', 2), ('������2', 1)

INSERT INTO Groups(Title, IdParentGroup) VALUES ('������3', 3), ('������4', 3)

 INSERT INTO WarrantyPeriods VALUES ('���'), ('14 ����')

 INSERT INTO ExchangeRates VALUES ('���'), ('USD')

 INSERT INTO Suppliers VALUES ('���������1')

 INSERT INTO Products (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdExchangeRate, IdWarrantyPeriod, IdGroup, IdUnitStorage)
 VALUES ('�����1', 'ven1', 'bar1',  100, 200, 1, 1, 1, 1), ('�����2', 'ven2', 'bar2',  200, 400, 1, 2, 1, 1),
 ('�����3', 'ven3', 'bar3',  200, 400, 1, 1, 2, 1), ('�����4', 'ven4', 'bar4',  500, 800, 1, 2, 2, 1),
 ('�����5', 'ven5', 'bar5',  200, 400, 1, 1, 3, 1), ('�����6', 'ven6', 'bar6',  200, 400, 1, 2, 6, 1)

 SELECT * FROM ExchangeRates;
 SELECT * FROM Stores;
 SELECT * FROM UnitStorages;
 SELECT * FROM Groups;
 SELECT * FROM WarrantyPeriods;
 SELECT * FROM Products;
 SELECT * FROM PropertyNames;
 SELECT * FROM PropertyValues;
 SELECT * FROM PropertyProducts;
 SELECT * FROM Suppliers;
 SELECT * FROM PurchaseReports;
 SELECT * FROM PurchaseInfos;
 SELECT * FROM SerialNumbers;
 SELECT * FROM SalesReports;
 SELECT * FROM SalesInfos;
 SELECT * FROM Warranties;
 SELECT * FROM RevaluationProducts;
 SELECT * FROM CountProducts;
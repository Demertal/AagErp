 INSERT INTO UnitStorages VALUES ('��')

INSERT INTO Groups(Title, IdUnitStorage) VALUES ('Rulezz', 1)

INSERT INTO Groups(Title, IdUnitStorage) VALUES ('Craft', 1)

INSERT INTO Groups(Title, IdParentGroup, IdUnitStorage) VALUES ('������1', 2, 1), ('������2', 1, 1)

INSERT INTO Groups(Title, IdParentGroup, IdUnitStorage) VALUES ('������3', 3, 1), ('������4', 3, 1)

INSERT INTO PriceGroups VALUES (1)

 INSERT INTO WarrantyPeriods VALUES ('���'), ('14 ����')

 INSERT INTO ExchangeRates VALUES ('���', 1), ('USD', 20)

 INSERT INTO Products (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdExchangeRate, IdWarrantyPeriod, IdGroup)
 VALUES ('�����1', 'ven1', 'bar1',  100, 200, 1, 1, 1), ('�����2', 'ven2', 'bar2',  200, 400, 1, 2, 1),
 ('�����3', 'ven3', 'bar3',  200, 400, 1, 1, 2), ('�����4', 'ven4', 'bar4',  500, 800, 1, 2, 2),
 ('�����5', 'ven5', 'bar5',  200, 400, 1, 1, 3), ('�����6', 'ven6', 'bar6',  200, 400, 1, 2, 6)
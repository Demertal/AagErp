 INSERT INTO UnitStorages VALUES ('шт')

INSERT INTO Groups(Title, IdUnitStorage) VALUES ('Rulezz', 1)

INSERT INTO Groups(Title, IdUnitStorage) VALUES ('Craft', 1)

INSERT INTO Groups(Title, IdParentGroup, IdUnitStorage) VALUES ('Группа1', 2, 1), ('Группа2', 1, 1)

INSERT INTO Groups(Title, IdParentGroup, IdUnitStorage) VALUES ('Группа3', 3, 1), ('Группа4', 3, 1)

INSERT INTO PriceGroups VALUES (1)

 INSERT INTO WarrantyPeriods VALUES ('Нет'), ('14 дней')

 INSERT INTO ExchangeRates VALUES ('грн', 1), ('USD', 20)

 INSERT INTO Products (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdExchangeRate, IdWarrantyPeriod, IdGroup)
 VALUES ('Товар1', 'ven1', 'bar1',  100, 200, 1, 1, 1), ('Товар2', 'ven2', 'bar2',  200, 400, 1, 2, 1),
 ('Товар3', 'ven3', 'bar3',  200, 400, 1, 1, 2), ('Товар4', 'ven4', 'bar4',  500, 800, 1, 2, 2),
 ('Товар5', 'ven5', 'bar5',  200, 400, 1, 1, 3), ('Товар6', 'ven6', 'bar6',  200, 400, 1, 2, 6)
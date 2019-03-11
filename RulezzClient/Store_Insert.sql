INSERT INTO Groups(Title) VALUES ('Rulezz')

INSERT INTO Groups(Title) VALUES ('Craft')

INSERT INTO Groups VALUES ('Группа1', 2), ('Группа2', 1)

INSERT INTO Groups VALUES ('Группа3', 3), ('Группа4', 3)

INSERT INTO PriceGroups VALUES (1)

 INSERT INTO WarrantyPeriods VALUES ('Нет'), ('14 дней')

 INSERT INTO UnitStorages VALUES ('шт')

 INSERT INTO ExchangeRates VALUES ('грн', 1), ('USD', 20)

 INSERT INTO Products (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdUnitStorage, IdExchangeRate, IdWarrantyPeriod, IdGroup)
 VALUES ('Товар1', 'ven1', 'bar1',  100, 200, 1, 1, 1, 1), ('Товар2', 'ven2', 'bar2',  200, 400, 1, 1, 2, 1),
 ('Товар3', 'ven3', 'bar3',  200, 400, 1, 1, 1, 2), ('Товар4', 'ven4', 'bar4',  500, 800, 1, 1, 2, 2),
 ('Товар5', 'ven5', 'bar5',  200, 400, 1, 1, 1, 3), ('Товар6', 'ven6', 'bar6',  200, 400, 1, 1, 2, 6)
INSERT INTO Store VALUES ('Rulezz'), ('Craft')

INSERT INTO NomenclatureGroup VALUES ('Группа1', 1), ('Группа2', 1), ('Группа3', 2), ('Группа4', 2)

INSERT INTO PriceGroup VALUES (1)

INSERT INTO NomenclatureSubGroup (Title, IdNomenclatureGroup, IdPriceGroup) VALUES ('Группа1', 2, 1), ('Группа2', 2, 1), ('Группа3', 3, 1), ('Группа4', 3, 1),
 ('Группа5', 4, 1), ('Группа6', 4, 1), ('Группа7', 5, 1), ('Группа8', 5, 1)

 INSERT INTO WarrantyPeriod VALUES (0)

 INSERT INTO UnitStorage VALUES ('шт')

 INSERT INTO ExchangeRate VALUES ('грн', 1), ('USD', 20)

 INSERT INTO Product (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdNomenclatureSubGroup, IdUnitStorage, IdExchangeRate, IdWarrantyPeriod)
 VALUES ('asd', 'asd', 'asd',  2, 2, 4, 1, 1, 1)
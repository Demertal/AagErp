INSERT INTO Store VALUES ('Rulezz'), ('Craft')

INSERT INTO NomenclatureGroup VALUES ('������1', 1), ('������2', 1), ('������3', 2), ('������4', 2)

INSERT INTO PriceGroup VALUES (1)

INSERT INTO NomenclatureSubGroup (Title, IdNomenclatureGroup, IdPriceGroup) VALUES ('������1', 2, 1), ('������2', 2, 1), ('������3', 3, 1), ('������4', 3, 1),
 ('������5', 4, 1), ('������6', 4, 1), ('������7', 5, 1), ('������8', 5, 1)

 INSERT INTO WarrantyPeriod VALUES (0)

 INSERT INTO UnitStorage VALUES ('��')

 INSERT INTO ExchangeRate VALUES ('���', 1), ('USD', 20)

 INSERT INTO Product (Title, VendorCode, Barcode, PurchasePrice, SalesPrice, IdNomenclatureSubGroup, IdUnitStorage, IdExchangeRate, IdWarrantyPeriod)
 VALUES ('asd', 'asd', 'asd',  2, 2, 4, 1, 1, 1)
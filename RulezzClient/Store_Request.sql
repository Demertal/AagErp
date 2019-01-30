USE Store
GO

DROP FUNCTION IF EXISTS ProductView
GO

CREATE FUNCTION ProductView(@idNomenclatureSubGroup INT)
RETURNS TABLE AS RETURN (SELECT Product.Id, Product.Barcode, Product.Count, (SELECT ExchangeRate.Title FROM ExchangeRate WHERE ExchangeRate.Id = Product.IdExchangeRate) as ExchangeRate,
	(SELECT UnitStorage.Title FROM UnitStorage WHERE UnitStorage.Id = Product.IdUnitStorage) as UnitStorage,
	(SELECT WarrantyPeriod.Period FROM WarrantyPeriod WHERE WarrantyPeriod.Id = Product.IdWarrantyPeriod) as WarrantyPeriod,
	Product.PurchasePrice, Product.SalesPrice, Product.Title, Product.VendorCode, Product.IdNomenclatureSubGroup FROM Product WHERE Product.IdNomenclatureSubGroup = @idNomenclatureSubGroup)
GO

CREATE FUNCTION AllProductView()
RETURNS TABLE AS RETURN
SELECT Product.Id, Product.Barcode, Product.Count, (SELECT ExchangeRate.Title FROM ExchangeRate WHERE ExchangeRate.Id = Product.IdExchangeRate) as ExchangeRate,
	(SELECT UnitStorage.Title FROM UnitStorage WHERE UnitStorage.Id = Product.IdUnitStorage) as UnitStorage,
	(SELECT WarrantyPeriod.Period FROM WarrantyPeriod WHERE WarrantyPeriod.Id = Product.IdWarrantyPeriod) as WarrantyPeriod,
	Product.PurchasePrice, Product.SalesPrice, Product.Title, Product.VendorCode FROM Product
GO

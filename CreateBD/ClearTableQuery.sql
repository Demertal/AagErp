delete from dbo.serialNumberLogs
DBCC CHECKIDENT ('serialNumberLogs', RESEED, 0)
select * from dbo.serialNumberLogs

delete from dbo.serialNumbers
DBCC CHECKIDENT ('serialNumbers', RESEED, 0)
select * from dbo.serialNumbers

delete from dbo.movementGoodsInfos
DBCC CHECKIDENT ('movementGoodsInfos', RESEED, 0)
select * from dbo.movementGoodsInfos

delete from dbo.movementGoods
DBCC CHECKIDENT ('movementGoods', RESEED, 0)
select * from dbo.movementGoods

delete from dbo.priceProducts
DBCC CHECKIDENT ('priceProducts', RESEED, 0)
select * from dbo.priceProducts

delete from dbo.revaluationProducts
DBCC CHECKIDENT ('revaluationProducts', RESEED, 0)
select * from dbo.revaluationProducts

delete from dbo.products
DBCC CHECKIDENT ('products', RESEED, 0)
select * from dbo.products

delete from dbo.warrantyPeriods
DBCC CHECKIDENT ('warrantyPeriods', RESEED, 0)
select * from dbo.warrantyPeriods

delete from dbo.unitStorages
DBCC CHECKIDENT ('unitStorages', RESEED, 0)
select * from dbo.unitStorages

delete from dbo.priceGroups
DBCC CHECKIDENT ('priceGroups', RESEED, 0)
select * from dbo.priceGroups

delete from dbo.propertyProducts
DBCC CHECKIDENT ('propertyProducts', RESEED, 0)
select * from dbo.propertyProducts

delete from dbo.propertyValues
DBCC CHECKIDENT ('propertyValues', RESEED, 0)
select * from dbo.propertyValues

delete from dbo.propertyNames
DBCC CHECKIDENT ('propertyNames', RESEED, 0)
select * from dbo.propertyNames

delete from dbo.categories
DBCC CHECKIDENT ('categories', RESEED, 0)
select * from dbo.categories

delete from dbo.stores
DBCC CHECKIDENT ('stores', RESEED, 0)
select * from dbo.stores

delete from dbo.currencies
DBCC CHECKIDENT ('currencies', RESEED, 0)
select * from dbo.currencies

delete from dbo.counterparties
DBCC CHECKIDENT ('counterparties', RESEED, 0)
select * from dbo.counterparties
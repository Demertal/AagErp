USE [master]
GO

DROP DATABASE IF EXISTS Store
GO

CREATE DATABASE Store
GO

USE Store

/*
* ExchangeRate таблица валют
* Title наименование валюты
* Сourse курс
*/
CREATE TABLE ExchangeRate(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(10) NOT NULL UNIQUE CHECK(Title !=''),
	Сourse FLOAT NOT NULL UNIQUE CHECK(Сourse >= 0),
	CONSTRAINT UQ_ExchangeRate_CurrencyСourse UNIQUE (Title, Сourse)
)
GO

/*
* Store таблица магазинов
* Title наименование магазина
*/
CREATE TABLE Store(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !='')
)
GO

/*
* NomenclatureGroup таблица номенклатурных групп
* Title наименование группы
* IdStore id магазина
*/
CREATE TABLE NomenclatureGroup(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL CHECK(Title !=''),
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Store (Id),
	CONSTRAINT UQ_NomenclatureGroup_TitleIdStore UNIQUE (Title, IdStore)
)
GO

/*
* PriceGroup таблица наценок
* Markup наценка
*/
CREATE TABLE PriceGroup(
	Id INT PRIMARY KEY IDENTITY,
	Markup float NOT NULL UNIQUE CHECK(Markup >= 0),
)
GO

/*
* NomenclatureSubGroup таблица номенклатурных подгрупп
* Title наименование группы
* IdNomenclatureGroup id номенклатурной группы
*/
CREATE TABLE NomenclatureSubGroup(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL CHECK(Title !=''),
	IdNomenclatureGroup INT NOT NULL,
	IdPriceGroup INT NOT NULL,
	FOREIGN KEY (IdNomenclatureGroup) REFERENCES NomenclatureGroup (Id),
	FOREIGN KEY (IdPriceGroup) REFERENCES PriceGroup (Id),
	CONSTRAINT UQ_NomenclatureSubGroup_TitleIdNomenclatureGroup UNIQUE (Title, IdNomenclatureGroup)
)
GO

/*
* UnitStorage таблица типов хранения
* Title наименование типа
*/
CREATE TABLE UnitStorage(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !=''),
)
GO

/*
* WarrantyPeriod таблица сроков гарантии
* Period срок
*/
CREATE TABLE WarrantyPeriod(
	Id INT PRIMARY KEY IDENTITY,
	Period INT NOT NULL UNIQUE CHECK(Period >= 0),
)
GO

/*
* Product таблица товаров
* Title наименование товара
* VendorCode код производителя
* Barcode штрихкод
* Count кол-во
* PurchasePrice закупочная цена
* SalesPrice цена продажи
* IdNomenclatureSubGroup id номенклатурной подгруппы
* IdUnitStorage id типа хранения
* IdExchangeRate id валюты для закупочной цены
* IdWarrantyPeriod id гарантийного срока
*/
CREATE TABLE Product(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(120) NOT NULL CHECK(Title !=''),
	VendorCode NVARCHAR(20) NULL,
	Barcode NVARCHAR(13) NULL,
	Count INT NOT NULL DEFAULT 0 CHECK(Count >= 0),
	PurchasePrice MONEY NOT NULL DEFAULT 0 CHECK(PurchasePrice >= 0),
	SalesPrice MONEY NOT NULL DEFAULT 0 CHECK(SalesPrice >= 0),
	IdNomenclatureSubGroup INT NOT NULL,
	IdUnitStorage INT NOT NULL,
	IdExchangeRate INT NOT NULL,
	IdWarrantyPeriod INT NOT NULL,
	FOREIGN KEY (IdNomenclatureSubGroup) REFERENCES NomenclatureSubGroup (Id),
	FOREIGN KEY (IdUnitStorage) REFERENCES UnitStorage (Id),
	FOREIGN KEY (IdExchangeRate) REFERENCES ExchangeRate (Id),
	FOREIGN KEY (IdWarrantyPeriod) REFERENCES WarrantyPeriod (Id),
)
GO

/*
* PropertyName таблица наименований параметров товара
* Title наименование параметра
* IdNomenclatureSubGroup id номенклатурной группы
*/
CREATE TABLE PropertyName(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL CHECK(Title !=''),
	IdNomenclatureGroup INT NOT NULL,
	FOREIGN KEY (IdNomenclatureGroup) REFERENCES NomenclatureGroup (Id),
	CONSTRAINT UQ_PropertyName_TitleIdNomenclatureGroup UNIQUE (Title, IdNomenclatureGroup)
)
GO

/*
* PropertyValue таблица значений параметров товара
* Value значние параметра
* IdPropertyName id наименования параметра
*/
CREATE TABLE PropertyValue(
	Id INT PRIMARY KEY IDENTITY,
	Value SQL_VARIANT NOT NULL,
	IdPropertyName INT NOT NULL,
	FOREIGN KEY (IdPropertyName) REFERENCES PropertyName (Id),
	CONSTRAINT UQ_PropertyValue_ValueIdPropertyName UNIQUE (Value, IdPropertyName)
)
GO

/*
* PropertyProduct таблица значений параметров товара
* IdProduct id продукта
* IdPropertyValue id значения параметра
*/
CREATE TABLE PropertyProduct(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdPropertyValue INT NOT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Product (Id),
	FOREIGN KEY (IdPropertyValue) REFERENCES PropertyValue (Id),
	CONSTRAINT UQ_PropertyProduct_IdProductIdPropertyValue UNIQUE (IdProduct, IdPropertyValue)
)
GO

/*
* Supplier таблица поставщиков
* Title поставщик
* IdStore id магазина
*/
CREATE TABLE Supplier(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Store (Id),
	CONSTRAINT UQ_Supplier_TitleIdStore UNIQUE (Title, IdStore)
)
GO

/*
* PurchaseReport таблица отчетов о покупке
* DataOrder дата покупки
* IdStore id магазина
* IdSupplier id поставщика
*/
CREATE TABLE PurchaseReport(
	Id INT PRIMARY KEY IDENTITY,
	DataOrder DATE NOT NULL,
	IdStore INT NOT NULL,
	IdSupplier INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Store (Id),
	FOREIGN KEY (IdSupplier) REFERENCES Supplier (Id)
)
GO

/*
* PurchaseInfo таблица информации об отчетах о покупке
* Count кол-во
* PurchasePrice цена закупки
* IdPurchaseReport id отчета о покупке
* IdProduct id продукта
* IdExchangeRate id валюты
*/
CREATE TABLE PurchaseInfo(
	Id INT PRIMARY KEY IDENTITY,
	Count INT NOT NULL CHECK (Count >= 0),
	PurchasePrice MONEY NOT NULL CHECK (PurchasePrice >= 0),
	IdPurchaseReport INT NOT NULL,
	IdProduct INT NOT NULL,
	IdExchangeRate INT NOT NULL,
	FOREIGN KEY (IdPurchaseReport) REFERENCES PurchaseReport (Id),
	FOREIGN KEY (IdProduct) REFERENCES Product (Id),
	FOREIGN KEY (IdExchangeRate) REFERENCES ExchangeRate (Id)
)
GO

/*
* SerialNumber таблица серийных номеров
* Value серийный номер
* SelleDate дата продажи
* PurchaseDate дата покупки
*/
CREATE TABLE SerialNumber(
	Id INT PRIMARY KEY IDENTITY,
	Value VARCHAR(20) NOT NULL UNIQUE CHECK (Value != ''),
	SelleDate DATE NULL,	
	PurchaseDate DATE NOT NULL
)
GO

/*
* SalesReport таблица отчетов о продаже
* DataSales дата продажи
* IdStore id магазина
*/
CREATE TABLE SalesReport(
	Id INT PRIMARY KEY IDENTITY,
	DataSales DATE NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Store (Id)
)
GO

/*
* SalesInfo таблица информации об отчетах о покупке
* Count кол-во
* SellingPrice цена продажи
* IdSalesReport id отчета о продажи
* IdProduct id продукта
* IdSerialNumber id серийного номера
*/
CREATE TABLE SalesInfo(
	Id INT PRIMARY KEY IDENTITY,
	Count INT NOT NULL CHECK (Count >= 0),
	SellingPrice MONEY NOT NULL CHECK (SellingPrice >= 0),	
	IdProduct INT NOT NULL,
	IdSalesReport INT NOT NULL,
	IdSerialNumber INT DEFAULT NULL,
	FOREIGN KEY (IdSalesReport) REFERENCES SalesReport (Id),
	FOREIGN KEY (IdProduct) REFERENCES Product (Id),
	FOREIGN KEY (IdSerialNumber) REFERENCES SerialNumber (Id)
)
GO

/*
* Warranty таблица товаров на гарантии
* Malfunction неисправность
* DateReceipt дата получения
* DateDeparture дата отправки
* DateIssue дата выдачи
* Info инфо о покупателе
* IdSupplier id поставщика
* IdSerialNumber id серийного номера
*/
CREATE TABLE Warranty(
	Id INT PRIMARY KEY IDENTITY,
	Malfunction NVARCHAR(256) NOT NULL CHECK (Malfunction != ''),
	DateReceipt DATE NOT NULL,	
	DateDeparture DATE NOT NULL,
	DateIssue DATE NOT NULL,
	Info NVARCHAR(256),
	IdSupplier INT NOT NULL,
	IdSerialNumber INT DEFAULT NULL,
	FOREIGN KEY (IdSupplier) REFERENCES Supplier (Id),
	FOREIGN KEY (IdSerialNumber) REFERENCES SerialNumber (Id)
)
GO
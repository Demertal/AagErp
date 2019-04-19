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
CREATE TABLE ExchangeRates(
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
CREATE TABLE Stores(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !='')
)
GO


/*
* UnitStorage таблица типов хранения
* Title наименование типа
*/
CREATE TABLE UnitStorages(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !=''),
)
GO

/*
* Group группы
* Title наименование группы
* IdUnitStorage id типа хранения
*/
CREATE TABLE Groups(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL CHECK(Title !=''),
	IdParentGroup INT,
	IdUnitStorage INT NOT NULL,
	FOREIGN KEY (IdParentGroup) REFERENCES Groups (Id),
	FOREIGN KEY (IdUnitStorage) REFERENCES UnitStorages (Id),
	CONSTRAINT UQ_Group_TitleIdParentGroup UNIQUE (Title, IdParentGroup)
)
GO

/*
* PriceGroup таблица наценок
* Markup наценка
*/
CREATE TABLE PriceGroups(
	Id INT PRIMARY KEY IDENTITY,
	Markup float NOT NULL UNIQUE CHECK(Markup >= 0),
)
GO

/*
* WarrantyPeriod таблица сроков гарантии
* Period срок
*/
CREATE TABLE WarrantyPeriods(
	Id INT PRIMARY KEY IDENTITY,
	Period NVARCHAR(20) NOT NULL UNIQUE CHECK(Period != ''),
)
GO

/*
* Product таблица товаров
* Title наименование товара
* VendorCode код производителя
* Barcode штрихкод
* PurchasePrice закупочная цена
* SalesPrice цена продажи
* IdExchangeRate id валюты для закупочной цены
* IdWarrantyPeriod id гарантийного срока
* IdGroup id группы
*/
CREATE TABLE Products(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(120) NOT NULL CHECK(Title !='') UNIQUE,
	VendorCode NVARCHAR(20) NULL,
	Barcode NVARCHAR(13) NULL,
	PurchasePrice MONEY NOT NULL DEFAULT 0 CHECK(PurchasePrice >= 0),
	SalesPrice MONEY NOT NULL DEFAULT 0 CHECK(SalesPrice >= 0),	
	IdExchangeRate INT NOT NULL,
	IdWarrantyPeriod INT NOT NULL,
	IdGroup INT NOT NULL,	
	FOREIGN KEY (IdExchangeRate) REFERENCES ExchangeRates (Id),
	FOREIGN KEY (IdWarrantyPeriod) REFERENCES WarrantyPeriods (Id),
	FOREIGN KEY (IdGroup) REFERENCES Groups (Id)
)
GO

/*
* PropertyName таблица наименований параметров товара
* Title наименование параметра
* IdGroup id группы
*/
CREATE TABLE PropertyNames(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL CHECK(Title !=''),
	IdGroup INT NOT NULL,
	FOREIGN KEY (IdGroup) REFERENCES Groups (Id),
	CONSTRAINT UQ_PropertyName_TitleIdGroup UNIQUE (Title, IdGroup)
)
GO

/*
* PropertyValue таблица значений параметров товара
* Value значние параметра
* IdPropertyName id наименования параметра
*/
CREATE TABLE PropertyValues(
	Id INT PRIMARY KEY IDENTITY,
	Value NVARCHAR(50) NOT NULL,
	IdPropertyName INT NOT NULL,
	FOREIGN KEY (IdPropertyName) REFERENCES PropertyNames (Id),
	CONSTRAINT UQ_PropertyValue_ValueIdPropertyName UNIQUE (Value, IdPropertyName)
)
GO

/*
* PropertyProduct таблица значений параметров товара
* IdProduct id продукта
* IdPropertyValue id значения параметра
*/
CREATE TABLE PropertyProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdPropertyValue INT NOT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdPropertyValue) REFERENCES PropertyValues (Id),
	CONSTRAINT UQ_PropertyProduct_IdProductIdPropertyValue UNIQUE (IdProduct, IdPropertyValue)
)
GO

/*
* Supplier таблица поставщиков
* Title поставщик
* IdStore id магазина
*/
CREATE TABLE Suppliers(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id),
	CONSTRAINT UQ_Supplier_TitleIdStore UNIQUE (Title, IdStore)
)
GO

/*
* PurchaseReport таблица отчетов о покупке
* DataOrder дата покупки
* IdStore id магазина
* IdSupplier id поставщика
*/
CREATE TABLE PurchaseReports(
	Id INT PRIMARY KEY IDENTITY,
	DataOrder DATE NOT NULL,
	IdStore INT NOT NULL,
	IdSupplier INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id),
	FOREIGN KEY (IdSupplier) REFERENCES Suppliers (Id)
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
CREATE TABLE PurchaseInfos(
	Id INT PRIMARY KEY IDENTITY,
	Count INT NOT NULL CHECK (Count >= 0),
	PurchasePrice MONEY NOT NULL CHECK (PurchasePrice >= 0),
	IdPurchaseReport INT NOT NULL,
	IdProduct INT NOT NULL,
	IdExchangeRate INT NOT NULL,
	FOREIGN KEY (IdPurchaseReport) REFERENCES PurchaseReports (Id),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdExchangeRate) REFERENCES ExchangeRates (Id)
)
GO

/*
* SerialNumber таблица серийных номеров
* Value серийный номер
* SelleDate дата продажи
* PurchaseDate дата покупки
* IdProduct id продукта
* IdSupplier id поставщика
*/
CREATE TABLE SerialNumbers(
	Id INT PRIMARY KEY IDENTITY,
	Value VARCHAR(20) NOT NULL UNIQUE CHECK (Value != ''),
	SelleDate DATE NULL,	
	PurchaseDate DATE NOT NULL,
	IdProduct INT NOT NULL,
	IdSupplier INT NOT NULL,
	FOREIGN KEY (IdSupplier) REFERENCES Suppliers (Id),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id) 
)
GO

/*
* SalesReport таблица отчетов о продаже
* DataSales дата продажи
* IdStore id магазина
*/
CREATE TABLE SalesReports(
	Id INT PRIMARY KEY IDENTITY,
	DataSales DATE NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id)
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
CREATE TABLE SalesInfos(
	Id INT PRIMARY KEY IDENTITY,
	Count INT NOT NULL CHECK (Count >= 0),
	SellingPrice MONEY NOT NULL CHECK (SellingPrice >= 0),	
	IdProduct INT NOT NULL,
	IdSalesReport INT NOT NULL,
	IdSerialNumber INT DEFAULT NULL,
	FOREIGN KEY (IdSalesReport) REFERENCES SalesReports (Id),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdSerialNumber) REFERENCES SerialNumbers (Id)
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
CREATE TABLE Warranties(
	Id INT PRIMARY KEY IDENTITY,
	Malfunction NVARCHAR(256) NOT NULL CHECK (Malfunction != ''),
	DateReceipt DATE NOT NULL,	
	DateDeparture DATE NOT NULL,
	DateIssue DATE NOT NULL,
	Info NVARCHAR(256),
	IdSupplier INT NOT NULL,
	IdSerialNumber INT DEFAULT NULL,
	FOREIGN KEY (IdSupplier) REFERENCES Suppliers (Id),
	FOREIGN KEY (IdSerialNumber) REFERENCES SerialNumbers (Id)
)
GO

CREATE TABLE RevaluationProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	Date DATE NOT NULL,
	OldSalesPrice MONEY NOT NULL,
	NewSalesPrice MONEY NOT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
)
GO

CREATE TABLE CountProducts(
	IdProduct INT NOT NULL,
	IdStore INT NOT NULL,
	Count FLOAT NOT NULL DEFAULT 0,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdStore) REFERENCES Stores (Id)
)
GO
USE [master]
GO

DROP DATABASE IF EXISTS AutomationAccountingGoods
GO

CREATE DATABASE AutomationAccountingGoods
GO

USE AutomationAccountingGoods

/*
* ExchangeRate таблица валют
* Title наименование валюты
* Course курс
*/
CREATE TABLE ExchangeRates(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(10) NOT NULL UNIQUE CHECK(Title !=''),
	Course MONEY NOT NULL CHECK(Course > 0)
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
	FOREIGN KEY (IdParentGroup) REFERENCES Groups (Id),	
	CONSTRAINT UQ_Group_TitleIdParentGroup UNIQUE (Title, IdParentGroup)
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
* SalesPrice цена продажи
* IdWarrantyPeriod id гарантийного срока
* IdGroup id группы
*/
CREATE TABLE Products(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(120) NOT NULL CHECK(Title !='') UNIQUE,
	VendorCode NVARCHAR(20) NULL,
	Barcode NVARCHAR(13) NULL UNIQUE,
	SalesPrice MONEY NOT NULL DEFAULT 0 CHECK(SalesPrice >= 0),	
	IdWarrantyPeriod INT NOT NULL,
	IdGroup INT NOT NULL,
	IdUnitStorage INT NOT NULL,
	FOREIGN KEY (IdUnitStorage) REFERENCES UnitStorages (Id),
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
	FOREIGN KEY (IdGroup) REFERENCES Groups (Id) ON DELETE CASCADE,
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
	FOREIGN KEY (IdPropertyName) REFERENCES PropertyNames (Id) ON DELETE CASCADE,
	CONSTRAINT UQ_PropertyValue_ValueIdPropertyName UNIQUE (Value, IdPropertyName)
)
GO

/*
* PropertyProduct таблица значений параметров товара
* IdProduct id продукта
* IdPropertyName id параметра
* IdPropertyValue id значения параметра
*/
CREATE TABLE PropertyProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdPropertyName INT NOT NULL,
	IdPropertyValue INT DEFAULT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id) ON DELETE CASCADE,
	FOREIGN KEY (IdPropertyName) REFERENCES PropertyNames (Id) ON DELETE CASCADE,
	FOREIGN KEY (IdPropertyValue) REFERENCES PropertyValues (Id),
	CONSTRAINT UQ_PropertyProduct_IdProductIdPropertyName UNIQUE (IdProduct, IdPropertyName)
)
GO

/*
* Сounterparties таблица контрагентов
* Title наименование организации
* СontactPerson контактное лицо
* Props реквезиты
* Address адрес
* WhoIsIt кто это 0 - поставщик, 1 - покупатель
*/
CREATE TABLE Counterparties(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(40) NOT NULL,
	ContactPerson NVARCHAR(40) NULL,
	ContactPhone NVARCHAR(50) NULL,
	Props NVARCHAR(40) NULL,
	Address NVARCHAR(40) NULL,
	WhoIsIt BIT NOT NULL
	CONSTRAINT UQ_Title_WhoIsIt UNIQUE (Title, WhoIsIt)
)
GO

/*
* PurchaseReport таблица отчетов о закупке
* DataOrder дата закупки
* Сourse курс
* IdStore id магазина
* IdSupplier id поставщика
*/
CREATE TABLE PurchaseReports(
	Id INT PRIMARY KEY IDENTITY,
	DataOrder DATE NULL,
	Course MONEY NOT NULL CHECK(Course >= 0),
	TextInfo NVARCHAR(50), 
	IdStore INT NOT NULL,
	IdCounterparty INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id),
	FOREIGN KEY (IdCounterparty) REFERENCES Counterparties (Id)
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
* IdCounterparty id поставщика
*/
CREATE TABLE SerialNumbers(
	Id INT PRIMARY KEY IDENTITY,
	Value VARCHAR(20) NOT NULL CHECK (Value != ''),
	SelleDate DATE NULL,	
	PurchaseDate DATE NULL,
	IdProduct INT NOT NULL,
	IdCounterparty INT NOT NULL,
	FOREIGN KEY (IdCounterparty) REFERENCES Counterparties (Id),
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
	DataSales DATE NULL,
	IdStore INT NOT NULL,
	IdCounterparty INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id),
	FOREIGN KEY (IdCounterparty) REFERENCES Counterparties (Id)
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
* IdSerialNumber id серийного номера
*/
CREATE TABLE Warranties(
	Id INT PRIMARY KEY IDENTITY,
	Malfunction NVARCHAR(256) NOT NULL CHECK (Malfunction != ''),
	DateReceipt DATE NULL,	
	DateDeparture DATE NULL,
	DateIssue DATE NULL,
	Info NVARCHAR(256),
	IdSerialNumber INT NOT NULL,
	FOREIGN KEY (IdSerialNumber) REFERENCES SerialNumbers (Id)
)
GO

/*
* RevaluationProductsReports таблица отчетов о переоценке
* DataRevaluation дата переоценки
*/
CREATE TABLE RevaluationProductsReports(
	Id INT PRIMARY KEY IDENTITY,
	DataRevaluation DATE NULL
)
GO

/*
* RevaluationProductsInfos таблица информации о переоценке
* IdProduct id продукта
*/
CREATE TABLE RevaluationProductsInfos(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdRevaluationProductsReports INT NOT NULL,
	OldSalesPrice MONEY NOT NULL,
	NewSalesPrice MONEY NOT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdRevaluationProductsReports) REFERENCES RevaluationProductsReports (Id)
)
GO

CREATE TABLE CountProducts(
	IdProduct INT NOT NULL,
	IdStore INT NOT NULL,
	Count FLOAT NOT NULL DEFAULT 0,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id) ON DELETE CASCADE,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id) ON DELETE CASCADE
)
GO

CREATE LOGIN Tester WITH PASSWORD = '12345!', DEFAULT_DATABASE = AutomationAccountingGoods;
CREATE USER Tester FOR LOGIN Tester;  
CREATE LOGIN Tester1 WITH PASSWORD = '12345!', DEFAULT_DATABASE = AutomationAccountingGoods;
CREATE USER Tester1 FOR LOGIN Tester1;  
GO  

CREATE ROLE Seller
GRANT SELECT ON Stores TO Seller
GRANT SELECT ON UnitStorages TO Seller
GRANT SELECT ON Groups TO Seller
GRANT SELECT ON WarrantyPeriods TO Seller
GRANT SELECT ON Products TO Seller
GRANT SELECT ON PropertyNames TO Seller
GRANT SELECT ON PropertyValues TO Seller
GRANT SELECT ON PropertyProducts TO Seller
GRANT SELECT ON Counterparties TO Seller
GRANT SELECT ON SerialNumbers TO Seller
GRANT SELECT ON Warranties TO Seller
GRANT SELECT ON CountProducts TO Seller
GRANT UPDATE ON SerialNumbers TO Seller
GRANT UPDATE ON Warranties TO Seller
GRANT UPDATE ON CountProducts TO Seller
GRANT INSERT ON Warranties TO Seller
GRANT INSERT ON SalesInfos TO Seller
GRANT INSERT ON SalesReports TO Seller
DENY SELECT ON SalesReports TO Seller

CREATE SERVER ROLE db_automationAccountingGoods

ALTER ROLE Seller ADD MEMBER Tester
ALTER ROLE Seller ADD MEMBER Tester1
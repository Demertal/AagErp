USE [master]
GO

DROP DATABASE IF EXISTS AutomationAccountingGoods
GO

CREATE DATABASE AutomationAccountingGoods COLLATE Cyrillic_General_CI_AS

GO

USE AutomationAccountingGoods

/*
* ExchangeRate ������� �����
* Title ������������ ������
* Course ����
* IsDefault �������� ������
*/
CREATE TABLE ExchangeRates(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(10) NOT NULL UNIQUE CHECK(Title !=''),
	Course MONEY NOT NULL CHECK(Course > 0),
	IsDefault BIT NOT NULL DEFAULT 0
)
GO

/*
* Store ������� ���������
* Title ������������ ��������
*/
CREATE TABLE Stores(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(50) NOT NULL UNIQUE CHECK(Title !='')
)
GO

/*
* PriceGroups ������� ������� �����
* Markup �������
*/
CREATE TABLE PriceGroups(
	Id INT PRIMARY KEY IDENTITY,
	Markup FLOAT NOT NULL UNIQUE CHECK(Markup > 0)
)
GO


/*
* UnitStorage ������� ����� ��������
* Title ������������ ����
*/
CREATE TABLE UnitStorages(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !=''),
	IsWeightGoods BIT NOT NULL DEFAULT 0
)
GO

/*
* Group ������
* Title ������������ ������
* IdUnitStorage id ���� ��������
*/
CREATE TABLE Groups(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(50) NOT NULL CHECK(Title !=''),
	IdParentGroup INT,	
	FOREIGN KEY (IdParentGroup) REFERENCES Groups (Id),	
	CONSTRAINT UQ_Group_TitleIdParentGroup UNIQUE (Title, IdParentGroup)
)
GO

/*
* WarrantyPeriod ������� ������ ��������
* Period ����
*/
CREATE TABLE WarrantyPeriods(
	Id INT PRIMARY KEY IDENTITY,
	Period NVARCHAR(20) NOT NULL UNIQUE CHECK(Period != ''),
)
GO

/*
* Product ������� �������
* Title ������������ ������
* VendorCode ��� �������������
* Barcode ��������
* IdWarrantyPeriod id ������������ �����
* IdGroup id ������,
* IdPriceGroup id ������� ������
* IdUnitStorage id ��. ��.
* KeepTrackSerialNumbers ����� ���� �� �������� ������� 
*/
CREATE TABLE Products(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(120) NOT NULL CHECK(Title !='') UNIQUE,
	VendorCode NVARCHAR(20) NULL,
	Barcode NVARCHAR(13) NULL UNIQUE,	
	IdWarrantyPeriod INT NOT NULL,
	IdGroup INT NOT NULL,
	IdPriceGroup INT NOT NULL,
	IdUnitStorage INT NOT NULL,
	KeepTrackSerialNumbers BIT NOT NULL DEFAULT 0,
	FOREIGN KEY (IdUnitStorage) REFERENCES UnitStorages (Id),
	FOREIGN KEY (IdWarrantyPeriod) REFERENCES WarrantyPeriods (Id),
	FOREIGN KEY (IdGroup) REFERENCES Groups (Id),
	FOREIGN KEY (IdPriceGroup) REFERENCES PriceGroups (Id)
)
GO

/*
* PropertyName ������� ������������ ���������� ������
* Title ������������ ���������
* IdGroup id ������
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
* PropertyValue ������� �������� ���������� ������
* Value ������� ���������
* IdPropertyName id ������������ ���������
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
* PropertyProduct ������� �������� ���������� ������
* IdProduct id ��������
* IdPropertyName id ���������
* IdPropertyValue id �������� ���������
*/
CREATE TABLE PropertyProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdPropertyName INT NOT NULL,
	IdPropertyValue INT DEFAULT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdPropertyName) REFERENCES PropertyNames (Id),
	FOREIGN KEY (IdPropertyValue) REFERENCES PropertyValues (Id),
	CONSTRAINT UQ_PropertyProduct_IdProductIdPropertyName UNIQUE (IdProduct, IdPropertyName)
)
GO

/*
* �ounterparties ������� ������������
* Title ������������ �����������
* �ontactPerson ���������� ����
* Props ���������
* Address �����
* WhoIsIt ��� ��� 0 - ���������, 1 - ����������
* Debt ������������
*/
CREATE TABLE Counterparties(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(40) NOT NULL,
	ContactPerson NVARCHAR(40) NULL,
	ContactPhone NVARCHAR(50) NULL,
	Props NVARCHAR(40) NULL,
	Address NVARCHAR(40) NULL,
	WhoIsIt INT NOT NULL,
	Debt MONEY NOT NULL DEFAULT 0,
	CONSTRAINT UQ_Title_WhoIsIt UNIQUE (Title, WhoIsIt)
)
GO

/*
* MovementGoodsReports ������� ������� � �������� ������
* Date ����
* �ourse ����
* TextInfo ���������������� �����
* IdArrivalStore id ������ �����������
* IdDisposalStore id ������� �������
* IdCounterparty id �����������
* IdExchangeRate id ������
* TypeAction ��� ������
*/
CREATE TABLE MovementGoodsReports(
	Id INT PRIMARY KEY IDENTITY,
	Date DateTime NULL,
	Course MONEY NULL CHECK(Course >= 0) DEFAULT 0,
	TextInfo NVARCHAR(50) NULL, 
	IdArrivalStore INT NULL,
	IdDisposalStore INT NULL,
	IdCounterparty INT NULL,
	IdExchangeRate INT NULL,
	TypeAction INT NOT NULL,
	IdMovementGoodsReport INT NULL,
	FOREIGN KEY (IdArrivalStore) REFERENCES Stores (Id),
	FOREIGN KEY (IdDisposalStore) REFERENCES Stores (Id),
	FOREIGN KEY (IdCounterparty) REFERENCES Counterparties (Id),
	FOREIGN KEY (IdExchangeRate) REFERENCES ExchangeRates (Id),
	FOREIGN KEY (IdMovementGoodsReport) REFERENCES MovementGoodsReports (Id)
)
GO

/*
* PurchaseSaleGoodsInfos ������� ���������� �� ������� � �������� ������
* Count ���-��
* Price ����
* IdPurchas id ������
* IdProduct id ��������
*/
CREATE TABLE MovementGoodsInfos(
	Id INT PRIMARY KEY IDENTITY,
	Count INT NOT NULL CHECK (Count > 0),
	Price MONEY NULL CHECK (Price > 0),
	IdReport INT NOT NULL,
	IdProduct INT NOT NULL,
	FOREIGN KEY (IdReport) REFERENCES MovementGoodsReports (Id),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id)
)
GO

/*
* SerialNumber ������� �������� �������
* Value �������� �����
* IdProduct id ��������
* IdSaleReport id ������ � �������
* IdPurchaseReport id ������ � �������
*/
CREATE TABLE SerialNumbers(
	Id INT PRIMARY KEY IDENTITY,
	Value VARCHAR(20) NOT NULL CHECK (Value != ''),
	IdProduct INT NOT NULL,
	IdPurchaseReport INT NOT NULL,
	IdSaleReport INT NULL,
	FOREIGN KEY (IdPurchaseReport) REFERENCES MovementGoodsReports (Id),
	FOREIGN KEY (IdSaleReport) REFERENCES MovementGoodsReports (Id),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id) 
)
GO

/*
* Warranty ������� ������� �� ��������
* Malfunction �������������
* DateReceipt ���� ���������
* DateDeparture ���� ��������
* DateIssue ���� ������
* Info ���� � ����������
* IdSerialNumber id ��������� ������
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
* RevaluationProductsReports ������� ������� � ����������
* DateRevaluation ���� ����������
*/
CREATE TABLE RevaluationProductsReports(
	Id INT PRIMARY KEY IDENTITY,
	DateRevaluation DateTime NULL
)
GO

/*
* PriceProducts ������� ��� �������
* IdProduct id ��������
* Price ���� �������
* IdRevaluationProductsReports id ������ � �������
*/
CREATE TABLE PriceProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdRevaluationProductsReports INT NOT NULL,
	Price MONEY NOT NULL CHECK (Price > 0),
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdRevaluationProductsReports) REFERENCES RevaluationProductsReports (Id)
)
GO


/*
* InvoiceReport ������� ������
*/
CREATE TABLE InvoiceReport(
	Id INT PRIMARY KEY IDENTITY
)
GO

/*
* InvoiceInfos ������� ���� � �����
*/
CREATE TABLE InvoiceInfos(	
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	Count FLOAT NOT NULL CHECK(Count > 0),
	IdInvoiceReport INT NOT NULL,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id),
	FOREIGN KEY (IdInvoiceReport) REFERENCES InvoiceReport (Id)
)
GO

CREATE TABLE CountProducts(
	Id INT PRIMARY KEY IDENTITY,
	IdProduct INT NOT NULL,
	IdStore INT NOT NULL,
	Count FLOAT NOT NULL DEFAULT 0,
	FOREIGN KEY (IdProduct) REFERENCES Products (Id) ON DELETE CASCADE,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id) ON DELETE CASCADE
)
GO

/*
* MoneyTransfers ������� �������� �������� �������
* IdCounterparty id �����������
* MoneyAmount �����
* TypeTransfer ��� ��������
*/
CREATE TABLE MoneyTransfers(
	Id INT PRIMARY KEY IDENTITY,
	Date DateTime NULL,
	IdCounterparty INT NOT NULL,
	MoneyAmount MONEY NOT NULL CHECK(MoneyAmount > 0),
	TypeTransfer INT NOT NULL,
	FOREIGN KEY (IdCounterparty) REFERENCES Counterparties (Id),
)
GO

--CREATE LOGIN Tester1 WITH PASSWORD = '12345!', DEFAULT_DATABASE = AutomationAccountingGoods;
--CREATE USER Tester1 FOR LOGIN Tester1;
--CREATE LOGIN Tester2 WITH PASSWORD = '12345!', DEFAULT_DATABASE = AutomationAccountingGoods;
--CREATE USER Tester2 FOR LOGIN Tester2;  
--CREATE LOGIN Tester3 WITH PASSWORD = '12345!', DEFAULT_DATABASE = AutomationAccountingGoods;
--CREATE USER Tester3 FOR LOGIN Tester3;    
--GO  


----ExchangeRates Stores UnitStorages Groups WarrantyPeriods
----Products PropertyNames PropertyValues PropertyProducts
----Counterparties PurchaseSaleGoodsReports PurchaseSaleGoodsInfos 
----SerialNumbers Warranties RevaluationProductsReports
----PriceProducts CountProducts

--CREATE ROLE Seller
--GRANT SELECT ON Stores TO Seller
--GRANT SELECT ON UnitStorages TO Seller
--GRANT SELECT ON Groups TO Seller
--GRANT SELECT ON WarrantyPeriods TO Seller
--GRANT SELECT ON Products TO Seller
--GRANT SELECT ON PropertyNames TO Seller
--GRANT SELECT ON PropertyValues TO Seller
--GRANT SELECT ON PropertyProducts TO Seller
--GRANT SELECT ON Counterparties TO Seller
--GRANT SELECT ON SerialNumbers TO Seller
--GRANT UPDATE ON SerialNumbers TO Seller
--GRANT SELECT ON PurchaseSaleGoodsReports TO Seller
--GRANT INSERT ON PurchaseSaleGoodsReports TO Seller
--GRANT SELECT ON PurchaseSaleGoodsInfos TO Seller
--GRANT INSERT ON PurchaseSaleGoodsInfos TO Seller
--GRANT SELECT ON CountProducts TO Seller
--GRANT UPDATE ON CountProducts TO Seller
--GRANT SELECT ON PriceProducts TO Seller

--CREATE ROLE OldestSalesman
--GRANT SELECT ON ExchangeRates TO OldestSalesman
--GRANT SELECT ON Stores TO OldestSalesman
--GRANT SELECT ON UnitStorages TO OldestSalesman
--GRANT SELECT ON Groups TO OldestSalesman
--GRANT SELECT ON WarrantyPeriods TO OldestSalesman
--GRANT SELECT ON Products TO OldestSalesman
--GRANT UPDATE ON Products TO OldestSalesman
--GRANT SELECT ON PropertyNames TO OldestSalesman
--GRANT SELECT ON PropertyValues TO OldestSalesman
--GRANT SELECT ON PropertyProducts TO OldestSalesman
--GRANT SELECT ON Counterparties TO OldestSalesman
--GRANT INSERT ON PurchaseSaleGoodsReports TO OldestSalesman
--GRANT SELECT ON PurchaseSaleGoodsReports TO OldestSalesman
--GRANT INSERT ON PurchaseSaleGoodsInfos TO OldestSalesman
--GRANT SELECT ON PurchaseSaleGoodsInfos TO OldestSalesman
--GRANT SELECT ON SerialNumbers TO OldestSalesman
--GRANT INSERT ON SerialNumbers TO OldestSalesman
--GRANT SELECT ON Warranties TO OldestSalesman
--GRANT INSERT ON Warranties TO OldestSalesman
--GRANT UPDATE ON Warranties TO OldestSalesman
--GRANT SELECT ON RevaluationProductsReports TO OldestSalesman
--GRANT INSERT ON RevaluationProductsReports TO OldestSalesman
--GRANT SELECT ON PriceProducts TO OldestSalesman
--GRANT INSERT ON PriceProducts TO OldestSalesman
--GRANT SELECT ON CountProducts TO OldestSalesman
--GRANT EXECUTE ON GoodsShipped TO OldestSalesman
--GRANT EXECUTE ON GoodsIssued TO OldestSalesman

--CREATE ROLE Admin
--GRANT SELECT ON ExchangeRates TO Admin
--GRANT UPDATE ON ExchangeRates TO Admin
--GRANT SELECT ON UnitStorages TO Admin
--GRANT INSERT ON UnitStorages TO Admin
--GRANT DELETE ON UnitStorages TO Admin
--GRANT UPDATE ON UnitStorages TO Admin
--GRANT SELECT ON Groups TO Admin
--GRANT INSERT ON Groups TO Admin
--GRANT DELETE ON Groups TO Admin
--GRANT UPDATE ON Groups TO Admin
--GRANT SELECT ON Stores TO Admin
--GRANT INSERT ON Stores TO Admin
--GRANT DELETE ON Stores TO Admin
--GRANT UPDATE ON Stores TO Admin
--GRANT SELECT ON WarrantyPeriods TO Admin
--GRANT INSERT ON WarrantyPeriods TO Admin
--GRANT DELETE ON WarrantyPeriods TO Admin
--GRANT UPDATE ON WarrantyPeriods TO Admin
--GRANT SELECT ON Products TO Admin
--GRANT INSERT ON Products TO Admin
--GRANT DELETE ON Products TO Admin
--GRANT UPDATE ON Products TO Admin
--GRANT SELECT ON PropertyNames TO Admin
--GRANT INSERT ON PropertyNames TO Admin
--GRANT DELETE ON PropertyNames TO Admin
--GRANT UPDATE ON PropertyNames TO Admin
--GRANT SELECT ON PropertyValues TO Admin
--GRANT INSERT ON PropertyValues TO Admin
--GRANT DELETE ON PropertyValues TO Admin
--GRANT UPDATE ON PropertyValues TO Admin
--GRANT SELECT ON PropertyProducts TO Admin
--GRANT INSERT ON PropertyProducts TO Admin
--GRANT DELETE ON PropertyProducts TO Admin
--GRANT UPDATE ON PropertyProducts TO Admin
--GRANT SELECT ON Counterparties TO Admin
--GRANT INSERT ON Counterparties TO Admin
--GRANT DELETE ON Counterparties TO Admin
--GRANT UPDATE ON Counterparties TO Admin
--GRANT SELECT ON PurchaseSaleGoodsReports TO Admin
--GRANT SELECT ON PurchaseSaleGoodsInfos TO Admin
--GRANT SELECT ON SerialNumbers TO Admin
--GRANT SELECT ON RevaluationProductsReports TO Admin
--GRANT SELECT ON PriceProducts TO Admin
--GRANT SELECT ON CountProducts TO Admin


--ALTER ROLE Seller ADD MEMBER Tester1
--ALTER ROLE OldestSalesman ADD MEMBER Tester2
--ALTER ROLE Admin ADD MEMBER Tester3
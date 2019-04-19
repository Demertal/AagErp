USE [master]
GO

DROP DATABASE IF EXISTS Store
GO

CREATE DATABASE Store
GO

USE Store

/*
* ExchangeRate ������� �����
* Title ������������ ������
* �ourse ����
*/
CREATE TABLE ExchangeRates(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(10) NOT NULL UNIQUE CHECK(Title !=''),
	�ourse FLOAT NOT NULL UNIQUE CHECK(�ourse >= 0),
	CONSTRAINT UQ_ExchangeRate_Currency�ourse UNIQUE (Title, �ourse)
)
GO

/*
* Store ������� ���������
* Title ������������ ��������
*/
CREATE TABLE Stores(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !='')
)
GO


/*
* UnitStorage ������� ����� ��������
* Title ������������ ����
*/
CREATE TABLE UnitStorages(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !=''),
)
GO

/*
* Group ������
* Title ������������ ������
* IdUnitStorage id ���� ��������
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
* PriceGroup ������� �������
* Markup �������
*/
CREATE TABLE PriceGroups(
	Id INT PRIMARY KEY IDENTITY,
	Markup float NOT NULL UNIQUE CHECK(Markup >= 0),
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
* PurchasePrice ���������� ����
* SalesPrice ���� �������
* IdExchangeRate id ������ ��� ���������� ����
* IdWarrantyPeriod id ������������ �����
* IdGroup id ������
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
* IdPropertyValue id �������� ���������
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
* Supplier ������� �����������
* Title ���������
* IdStore id ��������
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
* PurchaseReport ������� ������� � �������
* DataOrder ���� �������
* IdStore id ��������
* IdSupplier id ����������
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
* PurchaseInfo ������� ���������� �� ������� � �������
* Count ���-��
* PurchasePrice ���� �������
* IdPurchaseReport id ������ � �������
* IdProduct id ��������
* IdExchangeRate id ������
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
* SerialNumber ������� �������� �������
* Value �������� �����
* SelleDate ���� �������
* PurchaseDate ���� �������
* IdProduct id ��������
* IdSupplier id ����������
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
* SalesReport ������� ������� � �������
* DataSales ���� �������
* IdStore id ��������
*/
CREATE TABLE SalesReports(
	Id INT PRIMARY KEY IDENTITY,
	DataSales DATE NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Stores (Id)
)
GO

/*
* SalesInfo ������� ���������� �� ������� � �������
* Count ���-��
* SellingPrice ���� �������
* IdSalesReport id ������ � �������
* IdProduct id ��������
* IdSerialNumber id ��������� ������
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
* Warranty ������� ������� �� ��������
* Malfunction �������������
* DateReceipt ���� ���������
* DateDeparture ���� ��������
* DateIssue ���� ������
* Info ���� � ����������
* IdSupplier id ����������
* IdSerialNumber id ��������� ������
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
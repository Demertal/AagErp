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
CREATE TABLE ExchangeRate(
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
CREATE TABLE Store(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !='')
)
GO

/*
* NomenclatureGroup ������� �������������� �����
* Title ������������ ������
* IdStore id ��������
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
* PriceGroup ������� �������
* Markup �������
*/
CREATE TABLE PriceGroup(
	Id INT PRIMARY KEY IDENTITY,
	Markup float NOT NULL UNIQUE CHECK(Markup >= 0),
)
GO

/*
* NomenclatureSubGroup ������� �������������� ��������
* Title ������������ ������
* IdNomenclatureGroup id �������������� ������
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
* UnitStorage ������� ����� ��������
* Title ������������ ����
*/
CREATE TABLE UnitStorage(
	Id INT PRIMARY KEY IDENTITY,
	Title NVARCHAR(20) NOT NULL UNIQUE CHECK(Title !=''),
)
GO

/*
* WarrantyPeriod ������� ������ ��������
* Period ����
*/
CREATE TABLE WarrantyPeriod(
	Id INT PRIMARY KEY IDENTITY,
	Period INT NOT NULL UNIQUE CHECK(Period >= 0),
)
GO

/*
* Product ������� �������
* Title ������������ ������
* VendorCode ��� �������������
* Barcode ��������
* Count ���-��
* PurchasePrice ���������� ����
* SalesPrice ���� �������
* IdNomenclatureSubGroup id �������������� ���������
* IdUnitStorage id ���� ��������
* IdExchangeRate id ������ ��� ���������� ����
* IdWarrantyPeriod id ������������ �����
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
* PropertyName ������� ������������ ���������� ������
* Title ������������ ���������
* IdNomenclatureSubGroup id �������������� ������
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
* PropertyValue ������� �������� ���������� ������
* Value ������� ���������
* IdPropertyName id ������������ ���������
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
* PropertyProduct ������� �������� ���������� ������
* IdProduct id ��������
* IdPropertyValue id �������� ���������
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
* Supplier ������� �����������
* Title ���������
* IdStore id ��������
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
* PurchaseReport ������� ������� � �������
* DataOrder ���� �������
* IdStore id ��������
* IdSupplier id ����������
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
* PurchaseInfo ������� ���������� �� ������� � �������
* Count ���-��
* PurchasePrice ���� �������
* IdPurchaseReport id ������ � �������
* IdProduct id ��������
* IdExchangeRate id ������
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
* SerialNumber ������� �������� �������
* Value �������� �����
* SelleDate ���� �������
* PurchaseDate ���� �������
*/
CREATE TABLE SerialNumber(
	Id INT PRIMARY KEY IDENTITY,
	Value VARCHAR(20) NOT NULL UNIQUE CHECK (Value != ''),
	SelleDate DATE NULL,	
	PurchaseDate DATE NOT NULL
)
GO

/*
* SalesReport ������� ������� � �������
* DataSales ���� �������
* IdStore id ��������
*/
CREATE TABLE SalesReport(
	Id INT PRIMARY KEY IDENTITY,
	DataSales DATE NOT NULL,
	IdStore INT NOT NULL,
	FOREIGN KEY (IdStore) REFERENCES Store (Id)
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
* Warranty ������� ������� �� ��������
* Malfunction �������������
* DateReceipt ���� ���������
* DateDeparture ���� ��������
* DateIssue ���� ������
* Info ���� � ����������
* IdSupplier id ����������
* IdSerialNumber id ��������� ������
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
USE PM
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.Customer','U') IS NOT NULL
	DROP TABLE dbo.Customer
CREATE TABLE dbo.Customer
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	INSERT_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	INSERT_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	IsActive BIT NOT NULL DEFAULT 1,
	
	OpeningDate DATE NULL,
	CustomerType NVARCHAR(100) NOT NULL,
	CustomerName NVARCHAR(1000) NOT NULL,
	
	-- Personal Customers
	Personal_Gender NVARCHAR(100),
	Personal_BirthDate DATE,
	Personal_SSN NVARCHAR(100),
	Personal_LicenseID NVARCHAR(100),

	-- Business Customers
	Business_TypeOfCompany NVARCHAR(100),
	Business_TaxID NVARCHAR(100),

	Notes NVARCHAR(4000)
)
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.Contacts','U') IS NOT NULL
	DROP TABLE dbo.Contacts
CREATE TABLE dbo.Contacts
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	IsActive BIT NOT NULL DEFAULT 1,

	CustomerID INT NOT NULL,
	ContactItemType NVARCHAR(100) NOT NULL,
	ContactItemValue NVARCHAR(1000) NOT NULL,
)
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.DocumentFolders','U') IS NOT NULL
	DROP TABLE dbo.DocumentFolders
CREATE TABLE dbo.DocumentFolders
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	IsActive BIT NOT NULL DEFAULT 1,

	CustomerID INT NOT NULL,
	ParentID INT,
	FolderName NVARCHAR(1000) NOT NULL,
	IsStarred BIT NOT NULL DEFAULT 0,
	IsHidden BIT NOT NULL DEFAULT 0
)
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.Lookups','U') IS NOT NULL
	DROP TABLE dbo.Lookups
CREATE TABLE dbo.Lookups
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	LookupType NVARCHAR(100) NOT NULL,
	SortOrder INT NOT NULL,
	LookupName NVARCHAR(100) NOT NULL,
)
GO
INSERT INTO dbo.Lookups (LookupType, SortOrder, LookupName) VALUES
('CustomerType', 1, 'Personal'),
('CustomerType', 2, 'Business'),

('ContactItemType', 1, 'Contact Name'),
('ContactItemType', 2, 'Email'),
('ContactItemType', 3, 'Mobile Number'),
('ContactItemType', 4, 'Business Phone'),
('ContactItemType', 5, 'Other Phone Number'),
('ContactItemType', 6, 'Fax'),
('ContactItemType', 7, 'Address'),
('ContactItemType', 8, 'Website'),
('ContactItemType', 9, 'Other Contacts'),

('TypeOfCompany', 1, 'Taxi/Limo'),
('TypeOfCompany', 2, 'Tax/Accounting'),
('TypeOfCompany', 3, 'Restaurant'),
('TypeOfCompany', 4, 'Retail'),
('TypeOfCompany', 5, 'Grocery'),
('TypeOfCompany', 6, 'Others'),

('ServiceType', 1, 'Accounting'),
('ServiceType', 2, 'Immigration'),
('ServiceType', 3, 'Insurance'),
('ServiceType', 4, 'Multi-Services'),
('ServiceType', 5, 'Tax'),

('DefaultFolder', 1, 'Accounting'),
('DefaultFolder', 2, 'Immigration'),
('DefaultFolder', 3, 'Insurance'),
('DefaultFolder', 4, 'Tax'),
('DefaultFolder', 5, 'Personal Documents'),
('DefaultFolder', 6, 'Financial Documents'),
('DefaultFolder', 7, 'Other Legal'),
('DefaultFolder', 8, 'Miscellaneous')

GO


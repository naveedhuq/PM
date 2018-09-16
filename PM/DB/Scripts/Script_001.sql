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
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Customer TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.Contacts','U') IS NOT NULL
	DROP TABLE dbo.Contacts
CREATE TABLE dbo.Contacts
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	INSERT_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	INSERT_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	IsActive BIT NOT NULL DEFAULT 1,

	CustomerID INT NOT NULL,
	ContactItemType NVARCHAR(100) NOT NULL,
	ContactItemValue NVARCHAR(1000) NOT NULL,
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Contacts TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.DocumentFolders','U') IS NOT NULL
	DROP TABLE dbo.DocumentFolders
CREATE TABLE dbo.DocumentFolders
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	INSERT_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	INSERT_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	IsActive BIT NOT NULL DEFAULT 1,

	CustomerID INT NOT NULL,
	ParentID INT,
	FolderName NVARCHAR(1000) NOT NULL,
	IsStarred BIT NOT NULL DEFAULT 0,
	IsHidden BIT NOT NULL DEFAULT 0
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.DocumentFolders TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.Documents','U') IS NOT NULL
	DROP TABLE dbo.Documents
CREATE TABLE dbo.Documents
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	INSERT_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	INSERT_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	IsActive BIT NOT NULL DEFAULT 1,
	
	CustomerID INT NOT NULL,
	DocumentFolderID INT, 
	DocumentFileName NVARCHAR(100) NOT NULL,
	DocumentType NVARCHAR(100),
	UploadDate DATE,
	ExpirationDate DATE,
	Comments NVARCHAR(1000)
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Documents TO PUBLIC
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
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Lookups TO PUBLIC
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
('DefaultFolder', 8, 'Miscellaneous'),

('ExtensionToImageMapping', 1, '.pdf|IconPDF'),
('ExtensionToImageMapping', 2, '.xls|IconExcel'),
('ExtensionToImageMapping', 3, '.xlsx|IconExcel'),
('ExtensionToImageMapping', 4, '.doc|IconWord'),
('ExtensionToImageMapping', 5, '.docx|IconWord'),
('ExtensionToImageMapping', 6, '.rtf|IconWord'),
('ExtensionToImageMapping', 7, '.ppt|IconPowerPoint'),
('ExtensionToImageMapping', 8, '.pptx|IconPowerPoint'),
('ExtensionToImageMapping', 9, '.txt|IconPowerText'),
('ExtensionToImageMapping', 10, '.jpg|IconImage'),
('ExtensionToImageMapping', 11, '.jpeg|IconImage'),
('ExtensionToImageMapping', 12, '.gif|IconImage'),
('ExtensionToImageMapping', 13, '.png|IconImage'),
('ExtensionToImageMapping', 14, '.tiff|IconImage'),
('ExtensionToImageMapping', 15, '.gif|IconImage'),
('ExtensionToImageMapping', 16, '.bmp|IconImage')

GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.AppSettings','U') IS NOT NULL
	DROP TABLE dbo.AppSettings
CREATE TABLE dbo.AppSettings
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	SettingsName NVARCHAR(100) NOT NULL UNIQUE,
	SettingsValue NVARCHAR(1000)
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.AppSettings TO PUBLIC
GO
INSERT INTO dbo.AppSettings(SettingsName, SettingsValue) VALUES
('SPECIAL_FOLDERNAME_ALL', 'ALL'),
('SPECIAL_FOLDERNAME_UNCATEGORIZED', 'Un-Categorized')
GO




-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_CreateDefaultDocumentFolders','P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CreateDefaultDocumentFolders
GO
CREATE PROCEDURE dbo.sp_CreateDefaultDocumentFolders
	@CustomerID INT
AS
BEGIN
	WITH cte AS
	(
		SELECT SortOrder, LookupName FolderName FROM dbo.Lookups
		WHERE LookupType='DefaultFolder'
	)
	MERGE dbo.DocumentFolders AS t
	USING (SELECT @CustomerID, FolderName FROM cte) AS s (CustomerID, FolderName)
	ON t.CustomerID=s.CustomerID AND t.FolderName=s.FolderName AND t.IsActive=1
	WHEN NOT MATCHED THEN
	INSERT (CustomerID, FolderName)
	VALUES (s.CustomerID, s.FolderName);
END
GO
GRANT EXEC ON dbo.sp_CreateDefaultDocumentFolders TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_GetDocumentFolderCountForCustomer','FN') IS NOT NULL
	DROP FUNCTION dbo.fn_GetDocumentFolderCountForCustomer
GO
CREATE FUNCTION dbo.fn_GetDocumentFolderCountForCustomer(@CustomerID INT)
RETURNS INT AS
BEGIN
	DECLARE @ret INT
	SELECT @ret = COUNT(*) FROM dbo.DocumentFolders WHERE CustomerID=@CustomerID
	RETURN @ret
END
GO
GRANT EXECUTE ON dbo.fn_GetDocumentFolderCountForCustomer TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SaveDocumentFolders','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SaveDocumentFolders
GO
CREATE PROCEDURE dbo.sp_SaveDocumentFolders
    @ID INT,
    @CustomerID INT,
    @ParentID INT,
    @FolderName NVARCHAR(1000),
    @IsStarred BIT,
    @IsHidden BIT
AS
BEGIN
    IF EXISTS(SELECT * FROM dbo.DocumentFolders WHERE ID=@ID)
        UPDATE dbo.DocumentFolders
        SET CustomerID=@CustomerID,
            ParentID=@ParentID,
            FolderName=@FolderName,
            IsStarred=@IsStarred,
            IsHidden=@IsHidden
        WHERE ID=@ID
    ELSE
        INSERT INTO dbo.DocumentFolders (CustomerID,ParentID,FolderName,IsStarred,IsHidden)
        VALUES (@CustomerID,@ParentID,@FolderName,@IsStarred,@IsHidden)

    RETURN SCOPE_IDENTITY()
END
GO
GRANT EXECUTE ON dbo.sp_SaveDocumentFolders TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_DeleteDocumentFolder','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeleteDocumentFolder
GO
CREATE PROCEDURE dbo.sp_DeleteDocumentFolder
    @ID INT
AS
BEGIN
	UPDATE dbo.DocumentFolders
	SET IsActive = 0
	WHERE ID=@ID
END
GO
GRANT EXECUTE ON dbo.sp_DeleteDocumentFolder TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_GetDocumentFoldersForCustomer','IF') IS NOT NULL
	DROP FuNCTION dbo.fn_GetDocumentFoldersForCustomer
GO
CREATE FUNCTION dbo.fn_GetDocumentFoldersForCustomer(@CustomerID INT)
RETURNS TABLE AS RETURN
(
	SELECT *
	FROM dbo.DocumentFolders
	WHERE IsActive=1
	AND CustomerID=@CustomerID
)
GO
GRANT SELECT ON dbo.fn_GetDocumentFoldersForCustomer TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_GetDocumentsForCustomer','IF') IS NOT NULL
	DROP FuNCTION dbo.fn_GetDocumentsForCustomer
GO
CREATE FUNCTION dbo.fn_GetDocumentsForCustomer(@CustomerID INT)
RETURNS TABLE AS RETURN
(
	SELECT *
	FROM dbo.Documents
	WHERE IsActive=1
	AND CustomerID=@CustomerID
)
GO
GRANT SELECT ON dbo.fn_GetDocumentsForCustomer TO PUBLIC
GO
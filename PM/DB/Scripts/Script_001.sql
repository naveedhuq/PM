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
	FileTimestamp DATETIME,
	UploadDate DATE,
	ExpirationDate DATE,
	Comments NVARCHAR(1000)
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Documents TO PUBLIC
GO

-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.DocumentData','U') IS NOT NULL
	DROP TABLE dbo.DocumentData
CREATE TABLE dbo.DocumentData
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	INSERT_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	INSERT_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	IsActive BIT NOT NULL DEFAULT 1,
	
	DocumentID INT NOT NULL,
	RawData VARBINARY(MAX)
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.DocumentData TO PUBLIC
GO

-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.EventLog','U') IS NOT NULL
	DROP TABLE dbo.EventLog
CREATE TABLE dbo.EventLog
(
	ID INT IDENTITY(1,1) PRIMARY KEY CLUSTERED,
	LOG_TIMESTAMP DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	LOG_USER NVARCHAR(100) NOT NULL DEFAULT SUSER_SNAME(),
	
	EventType NVARCHAR(100) NOT NULL,
	EventMessage NVARCHAR(1000)
)
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.EventLog TO PUBLIC
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
('ExtensionToImageMapping', 16, '.bmp|IconImage'),

('DocumentType', 1, 'Driver''s License'),
('DocumentType', 2, 'Social Security'),
('DocumentType', 3, 'Tax Returns'),
('DocumentType', 4, 'Passport'),
('DocumentType', 5, 'Bank Statements'),
('DocumentType', 6, 'Insurance Document'),
('DocumentType', 7, 'W-2'),
('DocumentType', 8, 'Paystub'),
('DocumentType', 9, 'Property Records'),
('DocumentType', 10, 'Car Documents'),
('DocumentType', 11, 'Business Documents'),
('DocumentType', 12, 'Other Documents'),

('FileType', 1, 'Word/RTF'),
('FileType', 2, 'Excel'),
('FileType', 3, 'PDF'),
('FileType', 4, 'Image'),
('FileType', 5, 'Text'),
('FileType', 6, 'Other')

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
IF OBJECT_ID('dbo.sp_SaveDocumentFolder','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SaveDocumentFolder
GO
CREATE PROCEDURE dbo.sp_SaveDocumentFolder
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
GRANT EXECUTE ON dbo.sp_SaveDocumentFolder TO PUBLIC
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
CREATE FUNCTION dbo.fn_GetDocumentsForCustomer(@CustomerID INT, @ActiveOnly BIT = 1)
RETURNS TABLE AS RETURN
(
	SELECT
		d.ID,
		d.IsActive,
		d.CustomerID,
		(CASE
		   WHEN d.DocumentFolderID IS NULL THEN -1
		   WHEN NOT EXISTS(SELECT * FROM dbo.DocumentFolders WHERE CustomerID=@CustomerID AND IsActive=1 AND ID=d.DocumentFolderID) THEN -1
		   ELSE d.DocumentFolderID
		END) DocumentFolderID,
		d.DocumentFileName,
		d.DocumentType,
		d.FileTimestamp,
		d.UploadDate,
		d.ExpirationDate,
		d.Comments
	FROM dbo.Documents d
	WHERE (CASE WHEN @ActiveOnly=1 THEN d.IsActive ELSE 1 END)=1
	AND d.CustomerID=@CustomerID
)
GO
GRANT SELECT ON dbo.fn_GetDocumentsForCustomer TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SaveDocument','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SaveDocument
GO
CREATE PROCEDURE dbo.sp_SaveDocument
    @ID INT,
    @CustomerID INT,
    @DocumentFolderID INT,
    @DocumentFileName NVARCHAR(100),
    @DocumentType NVARCHAR(100),
	@FileTimestamp DATETIME,
    @UploadDate DATE,
	@ExpirationDate DATE,
	@Comments NVARCHAR(1000)
AS
BEGIN
    IF EXISTS(SELECT * FROM dbo.Documents WHERE ID=@ID)
        UPDATE dbo.Documents
        SET CustomerID=@CustomerID,
            DocumentFolderID=@DocumentFolderID,
            DocumentFileName=@DocumentFileName,
			DocumentType=@DocumentType,
			FileTimestamp=@FileTimestamp,
            UploadDate=@UploadDate,
            ExpirationDate=@ExpirationDate,
			Comments=@Comments
        WHERE ID=@ID
    ELSE
        INSERT INTO dbo.Documents (CustomerID, DocumentFolderID, DocumentFileName, DocumentType, FileTimestamp, UploadDate, ExpirationDate, Comments)
        VALUES (@CustomerID, @DocumentFolderID, @DocumentFileName, @DocumentType, @FileTimestamp, @UploadDate, @ExpirationDate, @Comments)
    RETURN SCOPE_IDENTITY()
END
GO
GRANT EXECUTE ON dbo.sp_SaveDocument TO PUBLIC
GO

-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_DeleteDocument','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DeleteDocument
GO
CREATE PROCEDURE dbo.sp_DeleteDocument
    @ID INT
AS
BEGIN
	UPDATE dbo.Documents
	SET IsActive = 0
	WHERE ID=@ID

	UPDATE dbo.DocumentData
	SET IsActive = 0
	WHERE DocumentID=@ID
END
GO
GRANT EXECUTE ON dbo.sp_DeleteDocument TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_UploadDocumentData','P') IS NOT NULL
	DROP PROCEDURE dbo.sp_UploadDocumentData
GO
CREATE PROCEDURE dbo.sp_UploadDocumentData
	@DocumentID INT,
	@FileName NVARCHAR(255)
AS
BEGIN
	IF EXISTS (SELECT * FROM dbo.DocumentData WHERE DocumentID=@DocumentID AND IsActive=1)
		UPDATE dbo.DocumentData SET IsActive=0 WHERE DocumentID=@DocumentID

	DECLARE @sql NVARCHAR(1000) = '
	INSERT INTO dbo.DocumentData(DocumentID, RawData)
	SELECT '+CAST(@DocumentID AS NVARCHAR(10))+', *
	FROM OPENROWSET(BULK '''+ @FileName +''', SINGLE_BLOB) AS x'
	EXEC sp_executesql @sql
	RETURN SCOPE_IDENTITY()
END
GO
GRANT EXECUTE ON sp_UploadDocumentData TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SaveDocumentData','P') IS NOT NULL
	DROP PROCEDURE dbo.sp_SaveDocumentData
GO
CREATE PROCEDURE dbo.sp_SaveDocumentData
	@DocumentID INT,
	@RawData VARBINARY(MAX)
AS
BEGIN
	IF EXISTS (SELECT * FROM dbo.DocumentData WHERE DocumentID=@DocumentID AND IsActive=1)
		UPDATE dbo.DocumentData SET RawData=@RawData WHERE DocumentID=@DocumentID
	ELSE
		INSERT INTO	dbo.DocumentData (DocumentID, RawData)
		VALUES (@DocumentID, @RawData)

	RETURN SCOPE_IDENTITY()
END
GO
GRANT EXECUTE ON sp_SaveDocumentData TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_GetRawDocumentData','FN') IS NOT NULL
	DROP FUNCTION dbo.fn_GetRawDocumentData
GO
CREATE FUNCTION dbo.fn_GetRawDocumentData(@DocumentID INT)
RETURNS VARBINARY(MAX) AS
BEGIN
	DECLARE @ret VARBINARY(MAX)
	SELECT @ret=RawData FROM dbo.DocumentData WHERE DocumentID=@DocumentID AND IsActive=1
	RETURN @ret
END
GO
GRANT EXECUTE ON dbo.fn_GetRawDocumentData TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.fn_GetAllFolderNames','IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetAllFolderNames
GO
CREATE FUNCTION dbo.fn_GetAllFolderNames(@ActiveOnly BIT = 1)
RETURNS TABLE AS RETURN
(
    SELECT DISTINCT FolderName
    FROM dbo.DocumentFolders
    WHERE (CASE WHEN @ActiveOnly=1 THEN IsActive ELSE 1 END)=1
)
GO
GRANT SELECT ON dbo.fn_GetAllFolderNames TO PUBLIC
GO


-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SyncDocumentType','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SyncDocumentType
GO
CREATE PROCEDURE dbo.sp_SyncDocumentType
AS
BEGIN
    DECLARE @MaxSortOrder INT = (SELECT MAX(SortOrder) FROM dbo.Lookups WHERE LookupType='DocumentType')

    ;WITH cte AS
    (
        SELECT DISTINCT ID, DocumentType
        FROM dbo.Documents
        WHERE DocumentType NOT IN (SELECT LookupName FROM dbo.Lookups WHERE LookupType='DocumentType')
    )
    INSERT INTO dbo.Lookups (LookupType, SortOrder, LookupName)
    SELECT
        'DocumentType' LookupType,
        (ROW_NUMBER() OVER(ORDER BY ID))+@MaxSortOrder AS SortOrder,
        DocumentType LookupName    
    FROM cte
END
GO
GRANT EXECUTE ON dbo.sp_SyncDocumentType TO PUBLIC
GO

-----------------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_AddEventLog','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AddEventLog
GO
CREATE PROCEDURE dbo.sp_AddEventLog
    @EventType NVARCHAR(100),
    @EventMessage NVARCHAR(1000)
AS
BEGIN
    INSERT INTO dbo.EventLog (EventType, EventMessage)
    VALUES (@EventType, @EventMessage)
END
GO

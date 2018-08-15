USE PM
GO

-----------------------------------------------------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#jr_personal') IS NOT NULL
	DROP TABLE #jr_personal
CREATE TABLE #jr_personal
(
	Id INT,
	customer_name NVARCHAR(100),
	opening_date DATE,
	gender NVARCHAR(100),
	birthday DATE,
	ssn NVARCHAR(100),
	drivers_license NVARCHAR(100),
	email_address NVARCHAR(100),
	phone NVARCHAR(100),
	[address] NVARCHAR(100)
)
BULK INSERT #jr_personal FROM 'D:\Projects\PM\PM\DB\Scripts\jr_personal.txt'
WITH
(
    FIRSTROW = 2,
    FIELDTERMINATOR = '|',
    ROWTERMINATOR = '\n',
    TABLOCK
)


IF OBJECT_ID('tempdb..#jr_corp') IS NOT NULL
	DROP TABLE #jr_corp
CREATE TABLE #jr_corp
(
	Id INT,
	company_name NVARCHAR(100),
	contact_name NVARCHAR(100),
	contact_email NVARCHAR(100),
	contact_phone NVARCHAR(100),
	[address] NVARCHAR(100),
	tax_id NVARCHAR(100),
	open_date DATE,
	fax NVARCHAR(100),
)
BULK INSERT #jr_corp FROM 'D:\Projects\PM\PM\DB\Scripts\jr_corp.txt'
WITH
(
    FIRSTROW = 2,
    FIELDTERMINATOR = '|',
    ROWTERMINATOR = '\n'
)


-----------------------------------------------------------------------------------------------------------------------
TRUNCATE TABLE dbo.Customer
TRUNCATE TABLE dbo.Contacts
SET IDENTITY_INSERT dbo.Customer ON


INSERT INTO dbo.Customer(ID, OpeningDate, CustomerType, CustomerName, Personal_Gender, Personal_BirthDate, Personal_SSN, Personal_LicenseID)
SELECT Id, opening_date, 'Personal', customer_name, gender, birthday, ssn, drivers_license FROM #jr_personal

INSERT INTO dbo.Contacts (CustomerID, ContactItemType, ContactItemValue)
SELECT *
FROM
(
	SELECT Id CustomerID, 'Address' ContactItemType, [address] ContactItemValue FROM #jr_personal
	UNION ALL
	SELECT Id, 'Email', email_address FROM #jr_personal
	UNION ALL
	SELECT Id, 'Mobile Number', phone FROM #jr_personal
) a
ORDER BY 1, 2


INSERT INTO dbo.Customer(ID, OpeningDate, CustomerType, CustomerName, Business_TypeOfCompany, Business_TaxID)
SELECT
	Id,
	open_date,
	'Business',
	company_name,
	CASE CAST(RAND()*6+1 AS INT)
		WHEN 1 THEN 'Taxi/Limo'
		WHEN 2 THEN 'Tax/Accounting'
		WHEN 3 THEN 'Restaurant'
		WHEN 4 THEN 'Retail'
		WHEN 5 THEN 'Grocery'
		WHEN 6 THEN 'Others'
	END,
	tax_id
FROM #jr_corp

INSERT INTO dbo.Contacts (CustomerID, ContactItemType, ContactItemValue)
SELECT *
FROM
(
	SELECT Id CustomerID, 'Address' ContactItemType, [address] ContactItemValue FROM #jr_corp
	UNION ALL
	SELECT Id, 'Contact Name', contact_name FROM #jr_corp
	UNION ALL
	SELECT Id, 'Email', contact_email FROM #jr_corp
	UNION ALL
	SELECT Id, 'Business Phone', contact_phone FROM #jr_corp
	UNION ALL
	SELECT Id, 'Fax', fax FROM #jr_corp
) a
ORDER BY 1, 2


SET IDENTITY_INSERT dbo.Customer OFF



SET NOCOUNT ON;
SET XACT_ABORT ON;
USE SmartFarm;
BEGIN TRAN;

IF COL_LENGTH('dbo.Income', 'Date amd time') IS NOT NULL
    EXEC sp_rename 'dbo.Income.[Date amd time]', 'Date and time', 'COLUMN';
IF COL_LENGTH('dbo.Income', 'Income Id') IS NULL
BEGIN
    DECLARE @pk sysname = (SELECT name FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('dbo.Income') AND type = 'PK');
    IF @pk IS NOT NULL EXEC('ALTER TABLE dbo.Income DROP CONSTRAINT [' + @pk + ']');
    ALTER TABLE dbo.Income ADD [Income Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Income PRIMARY KEY;
END


ALTER TABLE dbo.IssueInfo ALTER COLUMN Description NVARCHAR(200) NULL;
ALTER TABLE dbo.IssueInfo ALTER COLUMN status NVARCHAR(20) NULL;
UPDATE dbo.IssueInfo SET Description = RTRIM(Description), status = RTRIM(status);


ALTER TABLE dbo.OrderInfo ALTER COLUMN Confirmation NVARCHAR(20) NULL;
UPDATE dbo.OrderInfo SET Confirmation = RTRIM(Confirmation);
IF NOT EXISTS (SELECT 1 FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID('dbo.OrderInfo') AND type = 'PK')
BEGIN
    ALTER TABLE dbo.OrderInfo ALTER COLUMN [Order-Id] INT NOT NULL;
    ALTER TABLE dbo.OrderInfo ADD CONSTRAINT PK_OrderInfo PRIMARY KEY ([Order-Id]);
END


IF EXISTS (SELECT 1 FROM dbo.WorkerInfo WHERE [Worker Id] = 2001) AND NOT EXISTS (SELECT 1 FROM dbo.WorkerInfo WHERE [Worker Id] = 3)
    UPDATE dbo.WorkerInfo SET [Worker Id] = 3 WHERE [Worker Id] = 2001;
INSERT INTO dbo.WorkerInfo ([Worker Id], Username, Password, Address, Phone)
    SELECT l.ID, l.Username, l.password, l.Address, l.Phone FROM dbo.Login l
    WHERE l.Role = 'worker' AND NOT EXISTS (SELECT 1 FROM dbo.WorkerInfo w WHERE w.[Worker Id] = l.ID);
INSERT INTO dbo.BuyerInfo ([Buyer-ID], Username, Password, Address, Phone)
    SELECT l.ID, l.Username, l.password, l.Address, l.Phone FROM dbo.Login l
    WHERE l.Role = 'buyer' AND NOT EXISTS (SELECT 1 FROM dbo.BuyerInfo b WHERE b.[Buyer-ID] = l.ID);
INSERT INTO dbo.ManagerInfo ([Manager Id], Username, Password, Address, Phone)
    SELECT l.ID, l.Username, l.password, l.Address, l.Phone FROM dbo.Login l
    WHERE l.Role = 'manager' AND NOT EXISTS (SELECT 1 FROM dbo.ManagerInfo m WHERE m.[Manager Id] = l.ID);

COMMIT;

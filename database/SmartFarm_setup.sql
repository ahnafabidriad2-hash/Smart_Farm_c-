IF DB_ID('SmartFarm') IS NOT NULL
BEGIN
    PRINT 'SmartFarm database already exists. Nothing was changed. Use fix_existing_database.sql for an older copy.';
    SET NOEXEC ON;
END
GO
CREATE DATABASE SmartFarm;
GO
USE SmartFarm;
GO

-- login: sob role (admin, manager, worker, buyer) ekhane, ID diye login hoy
CREATE TABLE Login (
    ID INT NOT NULL PRIMARY KEY,
    Role NVARCHAR(20) NOT NULL,
    Username NVARCHAR(50) NOT NULL,
    password NVARCHAR(50) NOT NULL,
    Address NVARCHAR(100) NULL,
    Phone INT NULL
);
CREATE TABLE AdminInfo   (AdminId INT NOT NULL PRIMARY KEY, Username NVARCHAR(50), Password NVARCHAR(50), Address NVARCHAR(100), Phone INT);
CREATE TABLE ManagerInfo ([Manager Id] INT NOT NULL PRIMARY KEY, Username NVARCHAR(50), Password NVARCHAR(50), Address NVARCHAR(100), Phone INT);
CREATE TABLE WorkerInfo  ([Worker Id] INT NOT NULL PRIMARY KEY, Username NVARCHAR(50), Password NVARCHAR(50), Address NVARCHAR(100), Phone INT);
CREATE TABLE BuyerInfo   ([Buyer-ID] INT NOT NULL PRIMARY KEY, Username NVARCHAR(50), Password NVARCHAR(50), Address NVARCHAR(100), Phone INT);

CREATE TABLE ProductInfo (
    [Product ID] NVARCHAR(20) NOT NULL PRIMARY KEY,
    [Product Name] NVARCHAR(100),
    Details NVARCHAR(200),
    Quantity INT,
    [Price/unit] FLOAT
);
CREATE TABLE OrderInfo (
    [Order-Id] INT NOT NULL PRIMARY KEY,
    [Product-Id] INT,
    [Product Name] NVARCHAR(100),
    Quantity NVARCHAR(20),
    Price FLOAT,
    [Total price] FLOAT,
    [Buyer-Id] INT,
    Confirmation NVARCHAR(20)
);
CREATE TABLE Income (
    [Income Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Product Id] NVARCHAR(20),
    [Product Name] NVARCHAR(100),
    [Unit Price] FLOAT,
    Quantity INT,
    [Total Sell] FLOAT,
    [Date and time] DATETIME
);
CREATE TABLE Expense (
    [Expense Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
    [Expense Catagory] NVARCHAR(100),
    Details NVARCHAR(200),
    [Date and Time] DATETIME,
    Amount NVARCHAR(50)
);
CREATE TABLE SupplyInfo (
    [Supply-Id] INT NOT NULL PRIMARY KEY,
    [Supply name] NVARCHAR(100),
    Price DECIMAL(18, 2),
    Quantity INT,
    [Manager-Id] INT NULL,
    [Date and Time] DATETIME
);
CREATE TABLE InventoryInfo (
    [Supply-Id] INT NOT NULL PRIMARY KEY,
    [Supply Name] NVARCHAR(100),
    [Remaining Amount] FLOAT,
    [Used Amount] FLOAT,
    [Date & time] DATETIME,
    [Manage-Id] INT
);
CREATE TABLE TaskInfo (
    [Task-Id] INT NOT NULL PRIMARY KEY,
    Description NVARCHAR(200),
    Status NVARCHAR(20),
    [Worker-Id] INT
);
CREATE TABLE IssueInfo (
    [Issue-Id] INT NOT NULL PRIMARY KEY,
    Description NVARCHAR(200),
    status NVARCHAR(20),
    [Task-Id] INT
);
CREATE TABLE ManagerTask  ([Task-Id] INT NOT NULL PRIMARY KEY, Status NVARCHAR(20), Description NVARCHAR(200), [Manager-Id] INT);
CREATE TABLE ManagerIssue ([Issue-Id] INT NOT NULL PRIMARY KEY, Status NVARCHAR(20), Description NVARCHAR(200), [Manager-Id] INT);
CREATE TABLE TaskStatus   (Pending NCHAR(10), Working NCHAR(10), Completed NCHAR(10));
CREATE TABLE IssueStatus  (Answered NCHAR(10), Pending NCHAR(10), Resolved NCHAR(10));
GO

-- sample data (sob banano, asol manush na)
INSERT INTO Login VALUES
 (1, 'Admin', 'Admin', '1234', 'Dhaka', 1700000001),
 (2, 'manager', 'Manager', '12345', 'Dhaka', 1700000002),
 (3, 'worker', 'Worker One', '123456', 'Cumilla', 1700000003),
 (4, 'worker', 'Worker Two', '123456', 'Cumilla', 1700000004),
 (997, 'buyer', 'Buyer One', '997', 'Dhaka', 1700000997),
 (998, 'buyer', 'Buyer Two', '998', 'Rajshahi', 1700000998);
INSERT INTO AdminInfo   SELECT ID, Username, password, Address, Phone FROM Login WHERE Role = 'Admin';
INSERT INTO ManagerInfo SELECT ID, Username, password, Address, Phone FROM Login WHERE Role = 'manager';
INSERT INTO WorkerInfo  SELECT ID, Username, password, Address, Phone FROM Login WHERE Role = 'worker';
INSERT INTO BuyerInfo   SELECT ID, Username, password, Address, Phone FROM Login WHERE Role = 'buyer';

INSERT INTO ProductInfo VALUES
 ('111', 'Rui Fish', 'Avg. wt. 2kg', 140, 200),
 ('112', 'Desi Chicken', '67 days old, avg. wt. 700gm', 2000, 370),
 ('113', 'Sonali Chicken', '58 days old', 120, 240),
 ('114', 'Eggs', 'Collected today', 10000, 10.25);
INSERT INTO OrderInfo VALUES
 (1, 112, 'Desi Chicken', '20', 370, 7400, 997, 'Confirmed'),
 (2, 111, 'Rui Fish', '5', 200, 1000, 998, 'Pending');
INSERT INTO SupplyInfo VALUES
 (101, 'Chicken Feed (50kg bag)', 2500, 10, 2, GETDATE()),
 (102, 'Fish Feed (25kg bag)', 1800, 6, 2, GETDATE());
INSERT INTO Expense VALUES
 ('ORD-101', 'Chicken Feed (50kg bag)', 'Order for Chicken Feed', GETDATE(), '25000'),
 ('ORD-102', 'Fish Feed (25kg bag)', 'Order for Fish Feed', GETDATE(), '10800');
INSERT INTO InventoryInfo VALUES
 (101, 'Chicken Feed (kg)', 350, 150, GETDATE(), 2),
 (102, 'Fish Feed (kg)', 120, 30, GETDATE(), 2);
INSERT INTO TaskInfo VALUES
 (1, 'Feed the chickens (morning)', 'Pending', 3),
 (2, 'Clean the fish pond filter', 'Working', 3),
 (3, 'Collect eggs from shed 2', 'Completed', 4);
INSERT INTO IssueInfo VALUES
 (1, 'Pond filter pump is making noise', 'Pending', 2);
GO
SET NOEXEC OFF;
GO

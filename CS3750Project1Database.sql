/* 
Create Database 
*/
USE master
GO

IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name='Project1Todo')
	DROP DATABASE Project1Todo;

CREATE DATABASE [Project1Todo]
	ON PRIMARY
( NAME = N'Project1Todo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Project1Todo.mdf',
	SIZE = 5120KB, FILEGROWTH = 1024KB )
	LOG ON
( NAME = N'Project1Todo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Project1Todo_log.ldf',
	SIZE = 2048KB, FILEGROWTH = 10% )

/* 
Create User 
*/
CREATE LOGIN CMSUser WITH PASSWORD = 'Iwanttoconnect'; 
GO

USE Project1Todo
CREATE USER CMSUser FOR LOGIN CMSUser
EXEC sp_addrolemember 'db_datareader', CMSUser;
EXEC sp_addrolemember 'db_datawriter', CMSUser;

/* 
Create Tables 
*/

USE Project1Todo;
GO

CREATE TABLE dbo.TodoList (
	list_id int IDENTITY(1,1) NOT NULL,
	listName nvarchar(255) NOT NULL,
	lastChangedDate datetime NOT NULL
);

CREATE TABLE dbo.Item (
	item_id int IDENTITY(1,1) NOT NULL,
	list_id int NOT NULL,
	category_id int NOT NULL,
	taskName nvarchar(255) NOT NULL,
	isDone bit NOT NULL,
	lastChangedDate datetime NOT NULL
);

CREATE TABLE dbo.Category (
	category_id int IDENTITY(1,1) NOT NULL,
	categoryName nvarchar(100) NOT NULL,
	lastChangedDate datetime NOT NULL
);

ALTER TABLE dbo.TodoList
	ADD CONSTRAINT PK_TodoList PRIMARY KEY CLUSTERED (list_id);

ALTER TABLE dbo.Category
	ADD CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (category_id);
ALTER TABLE dbo.Category
	ADD CONSTRAINT AK_Category_categoryName UNIQUE(categoryName);

ALTER TABLE dbo.Item
	ADD CONSTRAINT PK_Item PRIMARY KEY CLUSTERED (item_id);
ALTER TABLE dbo.Item
	ADD CONSTRAINT FK_Item_TodoList_id FOREIGN KEY (list_id) REFERENCES dbo.TodoList(list_id);
ALTER TABLE dbo.Item
	ADD CONSTRAINT FK_Item_Category_id FOREIGN KEY (category_id) REFERENCES dbo.Category(category_id);


/* 
Insert Default Categories 
*/

INSERT INTO dbo.Category (categoryName, lastChangedDate)
VALUES ('School', GETDATE());

INSERT INTO dbo.Category (categoryName, lastChangedDate)
VALUES ('Home', GETDATE());

INSERT INTO dbo.Category (categoryName, lastChangedDate)
VALUES ('Work', GETDATE());

INSERT INTO dbo.Category (categoryName, lastChangedDate)
VALUES ('Shopping', GETDATE());

INSERT INTO dbo.TodoList(listName, lastChangedDate)
VALUES ('SampleList 1', GETDATE());

INSERT INTO dbo.TodoList (listName, lastChangedDate)
VALUES ('SampleList 2', GETDATE());

INSERT INTO dbo.Item (list_id, category_id, taskName, isDone, lastChangedDate)
VALUES (1, 1, 'SampleTask 1', 0, GETDATE());

INSERT INTO dbo.Item (list_id, category_id, taskName, isDone, lastChangedDate)
VALUES (1, 2, 'SampleTask 2', 1, GETDATE());

INSERT INTO dbo.Item (list_id, category_id, taskName, isDone, lastChangedDate)
VALUES (2, 1, 'SampleTask 3', 0, GETDATE());
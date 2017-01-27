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


----------------
-- Create the User table
----------------
CREATE TABLE [dbo].[User]
(
	[id] BIGINT NOT NULL IDENTITY(1,1), 
	[first_name] NVARCHAR(50) NOT NULL,
	[last_name] NVARCHAR(50) NOT NULL,
	[username] NVARCHAR(50) NOT NULL,
	[email] NVARCHAR(40) NULL,
	[password_hash] VARCHAR(295) NULL,
	CONSTRAINT PK_User PRIMARY KEY (id),
	CONSTRAINT UQ_User_Username UNIQUE (username)
)

----------------
-- Create the Role table
----------------
CREATE TABLE [dbo].[Role]
(
	[id] BIGINT NOT NULL IDENTITY(1,1), 
	[name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_Role PRIMARY KEY (id),
	CONSTRAINT UQ_User_Name UNIQUE ([name])
)

INSERT INTO [dbo].[Role] (name)
VALUES
	(N'Employee'),
	(N'Manager')

----------------
-- Create the UserRole table
----------------
CREATE TABLE [dbo].[UserRole]
(
	[id] BIGINT NOT NULL IDENTITY(1,1),
	[user_id] BIGINT NOT NULL,
	[role_id] BIGINT NOT NULL,
	CONSTRAINT PK_UserRole PRIMARY KEY (id),
	CONSTRAINT FK_UserRole_User_Id FOREIGN KEY ([user_id]) REFERENCES [User](id),
	CONSTRAINT FK_UserRole_Role_Id FOREIGN KEY (role_id) REFERENCES [Role](id),
	CONSTRAINT UQ_UserRole_User_Role UNIQUE ([user_id], role_id)
)
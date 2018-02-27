-- Script Date: 2/27/2018 9:52 AM  - ErikEJ.SqlCeScripting version 3.5.2.75
SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [SimpleUser] (
  [IDUser] INTEGER  NOT NULL
, [Name] nvarchar(150)  NOT NULL
, [Email] nvarchar(150)  NOT NULL
, [Password] nvarchar(150)  NOT NULL
, [ConfirmedByEmail] bit NOT NULL
, [IsAdmin] bit NOT NULL
, CONSTRAINT [PK_SimpleUser] PRIMARY KEY ([IDUser])
);
CREATE TABLE [Book] (
  [IDBook] INTEGER  NOT NULL
, [IDTinRead] nvarchar(50)  NOT NULL
, [Title] nvarchar(500)  NULL
, [Creator] nvarchar(500)  NULL
, [Identifier] nvarchar(150)  NULL
, [IsCorrect] bit NOT NULL
, [ErrorMessage] nvarchar(500)  NULL
, CONSTRAINT [PK_Book] PRIMARY KEY ([IDBook])
);
COMMIT;


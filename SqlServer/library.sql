CREATE DATABASE Library
GO

Use Library
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Books] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NULL,
    [Author] nvarchar(200) NULL,
    [Genre] nvarchar(max) NULL,
    [NumberOfPages] int NOT NULL,
    [InInventory] bit NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id])
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([Id], [Author], [Genre], [InInventory], [NumberOfPages], [Title])
VALUES (1, N'Threau', N'Philosophy', CAST(1 AS bit), 322, N'Walden');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([Id], [Author], [Genre], [InInventory], [NumberOfPages], [Title])
VALUES (2, N'DJ Spooky That Subliminal Kid', N'Music', CAST(1 AS bit), 180, N'Rythm Science');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([Id], [Author], [Genre], [InInventory], [NumberOfPages], [Title])
VALUES (3, N'Emerson', N'Philosophy', CAST(1 AS bit), 182, N'Nature');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Author', N'Genre', N'InInventory', N'NumberOfPages', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200122162603_Initial', N'3.1.1');

GO

CREATE TABLE [Reservations] (
    [Id] int NOT NULL IDENTITY,
    [For] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [ReservationCreated] datetime2 NOT NULL,
    [Books] nvarchar(max) NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200128155600_ReservationsAdded', N'3.1.1');

GO


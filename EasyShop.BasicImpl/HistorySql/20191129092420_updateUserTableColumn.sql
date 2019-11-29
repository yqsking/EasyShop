IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [t_Table_Root] (
    [Id] nvarchar(32) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [RootName] nvarchar(50) NOT NULL,
    [RootCode] nvarchar(50) NOT NULL,
    [Remark] nvarchar(200) NULL,
    CONSTRAINT [PK_t_Table_Root] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [t_User_Role] (
    [Id] nvarchar(32) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [RoleName] nvarchar(50) NOT NULL,
    [RoleCode] nvarchar(50) NOT NULL,
    [Remark] nvarchar(200) NULL,
    CONSTRAINT [PK_t_User_Role] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [t_User_RoleRoot] (
    [Id] nvarchar(32) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UserRoleId] nvarchar(32) NOT NULL,
    [RootId] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_t_User_RoleRoot] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [t_User_User] (
    [Id] nvarchar(32) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [Password] nvarchar(10) NOT NULL,
    [Photo] nvarchar(200) NULL,
    [QQNumber] nvarchar(50) NULL,
    [WeCharNumber] nvarchar(50) NULL,
    [Email] nvarchar(50) NULL,
    CONSTRAINT [PK_t_User_User] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [t_User_UserRole] (
    [Id] nvarchar(32) NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [UserId] nvarchar(32) NOT NULL,
    [RoleId] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_t_User_UserRole] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191126075021_createDatabase', N'3.0.1');

GO

ALTER TABLE [t_User_User] ADD [UserState] nvarchar(50) NOT NULL DEFAULT N'';

GO

ALTER TABLE [t_User_User] ADD [UserType] nvarchar(50) NOT NULL DEFAULT N'';

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191129092420_updateUserTableColumn', N'3.0.1');

GO


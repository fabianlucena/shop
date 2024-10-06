IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE [name] = 'auth')
	EXEC('CREATE SCHEMA auth AUTHORIZATION [dbo]');
GO

IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Users') AND type in (N'U'))
	CREATE TABLE auth.Users (
		Id bigint IDENTITY(1,1) NOT NULL,
		Uuid uniqueidentifier NOT NULL,
		CreatedAt datetime NOT NULL,
		UpdatedAt datetime NOT NULL,
		DeletedAt datetime NULL,
		IsEnabled bit DEFAULT 1 NOT NULL,
		Username nvarchar(255) NOT NULL,
		DisplayName nvarchar(255) NOT NULL,
		CONSTRAINT auth_Users_PK PRIMARY KEY (Id),
		CONSTRAINT auth_Users_UK_Uuid UNIQUE (Uuid),
		CONSTRAINT auth_Users_UK_Username UNIQUE (Username),
		CONSTRAINT auth_Users_UK_DisplayName UNIQUE (DisplayName)
	);
GO

IF NOT EXISTS (SELECT TOP 1 1 FROM auth.users WHERE Username = 'admin')
	INSERT INTO auth.users(Uuid, IsEnabled, Username, DisplayName, CreatedAt, UpdatedAt, DeletedAt)
	VALUES(NEWID(), 1, N'admin', N'Administrador', GETDATE(), GETDATE(), NULL);
GO

IF NOT EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Passwords') AND type in (N'U'))
	CREATE TABLE auth.Passwords (
		CreatedAt datetime NOT NULL,
		UpdatedAt datetime NOT NULL,
		DeletedAt datetime NULL,
		UserId bigint NOT NULL,
		[Hash] nvarchar(255) NOT NULL,
		CONSTRAINT auth_Passwords_PK PRIMARY KEY (UserId)
	);
GO

DECLARE @ForeignKeys Table(
	PKTABLE_QUALIFIER SYSNAME,
	PKTABLE_OWNER SYSNAME,
	PKTABLE_NAME SYSNAME,
	PKCOLUMN_NAME SYSNAME,
	FKTABLE_QUALIFIER SYSNAME,
	FKTABLE_OWNER SYSNAME,
	FKTABLE_NAME SYSNAME,
	FKCOLUMN_NAME SYSNAME,
	KEY_SEQ SMALLINT,
	UPDATE_RULE SMALLINT,
	DELETE_RULE SMALLINT,
	FK_NAME SYSNAME,
	PK_NAME SYSNAME,
	DEFERRABILITY SMALLINT
);

INSERT INTO @ForeignKeys EXEC sp_fkeys @fktable_name = 'Passwords', @fktable_owner = 'auth';

IF NOT EXISTS(SELECT TOP 1 1 FROM @ForeignKeys WHERE FKCOLUMN_NAME = N'UserId' AND PKTABLE_NAME = N'Users' AND PKTABLE_OWNER = N'auth' AND PKCOLUMN_NAME = N'Id')
	ALTER TABLE auth.Passwords WITH CHECK ADD CONSTRAINT Passwords_Users_FK FOREIGN KEY(UserId) REFERENCES auth.Users (id);
GO


DECLARE @adminUserId BIGINT = (SELECT Id FROM auth.Users WHERE Username = N'admin');
IF NOT EXISTS(SELECT TOP 1 1 FROM auth.Passwords WHERE UserId = @adminUserId)
BEGIN
	INSERT INTO auth.Passwords(CreatedAt, UpdatedAt, UserId, [Hash])
	VALUES (GETDATE(), GETDATE(), @adminUserId, N'$2a$11$fRe./FCGyNjS9Vao3IIBlOiVCx3C05NRBNFrHhVk32Qdw75Ia.Y5S');
END
GO

IF OBJECT_ID(N'auth.Devices', N'U') IS NULL
BEGIN
	CREATE TABLE auth.Devices(
		Id BIGINT IDENTITY(1,1) NOT NULL,
		Uuid UNIQUEIDENTIFIER NOT NULL,
		CreatedAt DATETIME NOT NULL,
		UpdatedAt DATETIME NOT NULL,
		DeletedAt DATETIME NULL,
		Token NVARCHAR(255) NOT NULL,
		CONSTRAINT auth_Devices_PK PRIMARY KEY NONCLUSTERED (Id),
	) ON [PRIMARY];
END
GO

IF OBJECT_ID(N'auth.Sessions', N'U') IS NULL
BEGIN
	CREATE TABLE auth.[Sessions](
		Id BIGINT IDENTITY(1,1) NOT NULL,
		Uuid UNIQUEIDENTIFIER NOT NULL,
		CreatedAt DATETIME NOT NULL,
		UpdatedAt DATETIME NOT NULL,
		DeletedAt DATETIME NULL,
		UserId BIGINT NOT NULL,
		DeviceId BIGINT NOT NULL,
		Token NVARCHAR(255) NOT NULL,
		AutoLoginToken NVARCHAR(255) NOT NULL,

		CONSTRAINT auth_Sessions_PK PRIMARY KEY NONCLUSTERED (Id),
		CONSTRAINT auth_Sessions_FK_UserId_auth_Users FOREIGN KEY (UserId)
			REFERENCES auth.Users(Id),
		CONSTRAINT auth_Sessions_FK_DeviceId_auth_Devices FOREIGN KEY (DeviceId)
			REFERENCES auth.Devices(Id)
	) ON [PRIMARY];
END
GO
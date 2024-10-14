IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Sessions') AND type in (N'U'))
	DROP TABLE auth.[Sessions]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Devices') AND type in (N'U'))
	DROP TABLE auth.Devices
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Passwords') AND type in (N'U'))
	DROP TABLE auth.Passwords
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.UsersEmails') AND type in (N'U'))
	DROP TABLE auth.UsersEmails
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.Users') AND type in (N'U'))
	DROP TABLE auth.Users
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'auth.UsersTypes') AND type in (N'U'))
	DROP TABLE auth.UsersTypes
GO


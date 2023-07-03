# EntityFrameworkCore.ChangeTrackingTriggers

EntityFrameworkCore.ChangeTrackingTriggers is an EF Core add-on for storing and querying changes made to SQL tables using triggers. A separate table will be created for each tracked entity to store the changes with full EF Core support for querying the changes. The changes table and trigger will be automatically added and updated via migrations when the source table schema changes.

## Why use triggers?

The main advantage of using triggers is that any ad-hoc updates to the databases (not using EF Core) are included in change tracking. This can be important if your database often requires manual intervention outside of EF Core and you don't want to miss these changes in your change tracking.

## Getting started

**NOTE: As your tracked entity and change entity will require most of the same properties, it is recommended to create a base class that both will extend. See the samples for an implementation of this.**
1. Add the below nuget package to your project
```
EntityFrameworkCore.ChangeTrackingTriggers.SqlServer
```
2. Implement the `ITracked` interface on any entity classes that you want to track
```C#
public class User : ITracked<UserChange>
{
...
```
3. Create a change entity for the tracked entity that implements the `IChange` interface
```C#
public class UserChange : IChange<User, int>
{
...
```
4. Add the below assembly attribute to your `Program.cs`
```C#
[assembly: DesignTimeServicesReference("EntityFrameworkCore.ChangeTrackingTriggers.ChangeTrackingDesignTimeServices, EntityFrameworkCore.ChangeTrackingTriggers")]
```
5. Add ChangeTrackingTriggers to your `DbContext`
```C#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTrackingTriggers();
});
```
6. Auto-configure your change tracking trigger entities in your `DbContext`
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AutoConfigureChangeTrackingTriggers();
...
```

## Configuration

### Storing who changes are made by

### Storing the source of changes

### Customising the trigger

If you need to customise a trigger, for example to change the trigger name, use the `ConfigureChangeTrackingTrigger()` extension method on your `ModelBuilder`.

```C#
e.ConfigureChangeTrackingTrigger(options =>
{
    options.TriggerNameFactory = tableName => $"CustomTriggerName_{tableName}";
});
```

## Sample generated table and trigger
```SQL
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [DateOfBirth] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
```
```SQL
CREATE TABLE [UserChanges] (
    [ChangeId] int NOT NULL IDENTITY,
    [OperationTypeId] int NOT NULL,
    [ChangeSource] int NOT NULL,
    [ChangedAt] datetimeoffset NOT NULL,
    [ChangedById] int NOT NULL,
    [Id] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [DateOfBirth] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserChanges] PRIMARY KEY ([ChangeId]),
    CONSTRAINT [FK_UserChanges_Users_ChangedById] FOREIGN KEY ([ChangedById]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_UserChanges_Users_Id] FOREIGN KEY ([Id]) REFERENCES [Users] ([Id])
);
```
```SQL
/*
Auto-generated trigger by EntityFrameworkCore.ChangeTrackingTriggers
https://github.com/codemunkie15/EntityFrameworkCore.ChangeTrackingTriggers
*/

CREATE TRIGGER [dbo].[Users_ChangeTracking]
ON [dbo].[Users]
FOR INSERT, UPDATE, DELETE
NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @OperationTypeId INT
	DECLARE @ChangeSource INT
	DECLARE @ChangedBy INT

	SET @OperationTypeId =
		(CASE
			WHEN EXISTS(SELECT * FROM INSERTED) AND EXISTS(SELECT * FROM DELETED)
				THEN 2  -- UPDATE
			WHEN EXISTS(SELECT * FROM INSERTED)
				THEN 1  -- INSERT
			WHEN EXISTS(SELECT * FROM DELETED)
				THEN 3  -- DELETE
		END)

	SET @ChangeSource = CAST(SESSION_CONTEXT(N'ChangeTrackingContext.ChangeSource') AS INT)
	IF @ChangeSource IS NULL
	BEGIN
		;THROW 130101, 'ChangeTrackingContext.ChangeSource must be set in session context for change tracking. Transaction was not commited.', 1
	END

	SET @ChangedBy = CAST(SESSION_CONTEXT(N'ChangeTrackingContext.ChangedBy') AS INT)
	IF @ChangedBy IS NULL
	BEGIN
		;THROW 130101, 'ChangeTrackingContext.ChangedBy must be set in session context for change tracking. Transaction was not commited.', 1
	END

	IF @OperationTypeId = 1
	BEGIN
		INSERT INTO dbo.[UserChanges] ([OperationTypeId],[ChangeSource],[ChangedAt],[ChangedById],[DateOfBirth],[Id],[Name])
		SELECT @OperationTypeId,@ChangeSource,GETUTCDATE(),@ChangedBy,[i].[DateOfBirth],[i].[Id],[i].[Name]
		FROM inserted [i]
	END

	IF @OperationTypeId = 2
	BEGIN
		INSERT INTO dbo.[UserChanges] ([OperationTypeId],[ChangeSource],[ChangedAt],[ChangedById],[DateOfBirth],[Id],[Name])
		SELECT @OperationTypeId,@ChangeSource,GETUTCDATE(),@ChangedBy,[i].[DateOfBirth],[i].[Id],[i].[Name]
		FROM inserted [i]
		JOIN deleted [d] ON [d].[Id] = [i].[Id]
		WHERE EXISTS (SELECT [i].* EXCEPT SELECT [d].*) -- Only select rows that have changed values
	END

	IF @OperationTypeId = 3
	BEGIN
		INSERT INTO dbo.[UserChanges] ([OperationTypeId],[ChangeSource],[ChangedAt],[ChangedById],[DateOfBirth],[Id],[Name])
		SELECT @OperationTypeId,@ChangeSource,GETUTCDATE(),@ChangedBy,[d].[DateOfBirth],[d].[Id],[d].[Name]
		FROM deleted [d]
	END

END;
GO
```
# EFCore.ChangeTriggers

[![Nuget](https://img.shields.io/nuget/v/EFCore.ChangeTriggers)](https://www.nuget.org/packages/EFCore.ChangeTriggers)

EFCore.ChangeTriggers is an Entity Framework Core extension for storing and querying changes made to SQL tables using triggers. A separate table will be created for each tracked entity to store the changes with full EF Core support for querying the changes. The changes table and trigger will be automatically added and updated via migrations when the source table schema changes.

**SQL Server is currently the only supported EF Core provider.**

## Features

* Auto-generates SQL triggers to track changes for entities in a separate table.
* Captures changes from EF Core (including migrations) and raw SQL queries executed on the database.
* Optional configuration to store who made the change (ChangedBy) and where the change originated from (ChangeSource). 
* Ability to query the changes using your DbContext to see previous values. See [EFCore.ChangeTriggers.ChangeEventQueries](https://github.com/codemunkie15/EFCore.ChangeTriggers/tree/main/src/EFCore.ChangeTriggers.ChangeEventQueries) if you need to project change entities into human-readable change events.

## Example populated change table

![Example](https://raw.githubusercontent.com/codemunkie15/EFCore.ChangeTriggers/main/docs/images/Example1.png)

## Getting started

**NOTE: As your tracked entity and change entity will require most of the same properties, it is recommended to create a base class that both will extend. See the [samples](https://github.com/codemunkie15/EFCore.ChangeTriggers/tree/main/samples) for an implementation of this.**
1. Add the below nuget package to your project
```
EFCore.ChangeTriggers.SqlServer
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
[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
```

5. Add ChangeTriggers to your `DbContext`
```C#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers();
});
```

6. Auto-configure your change trigger entities in your `DbContext`
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AutoConfigureChangeTriggers();
...
```

7. Create a migration to generate the required objects
```
dotnet ef migrations add ChangeTriggers
```

## Configuration

### Storing who changes are made by

A ChangedBy column can be added and populated on change tables to store who changes are made by.

1. Create a `ChangedByProvider` by inheriting the `ChangedByProvider<TChangedBy>` class and overriding the required method(s).
```C#
public class ChangedByProvider : ChangedByProvider<User>
{
	public override Task<User> GetChangedByAsync()
	{
		return Task.FromResult(new User { Id = 1});
	}
}
```

2. Use the `UseSqlServerChangeTriggers<TChangedByProvider, TChangedBy>()` overload, specifying your `ChangedByProvider` and ChangedBy type.
```C#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers<ChangedByProvider, User>();
});
```

### Storing the source of changes

A ChangeSource column can be added and populated on change tables to store where the change came from, i.e. if you need to distinguish between migrations, API updates etc.

1. Create a `ChangeSourceProvider` by inheriting `ChangeSourceProvider<TChangeSource>` class and overriding the required method(s).
```C#
public class ChangeSourceProvider : ChangeSourceProvider<ChangeSourceType>
{
	public override Task<ChangeSourceType> GetChangeSourceAsync()
	{
		return Task.FromResult(ChangeSourceType.WebApi);
	}
}
```

2. Use the `UseSqlServerChangeTriggers<TChangeSourceProvider, TChangeSource>()` overload, specifying your `ChangeSourceProvider` and ChangeSource type.
```C#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers<ChangeSourceProvider, ChangeSourceType>();
});
```

### Using migrations

If you are modifying data through migrations, you might want to have different values for ChangedBy and ChangeSource specifically for migration changes. You can override the below methods in your providers to achieve this.

```C#
public override User GetMigrationChangedBy()
{
}

public override ChangeSourceType GetMigrationChangeSource()
{
}
```

### Customising the trigger

Individual change tables can be configured using the `ConfigureChangeTrigger()` extension method on your `ModelBuilder`.

```C#
modelBuilder.Entity<Permission>(e =>
{
    e.ConfigureChangeTrigger(options =>
    {
        options.TriggerNameFactory = tableName => $"CustomTriggerName_{tableName}";
    });
});
```

## Sample generated tables and trigger
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
Auto-generated trigger by EFCore.ChangeTriggers
https://github.com/codemunkie15/EFCore.ChangeTriggers
*/

CREATE TRIGGER [dbo].[Users_Change]
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

	SET @ChangeSource = CAST(SESSION_CONTEXT(N'ChangeContext.ChangeSource') AS INT)
	IF @ChangeSource IS NULL
	BEGIN
		;THROW 130101, 'ChangeContext.ChangeSource must be set in session context for change tracking. Transaction was not commited.', 1
	END

	SET @ChangedBy = CAST(SESSION_CONTEXT(N'ChangeContext.ChangedBy') AS INT)
	IF @ChangedBy IS NULL
	BEGIN
		;THROW 130101, 'ChangeContext.ChangedBy must be set in session context for change tracking. Transaction was not commited.', 1
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

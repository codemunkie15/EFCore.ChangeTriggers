# EntityFrameworkCore.ChangeTrackingTriggers
EntityFrameworkCore.ChangeTrackingTriggers is an EF Core add-on for storing and querying changes made to SQL tables using triggers. A separate table will be created for each tracked entity to store the changes with full EF Core support for querying the changes. The changes table and trigger will be automatically added and updated via migrations when the source table schema changes.

## Why use triggers?
The main advantage of using triggers is that any ad-hoc updates to the databases (not using EF Core) are included in change tracking. This can be important if your database often requires manual intervention outside of EF Core and you don't want to miss these changes in your change tracking.

## Getting started
**NOTE: As your tracked entity and change entity will require most of the same properties, it is recommended to create a base class that both will extend. See the samples for an implemtation of this.**
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
4. Add the below assembly attribute to your Program.cs
```C#
[assembly: DesignTimeServicesReference("EntityFrameworkCore.ChangeTrackingTriggers.ChangeTrackingDesignTimeServices, EntityFrameworkCore.ChangeTrackingTriggers")]
```
5. Add ChangeTrackingTriggers to your DbContext
```C#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTrackingTriggers();
});
```
6. Auto-configure your change tracking trigger entities in your DbContext
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AutoConfigureChangeTrackingTriggers();
...
```

## Configuration
### Storing who changes are made by

### Storing the source of changes

### Modifying the trigger name

## Sample generated table and trigger

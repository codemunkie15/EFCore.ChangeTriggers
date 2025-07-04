---
title: Getting started
layout: home
nav_order: 1
---

{: .note }
As your tracked entity and change entity will require most of the same properties, it is recommended to create a base class that both will extend. See the [samples](https://github.com/codemunkie15/EFCore.ChangeTriggers/tree/main/samples) for an implementation of this.

1. Add the below nuget package to your project:
```
EFCore.ChangeTriggers.SqlServer
```

2. Implement the `ITracked<>` interface on any entity classes that you want to track:
```c#
public class User : ITracked<UserChange>
{
...
```

3. Create a change entity for the tracked entity that implements the `IChange<>` interface:
```c#
public class UserChange : IChange<User>
{
...
```

4. Add the below assembly attribute to your `Program.cs`:
```c#
[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]
```

5. Enable ChangeTriggers on your `DbContext`:
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServer()
        .UseSqlServerChangeTriggers();
});
```

6. Auto-configure your change trigger entities in the `OnModelCreating()` method of your `DbContext`:
```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.AutoConfigureChangeTriggers();
...
```

7. Create a migration to generate the required objects:
```powershell
dotnet ef migrations add ChangeTriggers
```
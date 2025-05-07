---
title: Tracking what made changes
layout: home
parent: Configuration
nav_order: 1
---

A ChangeSource property can be added and populated on your change entities, to store where change occured from.

1. Implement the `IHasChangeSource<TChangeSource>` interface on your change entity (ChangeSource is an enum in this example):
```c#
public class UserChange : IChange<User>, IHasChangeSource<ChangeSource>
{
...
```

1. Create a `ChangeSourceProvider` by inheriting the `ChangeSourceProvider<TChangeSource>` class and overriding the required method(s):
```c#
public class MyChangeSourceProvider : ChangeSourceProvider<ChangeSource>
{
    public override <ChangeSourceType> GetChangeSourceAsync()
    {
        return Task.FromResult(ChangeSource.ConsoleApp);
    }
}
```

1. Add the `UseChangeSource<TChangeSourceProvider, TChangeSource>()` option to your DbContextOptions, specifying your `ChangeSourceProvider` and ChangeSource type:
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers(options =>
        {
            options.UseChangeSource<MyChangeSourceProvider, ChangeSource>()
        });
}
```

1. Create a migration if required:
```terminal
dotnet ef migrations add ChangeSource
```
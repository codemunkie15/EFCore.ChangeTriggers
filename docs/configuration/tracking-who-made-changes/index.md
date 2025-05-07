---
title: Tracking who made changes
layout: home
parent: Configuration
nav_order: 0
---

A ChangedBy property can be added and populated on your change entities, to store who changes are made by.

1. Implement the `IHasChangedBy<TChangedBy>` interface on your change entity:
```c#
public class UserChange : IChange<User>, IHasChangedBy<User>
{
...
```

1. Create a `ChangedByProvider` by inheriting the `ChangedByProvider<TChangedBy>` class and overriding the required method(s):
```c#
public class MyChangedByProvider : ChangedByProvider<User>
{
    public override Task<User> GetChangedByAsync()
    {
        return Task.FromResult(new User { Id = 1});
    }
}
```

1. Add the `UseChangedBy<TChangedByProvider, TChangedBy>()` option to your DbContextOptions, specifying your `ChangedByProvider` and ChangedBy type:
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers(options =>
        {
            options.UseChangedBy<MyChangedByProvider, User>()
        });
}
```

1. Create a migration if required:
```terminal
dotnet ef migrations add ChangedBy
```
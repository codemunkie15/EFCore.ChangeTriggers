---
title: Getting started
layout: home
parent: Change Event Queries
nav_order: 1
---

1. Add the below nuget package to your project:
```
EFCore.ChangeTriggers.ChangeEventQueries
```

2. Enable ChangeEventQueries on your `ChangeTriggersDbContextOptionsBuilder`:
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServer()
        .UseSqlServerChangeTriggers(options =>
        {
            options
                .UseChangeEventQueries();
        });
});
```
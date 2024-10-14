---
title: Trigger name format
layout: home
parent: Configuration
nav_order: 2
---

The trigger name format can be customised in two ways, either globally for every trigger, or for a specific entity.

### Global trigger name format
1. Use the `UseTriggerNameFactory()` option on your DbContextOptions:
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServerChangeTriggers(options =>
        {
            options.UseTriggerNameFactory(tableName => $"{tableName}_CustomTriggerName")
        });
}
```

### For a specific entity
1. In your model builder configuration, use the `ConfigureChangeTrigger()` extension method to set a trigger name format for a specific entity:
```c#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>(u =>
    {
        u.ConfigureChangeTrigger(options =>
            {
                options.TriggerNameFactory = tableName => $"{tableName}_CustomUserChangesTrigger";
            });
    });
}
```
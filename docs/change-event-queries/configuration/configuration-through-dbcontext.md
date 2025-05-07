---
title: Configuration through DbContext
layout: home
parent: Configuration
grand_parent: Change Event Queries
nav_order: 2
---

Change event queries can be configured through the DbContext, similar to how the EF Core `IEntityTypeConfiguration` pattern works.

1. Create a configuration class for your change entity that implements the `IChangeEventEntityConfiguration<>` interface.
```c#
public class UserChangeConfiguration : IChangeEventEntityConfiguration<UserChange>
{
    public void Configure(ChangeEventEntityConfigurationBuilder<UserChange> builder)
    {
        builder.AddProperty(uc => uc.Name);
        builder.AddProperty(uc => uc.DateOfBirth);
        builder.AddProperty(uc => uc.PrimaryPaymentMethod!.Id.ToString())
            .WithDescription("Primary payment method changed");
    }
}
```

2. Modify your UseChangeEventQueries() call to include the assembly where your configurations are located.
```c#
services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServer()
        .UseSqlServerChangeTriggers(options =>
        {
            options
                .UseChangeEventQueries(typeof(MyDbContext).Assembly);
        });
});
```
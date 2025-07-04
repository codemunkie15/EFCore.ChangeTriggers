---
title: Configuration per query
layout: home
parent: Configuration
grand_parent: Change Event Queries
nav_order: 2
---

Change event queries can be configured individually by passing a `ChangeEventConfiguration` to the `ToChangeEvents()` extension method. This will overwrite any global configuration for this query.
```c#
var query = await dbContext.UserChanges
    .Where(uc => uc.Id == 1)
    .ToChangeEvents<User, ChangeSourceType>(
        new ChangeEventConfiguration(builder =>
        {
            builder.Configure<UserChange>(uc =>
            {
                uc.AddInserts();
                uc.AddProperty(uc => uc.Name);
                uc.AddProperty(uc => uc.PrimaryPaymentMethod.Name)
                    .WithDescription("Payment method changed");
            });
        }))
    .OrderBy(ce => ce.ChangedAt).ToListAsync();
```
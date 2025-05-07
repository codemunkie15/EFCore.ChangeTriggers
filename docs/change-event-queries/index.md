---
title: Change Event Queries
layout: home
nav_order: 3
---

EFCore.ChangeTriggers.ChangeEventQueries is an EFCore.ChangeTriggers add-on for querying change entities in a human-readable format (usually to display in a grid). It projects change entities into events to see which properties actually changed values.

### Change table data (UserChanges)

![Example](https://raw.githubusercontent.com/codemunkie15/EFCore.ChangeTriggers/main/docs/images/Example1.png)

### Query

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

### Results

| Description                    | OldValue  | NewValue          | ChangedAt                  | ChangedBy | ChangeSource |
| ------------------------------ | --------- | ----------------  | -------------------------- | --------  | ------------ |
| Name changed                   | Billy Bob | Billy James Bob   | 14/08/2023 23:18:36 +00:00 | [object]  | ConsoleApp   |
| Payment method changed         |           | My payment method | 14/08/2023 23:18:36 +00:00 | [object]  | ConsoleApp   |
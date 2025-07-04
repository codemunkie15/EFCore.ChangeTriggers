# EFCore.ChangeTriggers.ChangeEventQueries

[![Nuget](https://img.shields.io/nuget/v/EFCore.ChangeTriggers.ChangeEventQueries)](https://www.nuget.org/packages/EFCore.ChangeTriggers.ChangeEventQueries)

EFCore.ChangeTriggers.ChangeEventQueries is an EFCore.ChangeTriggers add-on for querying change entities in a human-readable format (usually to display in a grid). It projects change entities into events to see which properties actually changed values.

## Example

### Change table data (UserChanges)

![Example](https://raw.githubusercontent.com/codemunkie15/EFCore.ChangeTriggers/main/docs/images/Example1.png)

### Query

```C#
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
| Primary payment method changed |           | My payment method | 14/08/2023 23:18:36 +00:00 | [object]  | ConsoleApp   |

### Generated SQL query (may change depending on EF Core version)
```SQL
SELECT @__description_0 AS [Description], [t].[DateOfBirth] AS [OldValue], [u].[DateOfBirth] AS [NewValue], [u].[ChangedAt], [u1].[Id], [u1].[DateOfBirth], [u1].[Name], [u1].[PrimaryPaymentMethodId], [u].[ChangeSource]
FROM [UserChanges] AS [u]
CROSS APPLY (
	SELECT TOP(1) [u0].[DateOfBirth]
	FROM [UserChanges] AS [u0]
	WHERE [u0].[Id] = 2 AND [u0].[Id] = [u].[Id] AND [u0].[ChangedAt] < [u].[ChangedAt]
	ORDER BY [u0].[ChangedAt] DESC
) AS [t]
LEFT JOIN [Users] AS [u1] ON [u].[ChangedById] = [u1].[Id]
WHERE [u].[Id] = 2 AND [t].[DateOfBirth] <> [u].[DateOfBirth]
UNION ALL
SELECT @__description_1 AS [Description], [t1].[Name] AS [OldValue], [u2].[Name] AS [NewValue], [u2].[ChangedAt], [u3].[Id], [u3].[DateOfBirth], [u3].[Name], [u3].[PrimaryPaymentMethodId], [u2].[ChangeSource]
FROM [UserChanges] AS [u2]
CROSS APPLY (
	SELECT TOP(1) [u4].[Name]
	FROM [UserChanges] AS [u4]
	WHERE [u4].[Id] = 2 AND [u4].[Id] = [u2].[Id] AND [u4].[ChangedAt] < [u2].[ChangedAt]
	ORDER BY [u4].[ChangedAt] DESC
) AS [t1]
LEFT JOIN [Users] AS [u3] ON [u2].[ChangedById] = [u3].[Id]
WHERE [u2].[Id] = 2 AND [t1].[Name] <> [u2].[Name]
UNION ALL
SELECT @__description_2 AS [Description], [p].[Name] AS [OldValue], [p0].[Name] AS [NewValue], [u5].[ChangedAt], [u6].[Id], [u6].[DateOfBirth], [u6].[Name], [u6].[PrimaryPaymentMethodId], [u5].[ChangeSource]
FROM [UserChanges] AS [u5]
CROSS APPLY (
	SELECT TOP(1) [u7].[PrimaryPaymentMethodId]
	FROM [UserChanges] AS [u7]
	WHERE [u7].[Id] = 2 AND [u7].[Id] = [u5].[Id] AND [u7].[ChangedAt] < [u5].[ChangedAt]
	ORDER BY [u7].[ChangedAt] DESC
) AS [t3]
LEFT JOIN [PaymentMethods] AS [p] ON [t3].[PrimaryPaymentMethodId] = [p].[Id]
LEFT JOIN [PaymentMethods] AS [p0] ON [u5].[PrimaryPaymentMethodId] = [p0].[Id]
LEFT JOIN [Users] AS [u6] ON [u5].[ChangedById] = [u6].[Id]
WHERE [u5].[Id] = 2 AND ([p].[Name] <> [p0].[Name] OR ([p].[Name] IS NULL) OR ([p0].[Name] IS NULL)) AND (([p].[Name] IS NOT NULL) OR ([p0].[Name] IS NOT NULL))
```
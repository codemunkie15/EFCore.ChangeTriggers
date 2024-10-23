---
title: Migrations
layout: home
parent: Tracking who made changes
nav_order: 0
---

If you are modifying data via migrations, such as seeding the database, you might want to have a different ChangedBy value for these changes. You can override the below methods in your ChangedByProvider to achieve this.

```c#
public class MyChangedByProvider : ChangedByProvider<User>
{
    // Override this if you're using:
    // await dbContext.Database.MigrateAsync()
    public override Task<User> GetMigrationChangedByAsync()
    {
        return Task.FromResult(new User { Id = 1});
    }

    // Override this if you're using:
    // dbContext.Database.Migrate()
    // or updating the db from the CLI
    // or scripting SQL from the CLI
    public override User GetMigrationChangedBy()
    {
        return new User { Id = 1};
    }
}
```
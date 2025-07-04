---
title: Migrations
layout: home
parent: Tracking what made changes
nav_order: 0
---

If you are modifying data via migrations, such as seeding the database, you might want to have a different ChangeSource value for these changes. You can override the below methods in your ChangeSourceProvider to achieve this.

```c#
public class MyChangeSourceProvider : ChangeSourceProvider<ChangeSource>
{
    // Override this if you're using:
    // await dbContext.Database.MigrateAsync()
    public override Task<ChangeSource> GetMigrationChangeSourceAsync()
    {
        return Task.FromResult(ChangeSource.Migration);
    }

    // Override this if you're using:
    // dbContext.Database.Migrate()
    // or updating the db from the CLI
    // or scripting SQL from the CLI
    public override ChangeSource GetMigrationChangeSource()
    {
        return ChangeSource.Migration;
    }
}
```
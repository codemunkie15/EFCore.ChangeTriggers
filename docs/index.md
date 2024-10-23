---
title: Home
layout: home
nav_order: 0
---

EFCore.ChangeTriggers is an Entity Framework Core extension for storing and querying changes made to entities using SQL triggers and migrations.

### Features

* Auto-generates SQL triggers to track changes for entities, using migrations.
* Captures changes from EF Core (including seed data) and raw SQL queries executed on the database.
* Optional configuration to store who made the change (ChangedBy) and where the change originated from (ChangeSource). 
* Full query support on the entity changes to see previous values. See [EFCore.ChangeTriggers.ChangeEventQueries](https://github.com/codemunkie15/EFCore.ChangeTriggers/tree/main/src/EFCore.ChangeTriggers.ChangeEventQueries) if you need to project change entities into human-readable change events.

### Why use SQL triggers?
Unlike EF Core interceptors, which only capture changes made through the EF context, SQL triggers can also capture updates made through direct SQL commands (including seed data from migrations). This means that no matter how data is modified, whether through an application or directly in the database, you can keep a complete history of entity modifications.

### What about SQL Server Temporal Tables?
The main downside to temporal tables is that you can't add additional metadata columns to the history table (who made the change etc). EFCore.ChangeTriggers is able to inject metadata into the connection that the SQL trigger can then use when storing changes.

### Example populated change table

![Example](https://raw.githubusercontent.com/codemunkie15/EFCore.ChangeTriggers/main/docs/images/Example1.png)
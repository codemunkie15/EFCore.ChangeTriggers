# EFCore.ChangeTriggers

[![Nuget](https://img.shields.io/nuget/v/EFCore.ChangeTriggers)](https://www.nuget.org/packages/EFCore.ChangeTriggers)

EFCore.ChangeTriggers is an Entity Framework Core extension that enables comprehensive entity history tracking using SQL triggers and migrations. Unlike traditional solutions that rely on SaveChanges() interceptors, which only capture changes made via the application, this library can track entity modifications from any source, ensuring that all changes, even those made directly in the database, are accurately recorded.

**SQL Server is currently the only supported EF Core provider.**

## Features
* Auto-generates SQL triggers to track changes for entities, using migrations.
* Captures changes from EF Core (including seed data) and raw SQL queries executed on the database.
* Optional configuration to store who made the change (ChangedBy) and where the change originated from (ChangeSource). 
* Full query support on the entity changes to see previous values. See [EFCore.ChangeTriggers.ChangeEventQueries](https://github.com/codemunkie15/EFCore.ChangeTriggers/tree/main/src/EFCore.ChangeTriggers.ChangeEventQueries) if you need to project change entities into human-readable, individual property change events.

## Example populated change table

![Example](https://raw.githubusercontent.com/codemunkie15/EFCore.ChangeTriggers/main/docs/images/Example1.png)

## Getting started
See the documentation here.

## Why use SQL triggers?
Unlike EF Core interceptors, which only capture changes made through the EF context, SQL triggers can also capture updates made through direct SQL commands (including seed data from migrations). This means that no matter how data is modified, whether through an application or directly in the database, you can keep a complete history of entity modifications.

## What about SQL Server Temporal Tables?
The main downside to temporal tables is that you can't add additional metadata columns to the history table (who made the change etc). EFCore.ChangeTriggers is able to inject metadata into the DB connection, that the SQL trigger can then use when storing changes.
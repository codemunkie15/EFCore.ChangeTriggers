# EntityFrameworkCore.ChangeTrackingTriggers
EntityFrameworkCore.ChangeTrackingTriggers is an EF Core add-on for auditing changes to SQL tables by automatically generating triggers for inserts, updates and deletes. Triggers are automatically created and updated via migrations when the source table schema is modified.

Why use triggers?
The main advantage of using triggers is that any ad-hoc updates to the databases (not using EF Core) are included in change tracking. This can be important if your database often requires manual intervention outside of EF Core and you don't want to miss these changes in your change tracking.

Getting started

Configuration

Sample generated trigger

using _01_FullyFeatured;
using _01_FullyFeatured.DbModels.Permissions;
using _01_FullyFeatured.DbModels.Users;
using ConsoleTables;
using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

[assembly: DesignTimeServicesReference("EntityFrameworkCore.ChangeTrackingTriggers.ChangeTrackingDesignTimeServices, EntityFrameworkCore.ChangeTrackingTriggers")]

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=ChangeTrackingTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
        .UseSqlServerChangeTrackingTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>(options =>
        {
            options.MigrationSourceType = ChangeSourceType.Migration;
        });
});

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<MyDbContext>();

// Examples

var user2 = new User
{
    Name = "My user 2",
    DateOfBirth = "01/01/1990"
};
dbContext.Users.Add(user2);
await dbContext.SaveChangesAsync();
Console.WriteLine("Added new user: My user 2");

dbContext.Users.Remove(user2);
await dbContext.SaveChangesAsync();
Console.WriteLine("Deleted new user: My user 2");
Console.WriteLine();

var permission = new Permission
{
    Name = "My permission",
};
dbContext.Permissions.Add(permission);
await dbContext.SaveChangesAsync();
Console.WriteLine("Added new permission: My permission");

permission.Name = "My updated permission";
await dbContext.SaveChangesAsync();
Console.WriteLine("Updated permission name 'My permission' to 'My updated permission'");
Console.WriteLine();

Console.WriteLine("User changes:");
var userChanges = await dbContext.UserChanges.Include(uc => uc.ChangedBy).ToListAsync();
var userChangesTable = new ConsoleTable("Change Id", "Operation Type", "Change Source", "Changed At", "Changed By", "Id", "Name", "Date of Birth");
foreach (var userChange in userChanges)
{
    userChangesTable.AddRow(userChange.ChangeId, userChange.OperationType, userChange.ChangeSource, userChange.ChangedAt,
        userChange.ChangedBy.Name, userChange.Id, userChange.Name, userChange.DateOfBirth);
}
userChangesTable.Write();
Console.WriteLine();

Console.WriteLine("Permission changes:");
var permissionChanges = await dbContext.PermissionChanges.ToListAsync();
var permissionChangesTable = new ConsoleTable("Change Id", "Operation Type", "Changed At", "Id", "Name");
foreach (var permissionChange in permissionChanges)
{
    permissionChangesTable.AddRow(permissionChange.ChangeId, permissionChange.OperationType, permissionChange.ChangedAt, permissionChange.Id, permissionChange.Name);
}
permissionChangesTable.Write();

Console.ReadLine();

await host.RunAsync();
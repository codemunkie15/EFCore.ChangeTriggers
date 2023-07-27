using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TestHarness;
using TestHarness.DbModels.Permissions;
using TestHarness.DbModels.Users;

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

var queryBuilder = dbContext
    .CreateChangeEventQueryBuilder()
    .AddEntityQuery<UserChange, User, int>(
        dbContext.UserChanges.Where(uc => uc.Id == 1),
        builder =>
        {
            builder.AddProperty("Name changed", e => e.Name);
            builder.AddProperty("Date of birth changed", e => e.DateOfBirth);
        })
    .AddEntityQuery<PermissionChange, Permission, int>(
        dbContext.PermissionChanges,
        builder =>
        {
            builder.AddProperty("Name changed", e => e.Name);
            builder.AddProperty("Order changed", e => e.Order.ToString());
            builder.AddProperty("Reference changed", e => e.Reference.ToString());
            builder.AddProperty("Enabled changed", e => e.Enabled.ToString());
        });

var query = queryBuilder.Build();
var changes = await query.OrderBy(ce => ce.ChangedAt).ToListAsync();

Console.ReadLine();

await host.RunAsync();
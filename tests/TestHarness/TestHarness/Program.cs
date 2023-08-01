using EntityFrameworkCore.ChangeTrackingTriggers.Queries;
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

var query = dbContext
    .CreateChangeEventQueryBuilder<User, ChangeSourceType>()
    .AddEntityQuery(
        dbContext.UserChanges.Where(uc => uc.TrackedEntity.Id == 1),
        builder =>
        {
            builder
                .AddProperty("Name changed", e => e.Name)
                .AddProperty("Date of birth changed", e => e.DateOfBirth);
        })
    .AddEntityQuery(
        dbContext.UserChanges.Where(uc => uc.TrackedEntity.Id == 1),
        builder =>
        {
            builder
                .AddProperty("Name changed", e => e.Name)
                .AddProperty("Date of birth changed", e => e.DateOfBirth);
        })
    .Build();

var query2 = dbContext
    .CreateChangeEventQueryBuilder()
    .AddEntityQuery(
        dbContext.PermissionChanges,
        builder =>
        {
            builder
                .AddProperty("Name changed", e => e.Name)
                .AddProperty("Order changed", e => e.Order.ToString())
                .AddProperty("Reference changed", e => e.Reference.ToString())
                .AddProperty("Enabled changed", e => e.Enabled.ToString());
                //.AddProperty("SomeEntity changed", e => e.SomeEntity.ToString());
        })
    .Build();

var changes1 = await query.OrderBy(ce => ce.ChangedAt).ToListAsync();
var changes2 = await query2.OrderBy(ce => ce.ChangedAt).ToListAsync();

Console.ReadLine();

await host.RunAsync();
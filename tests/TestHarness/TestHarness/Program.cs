using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using EFCore.ChangeTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TestHarness;
using TestHarness.DbModels.Users;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDbContext<MyDbContext>(options =>
    {
        options
            .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
            .UseSqlServerChangeTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>(options =>
            {
                options.MigrationSourceType = ChangeSourceType.Migration;
            });
    })
    .AddScoped<TestData>();

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<MyDbContext>();
var testData = host.Services.GetRequiredService<TestData>();

var query = dbContext
    .CreateChangeEventQueryBuilder<User, ChangeSourceType>()
    .AddChanges(
        dbContext.UserChanges.Where(uc => uc.TrackedEntity.Id == 1),
        builder =>
        {
            builder
                .AddEntityProperties(prop => $"{prop} updated!")
                .AddProperty("Primary payment method changed", e => e.PrimaryPaymentMethod.Name);
        }
    ).Build();

var query2 = dbContext
    .CreateChangeEventQueryBuilder()
    .AddChanges(
        dbContext.PermissionChanges,
        builder =>
        {
            builder
                .AddProperty("Name changed", e => e.Name)
                .AddProperty("Order changed", e => e.Order.ToString())
                .AddProperty("Reference changed", e => e.Reference.ToString())
                .AddProperty("Enabled changed", e => e.Enabled.ToString());
        })
    .Build();

var changes1 = await query.OrderBy(ce => ce.ChangedAt).ToListAsync();
var changes2 = await query2.OrderBy(ce => ce.ChangedAt).ToListAsync();

//await testData.CreateAsync();

Console.ReadLine();

await host.RunAsync();
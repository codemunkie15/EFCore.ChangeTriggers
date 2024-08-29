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
            .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Database=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
            .UseSqlServerChangeTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>();
    })
    .AddScoped<TestDataService>()
    .AddScoped<TestChangeQueriesService>();

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<MyDbContext>();
var testDataService = host.Services.GetRequiredService<TestDataService>();
var testChangeQueriesService = host.Services.GetRequiredService<TestChangeQueriesService>();

await testDataService.CreateAsync();

//await testChangeQueriesService.RunAsync();

Console.ReadLine();

await host.RunAsync();
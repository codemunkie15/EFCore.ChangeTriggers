using BenchmarkDotNet.Running;
using Benchmarks;
using Benchmarks.DbModels.Users;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using EFCore.ChangeTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDbContext<MyDbContext>(options =>
    {
        options
            .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
            .UseSqlServerChangeTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>();
    });

var host = builder.Build();

BenchmarkRunner.Run<ChangeTriggersBenchmarks>();

await host.RunAsync();
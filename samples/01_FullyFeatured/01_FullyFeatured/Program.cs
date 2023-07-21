﻿using _01_FullyFeatured;
using _01_FullyFeatured.DbModels.Permissions;
using _01_FullyFeatured.DbModels.Users;
using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

[assembly: DesignTimeServicesReference("EntityFrameworkCore.ChangeTrackingTriggers.ChangeTrackingDesignTimeServices, EntityFrameworkCore.ChangeTrackingTriggers")]

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<MyDbContext>(options =>
        {
            options
                .UseSqlServer("Data Source=CAZ-DESKTOP\\SQLEXPRESS;Initial Catalog=ChangeTrackingTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
                .UseSqlServerChangeTrackingTriggers<ChangedByProvider, User, ChangeSourceProvider, ChangeSourceType>(options =>
                {
                    options.MigrationSourceType = ChangeSourceType.Migration;
                })
                .EnableSensitiveDataLogging();
        });
    })
    .Build();

var dbContext = host.Services.GetRequiredService<MyDbContext>();

var user = new User
{
    Name = "My user 2",
    DateOfBirth = "01/01/1995"
};
dbContext.Users.Add(user);
await dbContext.SaveChangesAsync();

var user1 = await dbContext.Users.FirstOrDefaultAsync();
user.Name = "Updated";
await dbContext.SaveChangesAsync();

var user2 = await dbContext.Users.Skip(1).FirstOrDefaultAsync();
dbContext.Users.Remove(user);
await dbContext.SaveChangesAsync();

var permission = new Permission
{
    Name = "My second permission",
    Order = 2
};
dbContext.Permissions.Add(permission);
await dbContext.SaveChangesAsync();

Console.ReadLine();

await host.RunAsync();
using _01_FullyFeatured;
using _01_FullyFeatured.DbModels.Orders;
using _01_FullyFeatured.DbModels.Products;
using _01_FullyFeatured.DbModels.Users;
using EFCore.ChangeTriggers.SqlServer;
using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options
        .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False")
        .UseSqlServerChangeTriggers(options =>
        {
            options
                .UseTriggerNameFactory(tableName => $"{tableName}_CustomTriggerName")
                .UseChangedBy<ChangedByProvider, User>()
                .UseChangeSource<ChangeSourceProvider, ChangeSourceType>();
        });
});

var host = builder.Build();

var dbContext = host.Services.GetRequiredService<MyDbContext>();

Console.WriteLine("Writing sample data...");

var user2 = new User { Name = "Bob", DateOfBirth = "01/01/2000" };
var product1 = new Product { Name = "Product 1" };
var product2 = new Product { Name = "Product 2" };

dbContext.Users.Add(user2);
dbContext.Products.Add(product1);
dbContext.Products.Add(product2);

await dbContext.SaveChangesAsync();

product1.Name = "New product 1";

await dbContext.SaveChangesAsync();

var order1 = new Order { Status = "New", Product = product1, User = user2 };
var order2 = new Order { Status = "New", Product = product2, User = user2 };

dbContext.Orders.Add(order1);
dbContext.Orders.Add(order2);

await dbContext.SaveChangesAsync();

order1.Status = "Completed";

await dbContext.SaveChangesAsync();

dbContext.Products.Remove(product2);
await dbContext.SaveChangesAsync();

Console.WriteLine("Sample data added. Check the database to view the change tables.");
Console.ReadLine();

await host.RunAsync();
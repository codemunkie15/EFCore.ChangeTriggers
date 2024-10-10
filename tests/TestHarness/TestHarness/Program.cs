using EFCore.ChangeTriggers.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestHarness;
using TestHarness.DbModels.Users;

[assembly: DesignTimeServicesReference("EFCore.ChangeTriggers.ChangeTriggersDesignTimeServices, EFCore.ChangeTriggers")]

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDbContext<MyDbContext>(options =>
    {
        options
            .UseSqlServer("Data Source=localhost\\SQLEXPRESS;Database=ChangeTriggers;Integrated Security=True;Encrypt=False;TrustServerCertificate=False", options =>
            {
            })
            .UseSqlServerChangeTriggers(options =>
            {
                options
                    .UseTriggerNameFactory(tableName => $"{tableName}_CustomTriggerName")
                    .UseChangedBy<ChangedByProvider, User>()
                    .UseChangeSource<ChangeSourceProvider, ChangeSourceType>();
            });
    })
    .AddScoped(services => new CurrentUserProvider(new User { Id = 7 }))
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
using EFCore.ChangeTriggers.ChangeEventQueries;
using EFCore.ChangeTriggers.SqlServer;
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
                //options.UseCompatibilityLevel()
            })
            .UseSqlServerChangeTriggers(options =>
            {
                options
                    .UseTriggerNameFactory(tableName => $"{tableName}_GlobalCustomTriggerName")
                    .UseChangedBy<ChangedByProvider, User>()
                    .UseChangeSource<ChangeSourceProvider, ChangeSourceType>()
                    .UseChangeEventQueries(typeof(MyDbContext).Assembly, options =>
                    {
                        options
                            .IncludeInserts()
                            .IncludeDeletes();
                    });
            });
    })
    .AddScoped(services => new CurrentUserProvider(new User { Id = 7 }))
    .AddScoped<TestDataService>()
    .AddScoped<TestChangeQueriesService>();

var host = builder.Build();

for (int i = 0; i < 5; i++)
{
    var scope = host.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    var testDataService = scope.ServiceProvider.GetRequiredService<TestDataService>();
    var testChangeQueriesService = scope.ServiceProvider.GetRequiredService<TestChangeQueriesService>();

    //await testDataService.CreateAsync();

    await testChangeQueriesService.RunAsync();
}

Console.ReadLine();

await host.RunAsync();
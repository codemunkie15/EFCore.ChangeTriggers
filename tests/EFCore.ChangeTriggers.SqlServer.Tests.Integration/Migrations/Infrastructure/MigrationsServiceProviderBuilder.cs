using EFCore.ChangeTriggers.SqlServer.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure
{
    internal static class MigrationsServiceProviderBuilder
    {
        public static ServiceProvider Build(string connectionString)
        {
            return new ServiceCollection()
                .AddDbContext<MigrationsDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString)
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options
                                .UseChangedBy<MigrationsChangedByProvider, MigrationsUser>()
                                .UseChangeSource<MigrationsChangeSourceProvider, ChangeSource>();
                        });
                })
                .AddSingleton(new MigrationsCurrentUserProvider())
                .AddSingleton(new MigrationsCurrentChangeSourceProvider())
                .BuildServiceProvider();
        }
    }
}

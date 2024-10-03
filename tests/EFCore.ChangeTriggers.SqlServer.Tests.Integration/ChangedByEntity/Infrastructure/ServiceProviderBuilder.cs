using EFCore.ChangeTriggers.SqlServer.Extensions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure
{
    internal static class ServiceProviderBuilder
    {
        public static ServiceProvider Build(string connectionString)
        {
            return new ServiceCollection()
                .AddDbContext<ChangedByEntityDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString)
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangedBy<ChangedByProvider, EntityUser>();
                        });
                })
                .AddSingleton(new CurrentUserProvider(new EntityUser { Id = 1 }))
                .BuildServiceProvider();
        }
    }
}

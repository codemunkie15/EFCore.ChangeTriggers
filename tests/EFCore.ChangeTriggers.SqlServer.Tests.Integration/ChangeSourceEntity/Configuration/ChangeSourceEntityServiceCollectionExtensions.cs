using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Configuration
{
    internal static class ChangeSourceEntityServiceCollectionExtensions
    {
        public static IServiceCollection AddChangeSourceEntity(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<ChangeSourceEntityDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangeSource<ChangeSourceEntityProvider, ChangeSource>();
                        });
                })
                .AddScoped<EntityChangeSourceProvider>();
        }
    }
}
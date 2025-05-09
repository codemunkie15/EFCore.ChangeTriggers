using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Configuration
{
    internal static class ChangedByEntityServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByEntity(this IServiceCollection serviceCollection, string connectionString)
        {
            return serviceCollection
                .AddDbContext<ChangedByEntityDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangedBy<ChangedByEntityProvider, ChangedByEntityUser>();
                        });
                })
                .AddScoped<EntityCurrentUserProvider>();
        }
    }
}

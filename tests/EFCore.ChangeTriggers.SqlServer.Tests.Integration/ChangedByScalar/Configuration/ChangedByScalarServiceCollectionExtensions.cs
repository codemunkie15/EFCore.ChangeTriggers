using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Configuration
{
    internal static class ChangedByScalarServiceCollectionExtensions
    {
        public static IServiceCollection AddChangedByScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<ChangedByScalarDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangedBy<ChangedByScalarProvider, string>();
                        });
                })
                .AddScoped<ScalarCurrentUserProvider>();
        }
    }
}

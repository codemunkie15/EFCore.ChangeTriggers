using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Configuration
{
    internal static class ChangeSourceScalarServiceCollectionExtensions
    {
        public static IServiceCollection AddChangeSourceScalar(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<ChangeSourceScalarDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(options =>
                        {
                            options.UseChangeSource<ChangeSourceScalarProvider, ChangeSource>();
                        });
                })
                .AddScoped<ScalarChangeSourceProvider>();
        }
    }
}

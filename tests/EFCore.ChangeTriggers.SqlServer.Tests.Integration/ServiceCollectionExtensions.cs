using EFCore.ChangeTriggers.SqlServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerChangeTriggers<TDbContext>(
            this IServiceCollection services,
            string connectionString,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder> optionsAction = null)
            where TDbContext : DbContext
        {
            return services
                .AddDbContext<TDbContext>(options =>
                {
                    options
                        .UseSqlServer(connectionString, options =>
                        {
                            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                        })
                        .UseSqlServerChangeTriggers(optionsAction);
                });
        }
    }
}
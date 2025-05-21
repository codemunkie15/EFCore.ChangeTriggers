using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.SqlServer;
using EFCore.ChangeTriggers.SqlServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerChangeEventQueries<TDbContext>(
            this IServiceCollection services,
            string connectionString,
            Action<ChangeTriggersSqlServerDbContextOptionsBuilder> optionsAction = null,
            Action<ChangeEventsDbContextOptionsBuilder> changeEventQueriesOptionsAction = null)
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
                        .UseSqlServerChangeTriggers(options =>
                        {
                            optionsAction?.Invoke(options);

                            options.UseChangeEventQueries(changeEventQueriesOptionsAction);
                        });
                });
        }
    }
}
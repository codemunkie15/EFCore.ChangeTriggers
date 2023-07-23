using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.CSharp;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ChangeTrackingTriggers
{
    /// <summary>
    /// Design time services required for change tracking trigger migrations.
    /// </summary>
    public class ChangeTrackingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddSingleton<IMigrationsCodeGenerator, ChangeTrackingCSharpMigrationsGenerator>();
            services.AddSingleton<ICSharpMigrationOperationGenerator, ChangeTrackingCSharpMigrationOperationGenerator>();
        }
    }
}

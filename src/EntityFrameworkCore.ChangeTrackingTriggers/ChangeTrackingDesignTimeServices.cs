using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.CSharp;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ChangeTrackingTriggers
{
    internal class ChangeTrackingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddSingleton<IMigrationsCodeGenerator, ChangeTrackingCSharpMigrationsGenerator>();
            services.AddSingleton<ICSharpMigrationOperationGenerator, ChangeTrackingCSharpMigrationOperationGenerator>();
        }
    }
}

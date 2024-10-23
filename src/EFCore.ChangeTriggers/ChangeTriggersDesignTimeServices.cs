using EFCore.ChangeTriggers.Migrations.Design;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers
{
    /// <summary>
    /// Design time services required for change trigger migrations.
    /// </summary>
    public class ChangeTriggersDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddSingleton<IMigrationsCodeGenerator, ChangeTriggersCSharpMigrationsGenerator>();
            services.AddSingleton<ICSharpMigrationOperationGenerator, ChangeTriggersCSharpMigrationOperationGenerator>();
        }
    }
}

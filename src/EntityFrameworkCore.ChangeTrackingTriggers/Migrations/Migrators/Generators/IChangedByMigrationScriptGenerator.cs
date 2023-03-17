using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators
{
    internal interface IChangedByMigrationScriptGenerator
    {
        void Generate(IndentedStringBuilder builder);
    }
}

using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators
{
    internal interface IChangeSourceMigrationScriptGenerator
    {
        void Generate<TChangeSource>(IndentedStringBuilder builder, TChangeSource? migrationSourceType);
    }
}

using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators
{
    internal class ChangeSourceMigrationScriptGenerator : IChangeSourceMigrationScriptGenerator
    {
        public void Generate<TChangeSource>(IndentedStringBuilder builder, TChangeSource? migrationSourceType)
        {
            builder.AppendLine($"EXEC sp_set_session_context '{ChangeTrackingContextConstants.ChangeSourceContextName}', {migrationSourceType}");
        }
    }
}

using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators
{
    internal class ChangedByMigrationScriptGenerator : IChangedByMigrationScriptGenerator
    {
        public void Generate(IndentedStringBuilder builder)
        {
            builder.AppendLine($"EXEC sp_set_session_context '{ChangeTrackingContextConstants.ChangedByContextName}', CHANGEDBYID");
        }
    }
}

using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal class ChangeTrackingTriggersChangedByMigrator : BaseChangeTrackingTriggersMigrator
    {
        private readonly IChangedByMigrationScriptGenerator changedByMigrationScriptGenerator;

        public ChangeTrackingTriggersChangedByMigrator(
            IChangedByMigrationScriptGenerator changedByMigrationScriptGenerator,
            IMigrationsAssembly migrationsAssembly,
            IHistoryRepository historyRepository,
            IDatabaseCreator databaseCreator,
            IMigrationsSqlGenerator migrationsSqlGenerator,
            IRawSqlCommandBuilder rawSqlCommandBuilder,
            IMigrationCommandExecutor migrationCommandExecutor,
            IRelationalConnection connection,
            ISqlGenerationHelper sqlGenerationHelper,
            ICurrentDbContext currentContext,
            IModelRuntimeInitializer modelRuntimeInitializer,
            IDiagnosticsLogger<DbLoggerCategory.Migrations> logger,
            IRelationalCommandDiagnosticsLogger commandLogger,
            IDatabaseProvider databaseProvider)
            : base(
                  migrationsAssembly,
                  historyRepository,
                  databaseCreator,
                  migrationsSqlGenerator,
                  rawSqlCommandBuilder,
                  migrationCommandExecutor,
                  connection,
                  sqlGenerationHelper,
                  currentContext,
                  modelRuntimeInitializer,
                  logger,
                  commandLogger,
                  databaseProvider)
        {
            this.changedByMigrationScriptGenerator = changedByMigrationScriptGenerator;
        }

        protected override void GenerateSetContextScript(IndentedStringBuilder builder)
        {
            changedByMigrationScriptGenerator.Generate(builder);
        }
    }
}

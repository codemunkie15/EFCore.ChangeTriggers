using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal class ChangeTrackingTriggersMigrator<TChangeSource> : BaseChangeTrackingTriggersMigrator
    {
        private readonly ChangeTrackingTriggersOptions<TChangeSource> changeTrackingTriggersOptions;
        private readonly IChangedByMigrationScriptGenerator changedByMigrationScriptGenerator;
        private readonly IChangeSourceMigrationScriptGenerator changeSourceMigrationScriptGenerator;

        public ChangeTrackingTriggersMigrator(
            ChangeTrackingTriggersOptions<TChangeSource> changeTrackingTriggersOptions,
            IChangedByMigrationScriptGenerator changedByMigrationScriptGenerator,
            IChangeSourceMigrationScriptGenerator changeSourceTypeMigrationScriptGenerator,
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
            this.changeTrackingTriggersOptions = changeTrackingTriggersOptions;
            this.changeSourceMigrationScriptGenerator = changeSourceTypeMigrationScriptGenerator;
            this.changedByMigrationScriptGenerator = changedByMigrationScriptGenerator;
        }

        protected override void GenerateSetContextScript(IndentedStringBuilder builder)
        {
            changedByMigrationScriptGenerator.Generate(builder);
            changeSourceMigrationScriptGenerator.Generate(builder, changeTrackingTriggersOptions.MigrationSourceType);
        }
    }
}

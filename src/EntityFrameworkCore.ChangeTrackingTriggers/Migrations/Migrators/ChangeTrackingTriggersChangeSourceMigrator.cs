using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators.Generators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal class ChangeTrackingTriggersChangeSourceMigrator<TSourceType> : BaseChangeTrackingTriggersMigrator
    {
        private readonly ChangeTrackingTriggersOptions<TSourceType> changeTrackingTriggersOptions;
        private readonly IChangeSourceMigrationScriptGenerator changeSourceTypeMigrationScriptGenerator;

        public ChangeTrackingTriggersChangeSourceMigrator(
            ChangeTrackingTriggersOptions<TSourceType> changeTrackingTriggersOptions,
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
            this.changeSourceTypeMigrationScriptGenerator = changeSourceTypeMigrationScriptGenerator;
        }

        protected override void GenerateSetContextScript(IndentedStringBuilder builder)
        {
            changeSourceTypeMigrationScriptGenerator.Generate(builder, changeTrackingTriggersOptions.MigrationSourceType);
        }
    }
}

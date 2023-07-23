using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal class ChangeTrackingTriggersMigrator<TChangedBy, TChangeSource> : BaseChangeTrackingTriggersMigrator
    {
        private readonly ChangeTrackingTriggersOptions<TChangeSource> changeTrackingTriggersOptions;

        public ChangeTrackingTriggersMigrator(
            ChangeTrackingTriggersOptions<TChangeSource> changeTrackingTriggersOptions,
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
        }

        protected override IEnumerable<MigrationOperation> GetSetContextOperations()
        {
            yield return new SetChangeTrackingContextOperation
            { 
                ContextName = ChangeTrackingContextConstants.ChangedByContextName,
                ContextValueType = typeof(TChangedBy)
            };

            yield return new SetChangeTrackingContextOperation
            {
                ContextName = ChangeTrackingContextConstants.ChangeSourceContextName,
                ContextValue = changeTrackingTriggersOptions.MigrationSourceType,
                ContextValueType = typeof(TChangeSource)
            };
        }
    }
}

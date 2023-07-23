using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal class ChangeTrackingTriggersMigrator<TChangedBy, TChangeSource> : BaseChangeTrackingTriggersMigrator
    {
        private readonly ChangeTrackingTriggersOptions<TChangeSource> changeTrackingTriggersOptions;
        private readonly ICurrentDbContext currentContext;

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
            this.currentContext = currentContext;
        }

        protected override IEnumerable<MigrationOperation> GetSetContextOperations()
        {
            yield return GenerateSetChangeTrackingContextOperation<TChangedBy>(ChangeTrackingContextConstants.ChangedByContextName, null);

            yield return GenerateSetChangeTrackingContextOperation<TChangeSource>(
                ChangeTrackingContextConstants.ChangeSourceContextName,
                changeTrackingTriggersOptions.MigrationSourceType);
        }
    }
}

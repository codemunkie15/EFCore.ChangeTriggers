using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ChangeTriggers.Migrations.Migrators
{
    internal class ChangeTriggersChangeSourceMigrator<TChangeSource> : BaseChangeTriggersMigrator
    {
        private readonly ChangeTriggersOptions<TChangeSource> changeTriggersOptions;
        private readonly ICurrentDbContext currentContext;

        public ChangeTriggersChangeSourceMigrator(
            ChangeTriggersOptions<TChangeSource> changeTriggersOptions,
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
            this.changeTriggersOptions = changeTriggersOptions;
            this.currentContext = currentContext;
        }

        protected override IEnumerable<MigrationOperation> GetSetContextOperations()
        {
            yield return GenerateSetChangeContextOperation<TChangeSource>(
                ChangeContextConstants.ChangeSourceContextName,
                changeTriggersOptions.MigrationSourceType);
        }
    }
}

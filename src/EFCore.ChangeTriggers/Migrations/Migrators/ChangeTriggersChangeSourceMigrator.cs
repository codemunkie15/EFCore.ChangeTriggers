using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration.ChangeTriggers;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.EfCoreExtension;
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
        private readonly IChangeSourceProvider<TChangeSource> changeSourceProvider;

        public ChangeTriggersChangeSourceMigrator(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
            IChangeSourceProvider<TChangeSource> changeSourceProvider,
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
                  changeTriggersExtensionContext,
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
            this.changeSourceProvider = changeSourceProvider;
        }

        protected override IEnumerable<MigrationOperation> GetScriptSetContextOperations()
        {
            yield return GenerateSetChangeContextOperation<TChangeSource>(
                ChangeContextConstants.ChangeSourceContextName,
                changeSourceProvider.GetMigrationChangeSource());
        }
    }
}

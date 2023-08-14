using EFCore.ChangeTriggers.EfCoreExtension;
using EFCore.ChangeTriggers.Extensions;
using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.ChangeTriggers.Migrations.Migrators
{
    internal abstract class BaseChangeTriggersMigrator : Migrator
    {
        private readonly ChangeTriggersExtensionContext changeTriggersExtensionContext;
        private readonly IMigrationsSqlGenerator migrationsSqlGenerator;
        protected readonly ISqlGenerationHelper sqlGenerationHelper;
        private readonly ICurrentDbContext currentContext;

        public BaseChangeTriggersMigrator(
            ChangeTriggersExtensionContext changeTriggersExtensionContext,
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
            this.changeTriggersExtensionContext = changeTriggersExtensionContext;
            this.migrationsSqlGenerator = migrationsSqlGenerator;
            this.sqlGenerationHelper = sqlGenerationHelper;
            this.currentContext = currentContext;
        }

        public override void Migrate(string? targetMigration = null)
        {
            changeTriggersExtensionContext.StartMigration();
            base.Migrate(targetMigration);
            changeTriggersExtensionContext.EndMigration();
        }

        public override async Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
        {
            changeTriggersExtensionContext.StartMigration();
            await base.MigrateAsync(targetMigration, cancellationToken);
            changeTriggersExtensionContext.EndMigration();
        }

        public override string GenerateScript(string? fromMigration = null, string? toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        {
            var setContextOperations = GetScriptSetContextOperations();
            var setContextCommands = migrationsSqlGenerator.Generate(setContextOperations.ToList(), currentContext.Context.Model);
            var builder = new IndentedStringBuilder();

            foreach (var command in setContextCommands)
            {
                builder.AppendLines(command.CommandText);
            }

            builder.Append(sqlGenerationHelper.BatchTerminator);
            builder.AppendLines(base.GenerateScript(fromMigration, toMigration, options));

            return builder.ToString();
        }

        protected abstract IEnumerable<MigrationOperation> GetScriptSetContextOperations();

        protected SetChangeContextOperation GenerateSetChangeContextOperation<TContextValue>(string name, object? value)
        {
            return new SetChangeContextOperation
            {
                ContextName = name,
                ContextValue = currentContext.Context.Model.GetRawValue<TContextValue>(value),
                ContextValueType = currentContext.Context.Model.GetRawValueType(typeof(TContextValue))
            };
        }
    }
}

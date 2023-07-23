using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Migrators
{
    internal abstract class BaseChangeTrackingTriggersMigrator : Migrator
    {
        private readonly IMigrationsSqlGenerator migrationsSqlGenerator;
        protected readonly ISqlGenerationHelper sqlGenerationHelper;
        private readonly ICurrentDbContext currentContext;

        public BaseChangeTrackingTriggersMigrator(
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
            this.migrationsSqlGenerator = migrationsSqlGenerator;
            this.sqlGenerationHelper = sqlGenerationHelper;
            this.currentContext = currentContext;
        }

        public override string GenerateScript(string? fromMigration = null, string? toMigration = null, MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        {
            var builder = new IndentedStringBuilder();

            var setContextOperations = GetSetContextOperations();

            var setContextCommands = migrationsSqlGenerator.Generate(setContextOperations.ToList(), currentContext.Context.Model);

            foreach (var command in setContextCommands)
            {
                builder.AppendLines(command.CommandText);
            }

            builder.Append(sqlGenerationHelper.BatchTerminator);

            builder.AppendLines(base.GenerateScript(fromMigration, toMigration, options));

            return builder.ToString();
        }

        protected abstract IEnumerable<MigrationOperation> GetSetContextOperations();
    }
}

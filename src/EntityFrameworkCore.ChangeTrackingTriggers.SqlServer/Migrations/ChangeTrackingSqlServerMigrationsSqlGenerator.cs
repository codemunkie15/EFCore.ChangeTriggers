using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;
using Scriban.Runtime;
using Scriban;
using System.Reflection;
using EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Templates;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Models;
using Azure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Migrations
{
    internal class ChangeTrackingSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        private readonly IEnumerable<ChangeConfig> changeConfigs = new List<ChangeConfig>()
        {
            new ChangeConfig((int)OperationType.Insert, "inserted", "i"),
            new ChangeConfig((int)OperationType.Update, "inserted", "i"),
            new ChangeConfig((int)OperationType.Delete, "deleted", "d"),
        };

        public ChangeTrackingSqlServerMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, ICommandBatchPreparer commandBatchPreparer)
            : base(dependencies, commandBatchPreparer)
        {
        }

        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            if (operation is CreateChangeTrackingTriggerOperation createChangeTrackingTriggerOperation)
            {
                Generate(createChangeTrackingTriggerOperation, builder);
            }
            else if (operation is DropChangeTrackingTriggerOperation dropChangeTrackingTriggerOperation)
            {
                Generate(dropChangeTrackingTriggerOperation, builder);
            }
            else if (operation is NoCheckConstraintOperation noCheckConstraintOperation)
            {
                Generate(noCheckConstraintOperation, model, builder);
            }
            else if (operation is SetChangeTrackingContextOperation setChangeTrackingContextOperation)
            {
                Generate(setChangeTrackingContextOperation, model, builder);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }

        protected virtual void Generate(CreateChangeTrackingTriggerOperation operation, MigrationCommandListBuilder builder)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Templates.CreateChangeTrackingTriggerSqlTemplate.sql");
            using var reader = new StreamReader(stream);
            var sqlTemplate = reader.ReadToEnd();

            var template = Template.Parse(sqlTemplate);

            var scriptObject = new ScriptObject
            {
                { "change_tracking_context_changed_by_name", ChangeTrackingContextConstants.ChangedByContextName },
                { "change_tracking_context_change_source_name", ChangeTrackingContextConstants.ChangeSourceContextName },
                { "tracked_table_name", operation.TrackedTableName },
                { "change_table_name", operation.ChangeTableName },
                { "change_configs", changeConfigs },
                { "trigger_name", operation.TriggerName },
                { "change_table_data_columns", operation.ChangeTableDataColumns },
                { "primary_key_column_names", operation.TrackedTablePrimaryKeyColumns },
                { "operation_type_column", operation.OperationTypeColumn },
                { "change_source_column", operation.ChangeSourceColumn },
                { "changed_by_column", operation.ChangedByColumn },
                { "changed_at_column", operation.ChangedAtColumn }
            };

            var context = new TemplateContext();
            context.PushGlobal(scriptObject);
            context.AutoIndent = false;

            var sql = template.Render(context);

            builder.AppendLine(sql);
            builder.EndCommand();
        }

        protected virtual void Generate(DropChangeTrackingTriggerOperation operation, MigrationCommandListBuilder builder)
        {
            builder
                .Append($"DROP TRIGGER [dbo].[{operation.TriggerName}]")
                .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
                .EndCommand();
        }

        protected virtual void Generate(NoCheckConstraintOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            builder
                .Append("ALTER TABLE ")
                .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema))
                .Append(" NOCHECK CONSTRAINT ")
                .Append(Dependencies.SqlGenerationHelper.DelimitIdentifier(operation.Constraint))
                .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
                .EndCommand();
        }

        protected virtual void Generate(SetChangeTrackingContextOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            var nameTypeMapping = Dependencies.TypeMappingSource.FindMapping(typeof(string))!;
            var valueTypeMapping = Dependencies.TypeMappingSource.FindMapping(operation.ContextValueType);

            if (valueTypeMapping == null)
            {
                throw new InvalidOperationException($"The change context type {operation.ContextValueType} is not supported by the provider.");
            }

            builder
                .Append("EXEC sp_set_session_context ")
                .Append(nameTypeMapping.GenerateSqlLiteral(operation.ContextName))
                .Append(", ")
                .Append(valueTypeMapping.GenerateSqlLiteral(operation.ContextValue))
                .AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator)
                .EndCommand();
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;
using Scriban.Runtime;
using Scriban;
using System.Reflection;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Models;
using EFCore.ChangeTriggers.Migrations.Operations;
using EFCore.ChangeTriggers.SqlServer.Templates;

namespace EFCore.ChangeTriggers.SqlServer.Migrations
{
    internal class ChangeTriggersSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        private readonly IEnumerable<ChangeConfig> changeConfigs = new List<ChangeConfig>()
        {
            new ChangeConfig((int)OperationType.Insert, "inserted", "i"),
            new ChangeConfig((int)OperationType.Update, "inserted", "i"),
            new ChangeConfig((int)OperationType.Delete, "deleted", "d"),
        };

        public ChangeTriggersSqlServerMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, ICommandBatchPreparer commandBatchPreparer)
            : base(dependencies, commandBatchPreparer)
        {
        }

        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            if (operation is CreateChangeTriggerOperation createChangeTriggerOperation)
            {
                Generate(createChangeTriggerOperation, builder);
            }
            else if (operation is DropChangeTriggerOperation dropChangeTriggerOperation)
            {
                Generate(dropChangeTriggerOperation, builder);
            }
            else if (operation is NoCheckConstraintOperation noCheckConstraintOperation)
            {
                Generate(noCheckConstraintOperation, model, builder);
            }
            else if (operation is SetChangeContextOperation setChangeContextOperation)
            {
                Generate(setChangeContextOperation, model, builder);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }

        protected virtual void Generate(CreateChangeTriggerOperation operation, MigrationCommandListBuilder builder)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EntityFrameworkCore.ChangeTriggers.SqlServer.Templates.CreateChangeTriggerSqlTemplate.sql");
            using var reader = new StreamReader(stream);
            var sqlTemplate = reader.ReadToEnd();

            var template = Template.Parse(sqlTemplate);

            var scriptObject = new ScriptObject
            {
                { "change_context_changed_by_name", ChangeContextConstants.ChangedByContextName },
                { "change_context_change_source_name", ChangeContextConstants.ChangeSourceContextName },
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

        protected virtual void Generate(DropChangeTriggerOperation operation, MigrationCommandListBuilder builder)
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

        protected virtual void Generate(SetChangeContextOperation operation, IModel model, MigrationCommandListBuilder builder)
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

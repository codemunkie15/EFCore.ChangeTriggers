using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    public static class MigrationBuilderExtensions
    {
        public static OperationBuilder<CreateChangeTriggerOperation> CreateChangeTrigger(
            this MigrationBuilder migrationBuilder,
            string trackedTableName,
            string changeTableName,
            string triggerName,
            IEnumerable<string> trackedTablePrimaryKeyColumns,
            IEnumerable<string> changeTableDataColumns,
            ChangeContextColumn operationTypeColumn,
            ChangeContextColumn changedAtColumn,
            ChangeContextColumn? changeSourceColumn = null,
            ChangeContextColumn? changedByColumn = null)
        {
            var operation = new CreateChangeTriggerOperation
            {
                TrackedTableName = trackedTableName,
                ChangeTableName = changeTableName,
                TriggerName = triggerName,
                TrackedTablePrimaryKeyColumns = trackedTablePrimaryKeyColumns,
                ChangeTableDataColumns = changeTableDataColumns,
                OperationTypeColumn = operationTypeColumn,
                ChangeSourceColumn = changeSourceColumn,
                ChangedAtColumn = changedAtColumn,
                ChangedByColumn = changedByColumn
            };

            migrationBuilder.Operations.Add(operation);

            return new OperationBuilder<CreateChangeTriggerOperation>(operation);
        }

        public static OperationBuilder<DropChangeTriggerOperation> DropChangeTrigger(
            this MigrationBuilder migrationBuilder,
            string triggerName)
        {
            var operation = new DropChangeTriggerOperation
            {
                TriggerName = triggerName
            };

            migrationBuilder.Operations.Add(operation);

            return new OperationBuilder<DropChangeTriggerOperation>(operation);
        }

        public static OperationBuilder<NoCheckConstraintOperation> AddNoCheckConstraint(
            this MigrationBuilder migrationBuilder,
            string table,
            string constraint,
            string? schema = null)
        {
            var operation = new NoCheckConstraintOperation
            {
                Table = table,
                Constraint = constraint,
                Schema = schema
            };

            migrationBuilder.Operations.Add(operation);

            return new OperationBuilder<NoCheckConstraintOperation>(operation);
        }
    }
}

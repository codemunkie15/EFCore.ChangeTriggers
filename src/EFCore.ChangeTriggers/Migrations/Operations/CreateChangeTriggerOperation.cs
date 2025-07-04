using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    public class CreateChangeTriggerOperation : MigrationOperation
    {
        public required string TrackedTableName { get; set; }

        public required string ChangeTableName { get; set; }

        public required string TriggerName { get; set; }

        public required IEnumerable<string> TrackedTablePrimaryKeyColumns { get; set; }

        public required IEnumerable<string> ChangeTableDataColumns { get; set; }

        public required ChangeContextColumn OperationTypeColumn { get; set; }

        public ChangeContextColumn? ChangeSourceColumn { get; set; }

        public required ChangeContextColumn ChangedAtColumn { get; set; }

        public ChangeContextColumn? ChangedByColumn { get; set; }
    }
}
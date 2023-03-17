using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations
{
    public class CreateChangeTrackingTriggerOperation : MigrationOperation
    {
        public string TrackedTableName { get; set; }

        public string ChangeTableName { get; set; }

        public string TriggerName { get; set; }

        public IEnumerable<string> TrackedTablePrimaryKeyColumns { get; set; }

        public IEnumerable<string> ChangeTableDataColumns { get; set; }

        public ChangeContextColumn OperationTypeColumn { get; set; }

        public ChangeContextColumn? ChangeSourceColumn { get; set; }

        public ChangeContextColumn ChangedAtColumn { get; set; }

        public ChangeContextColumn? ChangedByColumn { get; set; }
    }
}
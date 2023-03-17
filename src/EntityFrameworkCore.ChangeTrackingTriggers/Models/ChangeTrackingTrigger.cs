using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Models
{
    internal class ChangeTrackingTrigger : IEquatable<ChangeTrackingTrigger>
    {
        public string Name { get; set; }

        public IEnumerable<string> ChangeTableDataColumns { get; set; }

        public IEnumerable<string> TrackedTablePrimaryKeyColumns { get; set; }

        public ChangeContextColumn OperationTypeColumn { get; set; }

        public ChangeContextColumn? ChangeSourceColumn { get; set; }

        public ChangeContextColumn ChangedAtColumn { get; set; }

        public ChangeContextColumn? ChangedByColumn { get; set; }

        public bool Equals(ChangeTrackingTrigger? other)
        {
            if (other == null)
            {
                return false;
            }

            // Order of columns doesn't matter for equality
            return
                string.Equals(Name, other.Name) &&
                Enumerable.SequenceEqual(ChangeTableDataColumns.OrderBy(c => c), other.ChangeTableDataColumns.OrderBy(c => c)) &&
                Enumerable.SequenceEqual(TrackedTablePrimaryKeyColumns.OrderBy(c => c), other.TrackedTablePrimaryKeyColumns.OrderBy(c => c)) &&
                object.Equals(OperationTypeColumn, other.OperationTypeColumn) &&
                object.Equals(ChangeSourceColumn, other.ChangeSourceColumn) &&
                object.Equals(ChangedAtColumn, other.ChangedAtColumn) &&
                object.Equals(ChangedByColumn, other.ChangedByColumn);
        }

        public override bool Equals(object? obj) => this.Equals(obj as ChangeTrackingTrigger);

        public override int GetHashCode() => HashCode.Combine(
            Name,
            ChangeTableDataColumns,
            TrackedTablePrimaryKeyColumns,
            OperationTypeColumn,
            ChangeSourceColumn,
            ChangedAtColumn,
            ChangedByColumn);

        public static bool operator ==(ChangeTrackingTrigger? left, ChangeTrackingTrigger? right) => Equals(left, right);

        public static bool operator !=(ChangeTrackingTrigger? left, ChangeTrackingTrigger? right) => !Equals(left, right);
    }
}
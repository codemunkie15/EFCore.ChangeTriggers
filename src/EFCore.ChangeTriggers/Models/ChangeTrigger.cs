using EFCore.ChangeTriggers.Migrations.Operations;

namespace EFCore.ChangeTriggers.Models
{
    internal class ChangeTrigger : IEquatable<ChangeTrigger>
    {
        public required string Name { get; set; }

        public required IEnumerable<string> ChangeTableDataColumns { get; set; }

        public required IEnumerable<string> TrackedTablePrimaryKeyColumns { get; set; }

        public required ChangeContextColumn OperationTypeColumn { get; set; }

        public required ChangeContextColumn? ChangeSourceColumn { get; set; }

        public required ChangeContextColumn ChangedAtColumn { get; set; }

        public required ChangeContextColumn? ChangedByColumn { get; set; }

        public bool Equals(ChangeTrigger? other)
        {
            if (other == null)
            {
                return false;
            }

            // Order of columns doesn't matter for equality
            return
                string.Equals(Name, other.Name) &&
                ChangeTableDataColumns.OrderBy(c => c).SequenceEqual(other.ChangeTableDataColumns.OrderBy(c => c)) &&
                TrackedTablePrimaryKeyColumns.OrderBy(c => c).SequenceEqual(other.TrackedTablePrimaryKeyColumns.OrderBy(c => c)) &&
                Equals(OperationTypeColumn, other.OperationTypeColumn) &&
                Equals(ChangeSourceColumn, other.ChangeSourceColumn) &&
                Equals(ChangedAtColumn, other.ChangedAtColumn) &&
                Equals(ChangedByColumn, other.ChangedByColumn);
        }

        public override bool Equals(object? obj) => Equals(obj as ChangeTrigger);

        public override int GetHashCode() => HashCode.Combine(
            Name,
            ChangeTableDataColumns,
            TrackedTablePrimaryKeyColumns,
            OperationTypeColumn,
            ChangeSourceColumn,
            ChangedAtColumn,
            ChangedByColumn);

        public static bool operator ==(ChangeTrigger? left, ChangeTrigger? right) => Equals(left, right);

        public static bool operator !=(ChangeTrigger? left, ChangeTrigger? right) => !Equals(left, right);
    }
}
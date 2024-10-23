using EFCore.ChangeTriggers;
using EFCore.ChangeTriggers.Abstractions;

namespace TestHarness.DbModels.Permissions
{
    public class PermissionChange : PermissionBase, IChange<Permission>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Permission TrackedEntity { get; set; }
    }
}

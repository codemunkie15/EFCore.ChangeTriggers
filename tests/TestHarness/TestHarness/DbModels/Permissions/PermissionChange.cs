using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;
using System;

namespace TestHarness.DbModels.Permissions
{
    public class PermissionChange : PermissionBase, IChange<Permission, int>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Permission TrackedEntity { get; set; }
    }
}

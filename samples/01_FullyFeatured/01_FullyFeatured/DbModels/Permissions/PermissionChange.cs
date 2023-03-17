using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Models;
using System;

namespace _01_FullyFeatured.DbModels.Permissions
{
    public class PermissionChange : PermissionBase, IChange<Permission, int>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Permission TrackedEntity { get; set; }
    }
}

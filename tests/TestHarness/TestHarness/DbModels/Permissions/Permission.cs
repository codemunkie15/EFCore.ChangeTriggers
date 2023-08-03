using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Collections.Generic;
using TestHarness.DbModels.Users;

namespace TestHarness.DbModels.Permissions
{
    public class Permission : PermissionBase, ITracked<PermissionChange>
    {
        public ICollection<User> Users { get; set; }

        public ICollection<PermissionChange> Changes { get; set; }
    }
}

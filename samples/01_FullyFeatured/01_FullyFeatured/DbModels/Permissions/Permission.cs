using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Collections.Generic;

namespace _01_FullyFeatured.DbModels.Permissions
{
    public class Permission : PermissionBase, ITracked<PermissionChange>
    {
        public ICollection<PermissionChange> Changes { get; set; }
    }
}

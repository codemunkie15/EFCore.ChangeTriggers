﻿using EFCore.ChangeTriggers.Abstractions;
using TestHarness.DbModels.Users;

namespace TestHarness.DbModels.Permissions
{
    public class Permission : PermissionBase, ITracked<PermissionChange>
    {
        public ICollection<User> Users { get; set; }

        public ICollection<PermissionChange> Changes { get; set; }
    }
}

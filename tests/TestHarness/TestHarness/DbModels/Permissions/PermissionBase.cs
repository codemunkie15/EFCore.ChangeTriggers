using System;

namespace TestHarness.DbModels.Permissions
{
    public abstract class PermissionBase
    {
        public int Id { get; set; }

        public int SubId { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public Guid Reference { get; set; }

        public bool Enabled { get; set; }
    }
}

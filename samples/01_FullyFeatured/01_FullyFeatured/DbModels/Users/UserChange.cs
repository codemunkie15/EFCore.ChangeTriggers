using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;
using System;

namespace _01_FullyFeatured.DbModels.Users
{
    public class UserChange : UserBase, IChange<User, int>, IHasChangeSource<ChangeSourceType>, IHasChangedBy<User>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public ChangeSourceType ChangeSource { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public User ChangedBy { get; set; }

        public User TrackedEntity { get; set; }
    }
}
using _01_FullyFeatured.DbModels.Users;
using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;
using System;

namespace _01_FullyFeatured.DbModels.Orders
{
    public class OrderChange : OrderBase, IChange<Order, int>, IHasChangedBy<User>, IHasChangeSource<ChangeSourceType>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Order TrackedEntity { get; set; }

        public User ChangedBy { get; set; }

        public ChangeSourceType ChangeSource { get; set; }
    }
}

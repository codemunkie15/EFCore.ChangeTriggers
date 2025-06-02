using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain
{
    public class UserChange : UserBase, IHasChangeId, IChange<User>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public User TrackedEntity { get; set; }
    }
}

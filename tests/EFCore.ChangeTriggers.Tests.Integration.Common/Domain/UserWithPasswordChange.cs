using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain
{
    public class UserWithPasswordChange : UserBase, IHasChangeId, IChange<UserWithPassword>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public UserWithPassword TrackedEntity { get; set; }
    }
}

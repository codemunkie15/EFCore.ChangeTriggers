using EFCore.ChangeTriggers;
using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore.Models.Users
{
    public class UserChange : BaseUser, IChange<User>, IHasChangedBy<User>, IHasChangeSource<ChangeSource>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public User TrackedEntity { get; set; }

        public User ChangedBy { get; set; }

        public ChangeSource ChangeSource { get; set; }
    }
}
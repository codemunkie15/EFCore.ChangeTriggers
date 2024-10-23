using EFCore.ChangeTriggers;
using EFCore.ChangeTriggers.Abstractions;

namespace TestHarness.DbModels.Users
{
    public class UserChange : UserBase, IChange<User>, IHasChangeSource<string>, IHasChangedBy<User>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public string ChangeSource { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public User ChangedBy { get; set; }

        public User TrackedEntity { get; set; }
    }
}
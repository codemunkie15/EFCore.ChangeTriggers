using A_AspNetCore.Models.Users;
using EFCore.ChangeTriggers;
using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore.Models.Roles
{
    public class RoleChange : BaseRole, IChange<Role>, IHasChangedBy<User>, IHasChangeSource<ChangeSource>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Role TrackedEntity { get; set; }

        public User ChangedBy { get; set; }

        public ChangeSource ChangeSource { get; set; }
    }
}

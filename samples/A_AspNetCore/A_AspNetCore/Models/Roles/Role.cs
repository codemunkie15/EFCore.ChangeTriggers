using A_AspNetCore.Models.Users;
using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore.Models.Roles
{
    public class Role : BaseRole, ITracked<RoleChange>
    {
        public ICollection<User>? Users { get; set; }
        public ICollection<RoleChange>? Changes { get; set; }
    }
}

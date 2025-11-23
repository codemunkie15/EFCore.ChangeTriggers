using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain
{
    public class UserWithPassword : UserBase, ITracked<UserWithPasswordChange>
    {
        public string PasswordHash { get; set; }

        public ICollection<UserWithPasswordChange> Changes { get; set; }

        public static UserWithPassword SystemUser { get; } = new UserWithPassword { Id = 1, Username = nameof(SystemUser) };
    }
}

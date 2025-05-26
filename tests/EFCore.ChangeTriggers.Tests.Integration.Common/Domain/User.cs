using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain
{
    public class User : BaseUser, ITracked<UserChange>
    {
        public ICollection<UserChange> Changes { get; set; }

        public static User SystemUser { get; } = new User { Id = 1 };
    }
}
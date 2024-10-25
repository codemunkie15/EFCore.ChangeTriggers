using EFCore.ChangeTriggers.Abstractions;

namespace A_AspNetCore.Models.Users
{
    public class User : BaseUser, ITracked<UserChange>
    {
        public ICollection<UserChange>? Changes { get; set; }
    }
}
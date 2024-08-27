using EFCore.ChangeTriggers.Abstractions;

namespace Benchmarks.DbModels.Users
{
    public class User : UserBase, ITracked<UserChange>
    {
        public User()
        {
        }

        public ICollection<UserChange> Changes { get; set; }
    }
}
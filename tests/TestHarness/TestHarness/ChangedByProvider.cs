using EFCore.ChangeTriggers.Abstractions;
using System.Threading.Tasks;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class ChangedByProvider : ChangedByProvider<User>
    {
        public override Task<User> GetChangedByAsync()
        {
            return Task.FromResult(new User { Id = 1 });
        }

        public override User GetMigrationChangedBy()
        {
            return new User { Id = 2 };
        }
    }
}

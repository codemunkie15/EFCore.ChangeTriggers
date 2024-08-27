using Benchmarks.DbModels.Users;
using EFCore.ChangeTriggers.Abstractions;

namespace Benchmarks
{
    internal class ChangedByProvider : ChangedByProvider<User>
    {
        public override User GetChangedBy()
        {
            return new User { Id = 1 };
        }

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

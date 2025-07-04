using EFCore.ChangeTriggers.Abstractions;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class ChangedByProvider : ChangedByProvider<User>
    {
        private readonly CurrentUserProvider currentUserProvider;

        public ChangedByProvider(CurrentUserProvider currentUserProvider)
        {
            this.currentUserProvider = currentUserProvider;
        }

        public override Task<User> GetChangedByAsync()
        {
            return Task.FromResult(currentUserProvider.GetCurrentUser());
        }

        public override User GetMigrationChangedBy()
        {
            return currentUserProvider.GetCurrentUser();
        }
    }
}
